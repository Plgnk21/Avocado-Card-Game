using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    public Transform enemyCardDeckPos, playerCardDeckPos;
    public List<CardPosition> playerHandSlots, enemyHandSlots;
    public List<Card> cards;
    public Stack<Card> enemyCardDeck = new Stack<Card>(), playerCardDeck = new Stack<Card>();
    public GameController gameController;
    public UIController uIController;

    void Start ()
    {
        Invoke("ResetCards", 1.0f);
    }

    public void ResetCards()
    {        
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].gameObject.SetActive(true);
            cards[i].CloseCard();
        }
        Shuffle();
        SortToDecks();
        CardToHands();
    }
    
    public void CardToHands()
    {        
        StartCoroutine(GiveOutCards());
    }

    IEnumerator GiveOutCards()
    {
        for (int i = 0; i < enemyHandSlots.Count; i++)
        {
            if (enemyHandSlots[i].currCard == null && enemyCardDeck.Count > 0)
            {                
                Card card = enemyCardDeck.Pop();
                uIController.refreshCards(true, enemyCardDeck.Count);
                enemyHandSlots[i].currCard = card;
                card.currPosition = enemyHandSlots[i];
                AudioManager.Instance.PlaySound("Hmm");
                card.transform.DOMove(enemyHandSlots[i].transform.position, 0.3f).OnComplete(()=>
                {
                    card.startPos = card.transform.position;
                    card.ActivateCard();
                });
                card.transform.DOScale(1.0f, 0.3f);
                yield return new WaitForSeconds(0.3f);
            }
        }

        for (int i = 0; i < playerHandSlots.Count; i++)
        {
            if (playerHandSlots[i].currCard == null && playerCardDeck.Count > 0)
            {
                Card card = playerCardDeck.Pop();
                uIController.refreshCards(false, playerCardDeck.Count);
                playerHandSlots[i].currCard = card;
                card.currPosition = playerHandSlots[i];
                AudioManager.Instance.PlaySound("Hmm");
                card.transform.DOMove(playerHandSlots[i].transform.position, 0.3f).OnComplete(() =>
                {
                    card.startPos = card.transform.position;
                    card.ActivateCard();
                });
                card.transform.DOScale(1.0f, 0.3f);
                yield return new WaitForSeconds(0.3f);
            }
        }

        gameController.ChangeTurn();
        uIController.ActivateFieldButtons(true);
    }

    void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            var value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    void SortToDecks()
    {
        if (enemyCardDeck.Count != 0)
            enemyCardDeck.Clear();
        if (playerCardDeck.Count != 0)
            playerCardDeck.Clear();

        int count = cards.Count;
        for (int i = 0; i < count; i++)
        {
            if (i < count / 2)
            {
                cards[i].isEnemyCard = true;
                enemyCardDeck.Push(cards[i]);
                cards[i].transform.position = enemyCardDeckPos.position;
                cards[i].transform.DOScale(0.5f, 0.1f);
            }
            else
            {
                cards[i].isEnemyCard = false;
                playerCardDeck.Push(cards[i]);
                cards[i].transform.position = playerCardDeckPos.position;
                cards[i].transform.DOScale(0.5f, 0.1f);
            }
        }
        uIController.refreshCards(true, enemyCardDeck.Count);
        uIController.refreshCards(false, playerCardDeck.Count);
    }
}
