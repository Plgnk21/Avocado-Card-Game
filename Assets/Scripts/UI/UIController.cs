using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMesh playerCards, playerHP, enemyCards, enemyHP, resultText;
    public Transform ResultsPanel;
    public Collider2D fieldBtn;
    public Collider2D[] panelButtons; 

    bool isPanelShowed;
    
    public void refreshCards(bool isEnemyCards, int cards)
    {
        if (isEnemyCards)
            enemyCards.text = cards.ToString();
        else
            playerCards.text = cards.ToString();
    }

    public void refreshHP(bool isEnemyHP, int hP)
    {
        if (isEnemyHP)
            enemyHP.text = hP.ToString();
        else
            playerHP.text = hP.ToString();
    }

    public void ShowResults(bool isVictory)
    {
        ResultsPanel.DOMoveY(0.0f, 1.0f);
        isPanelShowed = true;
        ActivateFieldButtons(false);

        if (isVictory)
            resultText.text = "Victory";
        else
            resultText.text = "You lose";

        for (int i = 0; i < panelButtons.Length; i++)
            panelButtons[i].enabled = true;        
    }

    public void HideResults()
    {
        if (isPanelShowed)
        {
            isPanelShowed = false;
            ResultsPanel.DOMoveY(10.0f, 1.0f).OnComplete(()=>
            ActivateFieldButtons(true));            
        }

        for (int i = 0; i < panelButtons.Length; i++)
            panelButtons[i].enabled = false;
    }

    public void ActivateFieldButtons(bool isActivate)
    {
        if (isActivate)
            fieldBtn.enabled = true;
        else
            fieldBtn.enabled = false;
    }
}
