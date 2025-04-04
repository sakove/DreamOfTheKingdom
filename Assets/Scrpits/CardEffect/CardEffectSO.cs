using UnityEngine;


public abstract class CardEffectSO : ScriptableObject
{
    public int value;

    public EffectTargetType targetType;

    public abstract void Execute(CharacterBase from, CharacterBase target);
    

}
