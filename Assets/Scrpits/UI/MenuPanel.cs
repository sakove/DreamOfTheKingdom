using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button newGameButton,continueGameButton,quitGameButton;
    
    public ObjectEventSO newGameEvent, continueGameEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        newGameButton = rootElement.Q<Button>("NewGameButton");
        //continueGameButton = rootElement.Q<Button>("ContinueGameButton");
        quitGameButton = rootElement.Q<Button>("QuitGameButton");

        newGameButton.clicked += OnNewGameButtonClicked;
        // continueGameButton.clicked += OnContinueGameButtonClicked;
        quitGameButton.clicked += OnQuitGameButtonClicked;
    }

    private void OnQuitGameButtonClicked()
    {
        Application.Quit();
    }
    
    private void OnNewGameButtonClicked()
    {
        newGameEvent.RaiseEvent(null,this);
    }
    
    // private void OnContinueGameButtonClicked()
    // {
    //     
    // }
}
