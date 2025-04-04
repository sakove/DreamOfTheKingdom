using UnityEngine;
using UnityEngine.UIElements;

public class GameClearPanel : MonoBehaviour
{

    private Button backToMenuButton;
    public ObjectEventSO loadMenuEvent;

    private void OnEnable()
    {
        UIManager.Instance.HideAllPanel();
        GameObject playerObj=GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)playerObj.SetActive(false);
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToMenuButton").clicked += BackToMenu;
    }

    private void BackToMenu()
    {
        GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        loadMenuEvent.RaiseEvent(null, this);
        
    }
}
