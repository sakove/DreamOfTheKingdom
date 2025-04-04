using UnityEngine;
using UnityEngine.AddressableAssets;


[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Map/RoomDataSO")] 

public class RoomDataSO:ScriptableObject
{
    //房间 地图上显示的图标，类型，房间内部的样子（资源）
    public Sprite roomIcon;

    public RoomType roomType;

    public AssetReference sceneToLoad;
}