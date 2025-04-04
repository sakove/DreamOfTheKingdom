using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect ", menuName = "Card Effect/DrawCardEffect ")]
public class DrawCardEffect  : CardEffectSO
{
    public IntEventSO drawCardEvent; 
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCardEvent?.RaiseEvent(value,this);
    }
}
