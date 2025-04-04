using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   public Player player;
   
   public void OnNewGame()
   {
      player.NewLife();
   }

   public void OnBattleEnd()
   { 
      player.EndOfBattle();
   }
}
