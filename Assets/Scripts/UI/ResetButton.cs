using DG.Tweening;
using UnityEngine;

public class ResetButton : TapBehaviour
{
    public GameController controller;
    public UIController uIController;

    Collider2D btnCollider;

    void Start()
    {
        btnCollider = GetComponent<Collider2D>();
    }

    public override void OnTap(Vector2 tapPosition)
    {
        btnCollider.enabled = false;
        uIController.HideResults();
        transform.DOScale(0.9f, 0.15f).OnComplete(() =>
        {
            transform.DOScale(1.0f, 0.15f);
            controller.ResetGame();
        });        
    }
}
