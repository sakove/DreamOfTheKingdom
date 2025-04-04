using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{ 
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if(instance == null)
                instance=FindObjectOfType<UIManager>();
            if(instance==null)
                instance=new UIManager();
            return instance;
        }
    }
    [Header("面板")]
    public GameObject gamePlayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject gameClearPanel;
    public void  OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            Room currentRoom =data as Room;
            switch (currentRoom.roomData.roomType)
            {
                case RoomType.MinorEnemy:
                case RoomType.EliteEnemy:
                case RoomType.Boss:
                    gamePlayPanel.SetActive(true);
                    break;
                case RoomType.Shop:
                    break;
                case RoomType.Treasure:
                    break;
                case RoomType.RestRoom:
                    break;
            }

            
            
        }
    }
    
    //load map/menu....
    public void HideAllPanel()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pickCardPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void OnGameOverEvent()
    {
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
    public void OnPickCardEvent()
    {
        pickCardPanel.GetComponent<PickCardPanel>().cardRarity = GameManager.Instance.cardRarity;
        pickCardPanel.SetActive(true);
    }
    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
        
    }
    
    public void OnGameClearEvent()
    {
        gameClearPanel.SetActive(true);
    }
    
}
