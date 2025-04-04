using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effect/HealEffect")]
public class HealEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == null) return;
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.HealHealth(value);
                break;
            case EffectTargetType.Target:
                target.HealHealth(value);
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().HealHealth(value);
                }
                break;

        }
    }
}

