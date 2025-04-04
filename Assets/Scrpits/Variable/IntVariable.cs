using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int maxValue;
    public int currentValue;
    
    public IntEventSO ValueChangedEvent;
    public IntEventSO AfterValueChangedEvent;

    
    [TextArea]
    [SerializeField]
    private string description;

    public void SetValue(int value)
    {
        currentValue = value;
        ValueChangedEvent?.RaiseEvent(value, this);
        AfterValueChangedEvent?.RaiseEvent(value, this);
    }

}
