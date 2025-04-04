using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    private Button backToMenuButton;
    public ObjectEventSO loadMenuEvent;

    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToMenuButton").clicked += BackToMenu;
    }

    private void BackToMenu()
    {
        GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        loadMenuEvent.RaiseEvent(null,this);
    }
}
