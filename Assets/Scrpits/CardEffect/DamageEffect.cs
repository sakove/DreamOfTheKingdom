using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/DamageEffect")]
public class DamageEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == null) return;
        switch (targetType)
        {
            case EffectTargetType.Self:
                //
                break;
            case EffectTargetType.Target:
                int newValue = Mathf.RoundToInt((from.attackIncrement + value) * from.baseAttack);
                target.TakeDamage(newValue);
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;

        }
    }
}
