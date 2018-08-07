using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TapBehaviour : MonoBehaviour, IPointerClickHandler
{
    public abstract void OnTap(Vector2 tapPosition);

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTap(eventData.position);
    }
}
