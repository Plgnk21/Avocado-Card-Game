using UnityEngine;

public class GameController : MonoBehaviour
{    
    public const int fieldSize = 3;
    public CardPosition[] positions;
    public CardsController cardsController;
    public UIController uIController;

    bool isEnemyTurn;
    int playerTotalHP, enemyTotalHP;

    CardPosition[,] positionsField = new CardPosition[3,3];

    void Start ()
    {
        CreateField();        
    }

    public void ResetGame()
    {
        for (int i = 0; i < cardsController.playerHandSlots.Count; i++)
            cardsController.playerHandSlots[i].currCard = null;

        for (int i = 0; i < cardsController.enemyHandSlots.Count; i++)
            cardsController.enemyHandSlots[i].currCard = null;

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].currCard = null;
        }

        cardsController.ResetCards();
        CheckHP();
    }

    public void ChangeTurn()
    {
        isEnemyTurn = !isEnemyTurn;
        if (isEnemyTurn)
        {
            for (int i = 0; i < cardsController.playerHandSlots.Count; i++)
            {
                if (cardsController.enemyHandSlots[i].currCard != null)
                    cardsController.enemyHandSlots[i].currCard.ActivateCollider(true);
            }
        }
        else
        {
            for (int i = 0; i < cardsController.playerHandSlots.Count; i++)
            {
                if (cardsController.playerHandSlots[i].currCard != null)
                    cardsController.playerHandSlots[i].currCard.ActivateCollider(true);
            }
        }
    }

    internal void AddCard(Card card)
    {
        for (int i = 0; i < cardsController.cards.Count; i++)
        {
            cardsController.cards[i].ActivateCollider(false);
        }

        for (int i = 0; i < card.attackDirections.Length; i++)
        {

        }
         
        if (card.attackDirections[0])
        {
            int attackedX = card.currPosition.x;
            int attackedY = card.currPosition.y + 1;

            if (attackedY < fieldSize)
            {
                if (positionsField[attackedX, attackedY].currCard != null)
                    card.AttackCard(positionsField[attackedX, attackedY].currCard);
            }
        }

        if (card.attackDirections[1])
        {
            int attackedX = card.currPosition.x + 1;
            int attackedY = card.currPosition.y + 1;

            if (attackedX < fieldSize && attackedY < fieldSize)
            {
                if (positionsField[attackedX, attackedY].currCard != null)
                    card.AttackCard(positionsField[attackedX, attackedY].currCard);
            }
        }

        if (card.attackDirections[2])
        {
            int attackedX = card.currPosition.x + 1;
            int attackedY = card.currPosition.y;

            if (attackedX < fieldSize)
            {
                if (positionsField[attackedX, attackedY].currCard != null)
                    card.AttackCard(positionsField[attackedX, attackedY].currCard);
            }
        }

        if (card.attackDirections[3])
        {
            int attackedX = card.currPosition.x + 1;
            int attackedY = card.currPosition.y - 1;

            if (attackedX < fieldSize && attackedY >= 0)
            {
                if (positionsField[attackedX, attackedY].currCard != null)
                    card.AttackCard(positionsField[attackedX, attackedY].currCard);
            }
        }

        if (card.attackDirections[4])
        {
            int attackedX = card.currPosition.x;
            int attackedY = card.currPosition.y - 1;

            if (attackedY >= 0)
            {
                if (positionsField[attackedX, attackedY].currCard != null)
                    card.AttackCard(positionsField[attackedX, attackedY].currCard);
            }

        }

        if (card.attackDirections[5])
        {
            int attackedX = card.currPosition.x - 1;
            int attackedY = card.currPosition.y - 1;

            if (attackedX >= 0 && attackedY >= 0)
            {
                if (positionsField[attackedX, attackedY].currCard != null)
                    card.AttackCard(positionsField[attackedX, attackedY].currCard);
            }
        }

        if (card.attackDirections[6])
        {
            int attackedX = card.currPosition.x - 1;
            int attackedY = card.currPosition.y;

            if (attackedX >= 0)
            {
                if (positionsField[attackedX, attackedY].currCard != null)
                    card.AttackCard(positionsField[attackedX, attackedY].currCard);
            }

        }

        if (card.attackDirections[7])
        {
            int attackedX = card.currPosition.x - 1;
            int attackedY = card.currPosition.y + 1;

            if (attackedX >= 0 && attackedY < fieldSize)
            {
                if (positionsField[attackedX, attackedY].currCard != null)
                    card.AttackCard(positionsField[attackedX, attackedY].currCard);
            }
        }

        cardsController.CardToHands();
        CheckHP();
        CheckFinal();
    }
    
    void CreateField()
    {
        int counter = 0;
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                positionsField[j, i] = positions[counter];
                counter++;
            }
        }
    }

    void CheckHP()
    {
        enemyTotalHP = 0;
        playerTotalHP = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i].currCard != null)
            {
                if (positions[i].currCard.isEnemyCard)
                    enemyTotalHP += positions[i].currCard.currentBattleHP;
                else
                    playerTotalHP += positions[i].currCard.currentBattleHP;
            }
        }

        uIController.refreshHP(true, enemyTotalHP);
        uIController.refreshHP(false, playerTotalHP);
    }   
    
    void CheckFinal()
    {
        bool isFieldComplete = true;
        bool isHandsEmpty = true;
        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i].currCard == null)
            {
                isFieldComplete = false;
            }
        }

        for (int i = 0; i < cardsController.playerHandSlots.Count; i++)
        {
            if (cardsController.playerHandSlots[i].currCard != null)
            {
                isHandsEmpty = false;
            }
        }

        for (int i = 0; i < cardsController.enemyHandSlots.Count; i++)
        {
            if (cardsController.enemyHandSlots[i].currCard != null)
            {
                isHandsEmpty = false;
            }
        }

        if (isFieldComplete || isHandsEmpty)
        {
            uIController.ShowResults(playerTotalHP > enemyTotalHP);
        }
    } 
}
