using DG.Tweening;
using UnityEngine;

public class ExitButton : TapBehaviour
{
    public override void OnTap(Vector2 tapPosition)
    {
        transform.DOScale(0.9f, 0.15f).OnComplete(() => transform.DOScale(1.0f, 0.15f)).OnComplete(()=> Application.Quit());        
    }
}