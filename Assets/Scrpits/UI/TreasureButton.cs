using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureButton : MonoBehaviour,IPointerDownHandler
{
    public ObjectEventSO gameWinEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.cardRarity =2;
        gameWinEvent.RaiseEvent(null,this);
    }
}
