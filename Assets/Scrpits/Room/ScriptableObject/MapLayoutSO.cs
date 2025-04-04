using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDataList = new();
    
    public List<LinePosition> linePositionList = new();


    //自己加的！！！！！
    private void OnEnable()
    {
        mapRoomDataList.Clear();
        linePositionList.Clear();
    }
}

[System.Serializable]
public class MapRoomData
{
    public float posx, posy;
    
    public int column, line;
    
    public RoomDataSO roomData;
    
    public RoomState roomState;

    public List<Vector2Int> linkto;

}

[System.Serializable]
public class LinePosition
{
    public SerializeVector3 startPos, endPos;
}