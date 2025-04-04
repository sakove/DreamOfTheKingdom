
using System;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    
    public int column;
    public int line;

    private SpriteRenderer SpriteRenderer; 
    public RoomDataSO roomData;
    public RoomState roomState;

    public List<Vector2Int> linkTo = new();
    
    [Header("广播")]
    public ObjectEventSO loadRoomEvent;
    private void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    private void OnMouseDown()
    {
        //我this来广播roomData
        if (roomState == RoomState.Attainable)
        loadRoomEvent.RaiseEvent(this, this);
        
    }

    public void SetupRoom(int column,int line,RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;

        SpriteRenderer.sprite = roomData.roomIcon;

        SpriteRenderer.color = roomState switch
        {
            RoomState.Attainable => Color.white,
            RoomState.Locked => Color.gray,
            RoomState.Visited => Color.cyan,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
