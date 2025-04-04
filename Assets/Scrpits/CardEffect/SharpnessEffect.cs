using UnityEngine;


[CreateAssetMenu(fileName = "SharpnessEffect", menuName = "Card Effect/SharpnessEffect")]
public class SharpnessEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == null) return;
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetupSharpness(value);
                break;
            case EffectTargetType.Target:
                target.SetupSharpness(value);

                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().SetupSharpness(value);
                }
                break;

        }
    }
}
