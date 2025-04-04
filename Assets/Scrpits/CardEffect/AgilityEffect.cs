using UnityEngine;


[CreateAssetMenu(fileName = "AgilityEffect", menuName = "Card Effect/AgilityEffect")]
public class AgilityEffect : CardEffectSO
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            var player=from as Player;
            if (player != null) player.SetupAgilityValue(value);
        }
    }
}
