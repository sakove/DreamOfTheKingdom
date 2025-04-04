using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerater : MonoBehaviour
{
    [Header(header:"地图配置表")]
    public MapConfigSO mapConfig;

    [Header(header:"地图布局")]
    public MapLayoutSO mapLayout;
    
    [Header(header:"预制体")]
    public Room roomPrefab;
    public LineRenderer linePrefab;
    
    private float screenHeight;

    private float screenWidth;

    private float columnWidth;

    private Vector3 generatePoint;

    public float border;

    private  List<Room>rooms = new();
    private  List<LineRenderer>lines = new();
    
    public List<RoomDataSO> roomDataList = new();
    private Dictionary<RoomType,RoomDataSO> roomDataDict = new();
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);

        foreach (var roomDate in roomDataList)
        {
            roomDataDict.Add(roomDate.roomType, roomDate);
        }
    }
    

    private void OnEnable()
    {
        if(mapLayout.mapRoomDataList.Count>0) 
            LoadMap();
        else CreateMap();
    }


    public void CreateMap()
    {
        //创建前一列的房间列表
        List<Room> previousColumnRooms = new();
        
        //一列一列地创建
        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];

   
            //该Random最大值不包含
            var amount = UnityEngine.Random.Range(blueprint.min, blueprint.max+1);

            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);

            generatePoint = new Vector3(-screenWidth/2+border+columnWidth *column, startHeight, z: 0);

            var newPosition = generatePoint;

            var roomGapY = screenHeight / (amount + 1);
            
            //创建当前列的房间列表
            List<Room> currentColumnRooms = new();
            
            for (int i = 0; i < amount; i++)
            {
                
                //boss房
                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 1.5f;
                }

                /*else if (column != 0)
                {
                    //随机偏移
                    newPosition.x = generatePoint.x + Random.Range(-columnWidth / 5, columnWidth / 5);
                }*/
                
                newPosition.y = startHeight - roomGapY * i;
                var room = Instantiate(roomPrefab, newPosition,Quaternion.identity,transform );
    
                RoomType newRoomType=GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);

                if (column == 0)
                    room.roomState = RoomState.Attainable;
                else room.roomState = RoomState.Locked;
                
                room.SetupRoom(column, i,GetRoomData(newRoomType));
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }
            
            //判断是否为第一列，如果不是则连接到上一列
            if (previousColumnRooms.Count > 0)
            {
                //创建连线
                CreateConnections(previousColumnRooms, currentColumnRooms);
            }
            previousColumnRooms =currentColumnRooms;
        }
        
        SaveMap();
    }

    private void CreateConnections(List<Room> previousColumnRooms, List<Room> currentColumnRooms)
    {
        HashSet<Room> connectedRooms = new();

        //正向
        foreach (var room in previousColumnRooms)
        {
            var targetRoom=ConnectToRandomRoom(room,currentColumnRooms,true);
            connectedRooms.Add(targetRoom);
        }

        //反向
        foreach (var room in currentColumnRooms)
        {
            if (!connectedRooms.Contains(room))
            {
                ConnectToRandomRoom(room,previousColumnRooms ,false);
            }
        }
    }

    private Room ConnectToRandomRoom(Room room, List<Room> currentColumnRooms,bool check)
    {
        Room targetRoom;
        targetRoom = currentColumnRooms[UnityEngine.Random.Range(0, currentColumnRooms.Count)];
        
        //创建连线
        var line = Instantiate(linePrefab, transform);
        
        if (check)
        {
            room.linkTo.Add(new(targetRoom.column,targetRoom.line));
            line.SetPosition(0, room.transform.position);
            line.SetPosition(1, targetRoom.transform.position);
        }
        else
        {
            targetRoom.linkTo.Add(new(room.column,room.line));
            line.SetPosition(0, targetRoom.transform.position);
            line.SetPosition(1, room.transform.position);
        }
        lines.Add(line);
        
        return targetRoom;
    }


    [ContextMenu(itemName:"ReGenerateRoom")]

    public void ReGenerateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        rooms.Clear();
        lines.Clear();

        CreateMap();
    }

    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDict[roomType];
    }

    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options=flags.ToString().Split(',');
        string randomOption = options[UnityEngine.Random.Range(0, options.Length)];
        RoomType roomType=(RoomType)Enum.Parse(typeof(RoomType), randomOption);
        return roomType;
    }

    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();

        for (int i = 0; i < rooms.Count; i++)
        {
            var room = new MapRoomData();
            
            room.posx=rooms[i].transform.position.x;
            room.posy=rooms[i].transform.position.y;
            room.column=rooms[i].column;
            room.line=rooms[i].line;
            room.roomData=rooms[i].roomData;
            room.roomState=rooms[i].roomState;
            room.linkto = rooms[i].linkTo;

            
            mapLayout.mapRoomDataList.Add(room);
        }

        mapLayout.linePositionList = new();
        for (int i = 0; i < lines.Count; i++)
        {
            var line = new LinePosition(); 
            
            line.startPos = new SerializeVector3(lines[i].GetPosition((0)));
            line.endPos = new SerializeVector3(lines[i].GetPosition(1));

            mapLayout.linePositionList.Add(line);
        }
    }

    private void LoadMap()
    {
        //生成房间
        for (int i = 0; i < mapLayout.mapRoomDataList.Count; i++)
        {
            var newPos = new Vector3(mapLayout.mapRoomDataList[i].posx,mapLayout.mapRoomDataList[i].posy,0);
            var newRoom=Instantiate(roomPrefab,newPos,Quaternion.identity,transform);
            newRoom.roomState=mapLayout.mapRoomDataList[i].roomState;
            newRoom.SetupRoom(mapLayout.mapRoomDataList[i].column,mapLayout.mapRoomDataList[i].line,mapLayout.mapRoomDataList[i].roomData);
            newRoom.linkTo = mapLayout.mapRoomDataList[i].linkto;
            rooms.Add(newRoom);
        }
        
        //生成连线
        for (int i = 0; i < mapLayout.linePositionList.Count; i++)
        {
            var line =Instantiate(linePrefab,transform);
            line.SetPosition(0, mapLayout.linePositionList[i].startPos.ToVector3());
            line.SetPosition(1, mapLayout.linePositionList[i].endPos.ToVector3());
            
            lines.Add(line);
        }
    }
}



