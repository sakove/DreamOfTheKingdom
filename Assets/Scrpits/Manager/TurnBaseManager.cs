using System;
using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{ 
    public GameObject player;
    
    private bool isPlayerTurn = false;
    
    private bool isEnemyTurn=false;
    
    public bool battleEnd = true;

    private float timeCounter;

    public float enemyTurnDuration;
    public float playerTurnDuration;

    [Header("事件广播")]
    public ObjectEventSO playerTurnBegin;

    public ObjectEventSO beforePlayerTurnBegin;
    
    public ObjectEventSO enemyTurnBegin;
    
    public ObjectEventSO enemyTurnEnd;
    private void Update()
    {
        if (battleEnd) return;
        if (isEnemyTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enemyTurnDuration)
            {
                timeCounter = 0;
                //敌人回合结束
                EnemyTurnEnd();
                //玩家回合开始
                isPlayerTurn = true;
            }
        }
        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0;
                //玩家回合开始
                
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }
    
    public void GameStart()
    {
        isPlayerTurn = true;
        isEnemyTurn=false;
        battleEnd = false;
        timeCounter = 0;

    }

    public void PlayerTurnBegin()
    {
        beforePlayerTurnBegin.RaiseEvent(null,this);
        playerTurnBegin.RaiseEvent(null,this);
    }

    public void EnemyTurnBegin()
    {
        isEnemyTurn=true;
        enemyTurnBegin.RaiseEvent(null,this);
    }

    public void EnemyTurnEnd()
    {
        isEnemyTurn=false;
        enemyTurnEnd.RaiseEvent(null,this);
    }

    public void OnRoomLoadEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                player.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
                player.SetActive(false);
                break;
            case RoomType.Treasure:
                player.SetActive(false);
                break;
            case RoomType.RestRoom:
                player.SetActive(true);
                player.GetComponent<PlayerAnimation>().SetSleepAnimation();
                break;
        }
    }

    public void OnBattleEndEvent()
    {
        battleEnd = true;
        player.SetActive(false);
    }
}
