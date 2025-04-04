using UnityEngine;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Card Effect/BurnEffect")]
public class BurnEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == null) return;
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetupBurnValue(value);
                break;
            case EffectTargetType.Target:
                target.SetupBurnValue(value);
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().SetupBurnValue(value);
                }
                break;
        }
    }
}