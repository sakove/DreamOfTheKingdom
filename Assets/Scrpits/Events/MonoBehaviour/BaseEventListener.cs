using UnityEngine;
using UnityEngine.Events;



//泛型类匹配泛型Event事件
public class BaseEventListener<T> : MonoBehaviour
{

    public BaseEventSO<T> eventSO;
    public UnityEvent<T> Response;
    
    //注册事件
    private void OnEnable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised += OnEventRaised;
        }
    }

    //注销事件
    private void OnDisable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised -= OnEventRaised;
        }
    }

    private void OnEventRaised(T value)
    {

        Response?.Invoke(value);
    }

}