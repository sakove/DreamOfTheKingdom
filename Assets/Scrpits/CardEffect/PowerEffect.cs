using UnityEngine;

[CreateAssetMenu(fileName = "PowerEffect", menuName = "Card Effect/PowerEffect")]
public class PowerEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == null) return;
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetupPowerValue(value);
                break;
            case EffectTargetType.Target:
                target.SetupPowerValue(value);
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().SetupPowerValue(value);
                }
                break;
        }
    }
}