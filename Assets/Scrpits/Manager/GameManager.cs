using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager:MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
                instance=FindObjectOfType<GameManager>();
            if(instance==null)
                instance=new GameManager();
            return instance;
        }
    }

    public int cardRarity;
    public List<Enemy> aiveEnemyList;
    
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    [Header("事件广播")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;
    public ObjectEventSO gameClearEvent;
    public void UpadtaMapLayoutData(object value)
    {
        var roomVector2 = (Vector2Int)value;
        if(mapLayout.mapRoomDataList.Count==0)return;
        var currentRoom=mapLayout.mapRoomDataList.Find(r=>r.column==roomVector2.x&&r.line==roomVector2.y);

        currentRoom.roomState = RoomState.Visited;
        
        // 更新相邻房间的roomState
        var sameColumnRooms=mapLayout.mapRoomDataList.FindAll(r=>r.column==roomVector2.x);
        foreach (var room in sameColumnRooms)
        {
            if(room.line!=roomVector2.y)
            room.roomState = RoomState.Locked;
        }

        foreach (var linkData in currentRoom.linkto)
        {
            var linkedRoom=mapLayout.mapRoomDataList.Find(r=>r.column==linkData.x&&r.line==linkData.y);
            linkedRoom.roomState = RoomState.Attainable;
        }

        aiveEnemyList.Clear();
    }

    public void OnRoomLoadedEvevnt()
    {
        cardRarity = 0;
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            aiveEnemyList.Add(enemy);
        }
    }
     
    public void OnCharacterDeadEvent(object character)
    {
        if (character is Player)
        {
            //失败
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        
        else if (character is Boss)
        { 
            aiveEnemyList.Remove(character as Boss);
            StartCoroutine(EventDelayAction(gameClearEvent));
        }
        else if (character is Enemy)
        {
            cardRarity=Mathf.Max(cardRarity,(character as Enemy).cardRarity.currentValue);
            aiveEnemyList.Remove(character as Enemy);
            if (aiveEnemyList.Count == 0)
            {
                //获胜
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }

            
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaiseEvent(null,this);
    }

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }
}
