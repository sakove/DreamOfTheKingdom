using UnityEngine;


[CreateAssetMenu(fileName = "WeaknessEffect", menuName = "Card Effect/WeaknessEffect")]
public class WeaknessEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == null) return;
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetupWeakness(value);
                break;
            case EffectTargetType.Target:
                target.SetupWeakness(value);
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().SetupWeakness(value);
                }
                break;

        }
    }
}
