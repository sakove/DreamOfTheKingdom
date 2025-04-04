using UnityEngine;


[CreateAssetMenu(fileName = "VulnerableEffect", menuName = "Card Effect/VulnerableEffect")]
public class VulnerableEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetupVulnerable(value);
                break;
            case EffectTargetType.Target:
                target.SetupVulnerable(value);

                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().SetupVulnerable(value);
                }
                break;

        }
    }
}
