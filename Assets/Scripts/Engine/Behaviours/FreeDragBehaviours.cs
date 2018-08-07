using UnityEngine;
using UnityEngine.EventSystems;

public class FreeDragBehaviours : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public EngineEvent onEndDrag, onBeginDrag, onDrag, onClick, onDown, onUp;

    public bool isComplete, isLocalMove;
    public bool holdX, holdY, holdTop, holdBottom, holdLeft, holdRight, holdAll;

    private Vector3 initialPosition;
    private Vector3 touchPosition;
    private Vector3 targetPosition;

    public virtual void Awake()
    {
        onEndDrag = new EngineEvent();
        onBeginDrag = new EngineEvent();
        onDrag = new EngineEvent();
        onClick = new EngineEvent();
        onDown = new EngineEvent();
        onUp = new EngineEvent();
        targetPosition = new Vector3();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isComplete)
        {
            if (onBeginDrag != null) onBeginDrag.Execute();
            touchPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            if (isLocalMove) initialPosition = transform.localPosition;
            else initialPosition = transform.position;
            AudioManager.Instance.PlaySound("MenuClick");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isComplete)
        {
            if (!holdAll)
            {
                if (holdX)
                {
                    targetPosition.Set(initialPosition.x, initialPosition.y + (Camera.main.ScreenToWorldPoint(eventData.position).y - touchPosition.y), initialPosition.z);
                }
                else if (holdY)
                {
                    targetPosition.Set(initialPosition.x + (Camera.main.ScreenToWorldPoint(eventData.position).x - touchPosition.x), initialPosition.y, initialPosition.z);
                }
                else
                {
                    targetPosition.Set(initialPosition.x + (Camera.main.ScreenToWorldPoint(eventData.position).x - touchPosition.x), initialPosition.y + (Camera.main.ScreenToWorldPoint(eventData.position).y - touchPosition.y), initialPosition.z);
                }

                if (holdTop)
                {
                    if (isLocalMove)
                    {
                        if (targetPosition.y < transform.localPosition.y) transform.localPosition = targetPosition;
                    }
                    else
                    {
                        if (targetPosition.y < transform.position.y) transform.position = targetPosition;
                    }
                }
                else if (holdBottom)
                {
                    if (isLocalMove)
                    {
                        if (targetPosition.y > transform.localPosition.y) transform.localPosition = targetPosition;
                    }
                    else
                    {
                        if (targetPosition.y > transform.position.y) transform.position = targetPosition;
                    }
                }
                else if (holdRight)
                {
                    if (isLocalMove)
                    {
                        if (targetPosition.x < transform.localPosition.x) transform.localPosition = targetPosition;
                    }
                    else
                    {
                        if (targetPosition.x < transform.position.x) transform.position = targetPosition;
                    }
                }
                else if (holdLeft)
                {
                    if (isLocalMove)
                    {
                        if (targetPosition.x > transform.localPosition.x) transform.localPosition = targetPosition;
                    }
                    else
                    {
                        if (targetPosition.x > transform.position.x) transform.position = targetPosition;
                    }
                }
                else if (!holdBottom && !holdTop)
                {
                    if (isLocalMove)
                    {
                        transform.localPosition = targetPosition;
                    }
                    else
                    {
                        transform.position = targetPosition;
                    }
                }
                if (onDrag != null) onDrag.Execute();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isComplete && onEndDrag != null) onEndDrag.Execute();
    }

    private Vector2 tapPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        tapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (onDown != null) onDown.Execute();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onClick != null)
        {
            if (Vector2.Distance(tapPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 0.05f)
            {
                onClick.Execute();
            }
        }
        if (onUp != null) onUp.Execute();
    }
}
