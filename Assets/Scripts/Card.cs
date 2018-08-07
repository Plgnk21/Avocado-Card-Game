using DG.Tweening;
using System;
using UnityEngine;

public class Card : MonoBehaviour 
{
    public bool[] attackDirections; // 0-N, 1-NE, ---, 7-NW    
    public int healthPoints, currentBattleHP, damage;
    public bool isEnemyCard { get; set; }
    public Vector3 startPos { get; set; }
    public SpriteRenderer cardBack, frame;
    public CardPosition[] positions;
    public CardPosition currPosition;
    public GameController controller;
    public TextMesh hPoints, dmgPoints;

    Collider2D cardCollider;
    FreeDragBehaviours dragBehaviour;

    void Start ()
    {        
        cardCollider = GetComponent<Collider2D>();
        dragBehaviour = GetComponent<FreeDragBehaviours>();
        dragBehaviour.onEndDrag.AddAction(OnEndDrag);
        dragBehaviour.onBeginDrag.AddAction(() => ChangeZ(true));        
        dmgPoints.text = damage.ToString();
        currentBattleHP = healthPoints;
        RefreshHP();
    }

    public void CloseCard()
    {
        cardCollider.enabled = false;
        currentBattleHP = healthPoints;
        cardBack.DOFade(1.0f, 0.1f);
        frame.DOFade(1.0f, 0.1f);
        frame.color = Color.white;
    }

    public void ActivateCard()
    {
        if (isEnemyCard)
        {
            frame.color = Color.red;
            frame.DOFade(1.0f, 0.15f);
        }
        else
        {
            frame.color = Color.green;
            frame.DOFade(1.0f, 0.15f);
            cardBack.DOFade(0.0f, 0.3f);
        }        
    }

    public void ActivateCollider(bool isActivate)
    {
        if (isActivate)
        {
            cardCollider.enabled = true;
            frame.DOFade(0.0f, 0.5f).OnComplete(()=> frame.DOFade(1.0f, 0.5f));
        }
        else
            cardCollider.enabled = false;
    }

    public void RefreshHP()
    {
        hPoints.text = currentBattleHP.ToString();
    }

    public void AttackCard(Card attackedCard)
    {
        if (isEnemyCard != attackedCard.isEnemyCard)
        {
            attackedCard.currentBattleHP -= damage;
            attackedCard.transform.DOShakePosition(0.4f, new Vector3(0.15f, 0.15f, 0.0f)).OnComplete(() =>
            {
                if (attackedCard.currentBattleHP > 0)
                {
                    attackedCard.RefreshHP();
                }
                else
                {
                    AudioManager.Instance.PlaySound("Break");
                    attackedCard.currPosition.currCard = null;
                    attackedCard.gameObject.SetActive(false);
                }
            });
        }
    }

    void OnEndDrag()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (Math.Abs(positions[i].transform.position.x - transform.position.x) < 1.0f &&
                Math.Abs(positions[i].transform.position.y - transform.position.y) < 1.0f &&
                positions[i].currCard == null)
            {
                ActivateCollider(false);
                transform.position = positions[i].transform.position;
                currPosition.currCard = null;
                currPosition = positions[i];
                currPosition.currCard = this;
                AudioManager.Instance.PlaySound("Tap");
                controller.AddCard(this);
                if (isEnemyCard)
                    cardBack.DOFade(0.0f, 0.3f);
                return;
            }            
        }

        ActivateCollider(false);
        transform.DOMove(startPos, 0.3f).OnComplete(() =>
        {
            ActivateCollider(true);
            ChangeZ(false);
        });
    }

    void ChangeZ(bool isUp)
    {
        Vector3 pos = transform.localPosition;
        if (isUp)
            transform.localPosition = new Vector3(pos.x, pos.y, -0.2f);
        else
            transform.localPosition = new Vector3(pos.x, pos.y, -0.01f);
    }    
}
