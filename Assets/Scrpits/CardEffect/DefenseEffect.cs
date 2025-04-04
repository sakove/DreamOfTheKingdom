using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card Effect/DefenseEffect")]
public class DefenseEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == null) return;
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.UpdateDefense(value+from.defenseIncrement);
                break;
            case EffectTargetType.Target:
                target.UpdateDefense(value+from.defenseIncrement);
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().UpdateDefense(value+from.defenseIncrement);
                }
                break;

        }
    }
}
