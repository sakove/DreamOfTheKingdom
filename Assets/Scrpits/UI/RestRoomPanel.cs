using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restButton,studyButton,backToMapButton;

    public CardEffectSO restEffect;
    private GameObject playerObj;
    private Player player;
    
    [Header("事件广播")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;
    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        restButton = rootElement.Q<Button>("RestButton");
        studyButton = rootElement.Q<Button>("StudyButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");
        
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);
        
        restButton.clicked += OnRestButtonClicked;
        studyButton.clicked += OnStudyButtonClicked;
        backToMapButton.clicked += OnBackToMapButtonClicked;
    }

    private void OnStudyButtonClicked()
    {
        GameManager.Instance.cardRarity = 1;
        pickCardEvent.RaiseEvent(null,this);
        studyButton.SetEnabled(false);
        restButton.SetEnabled(false);
    }

    private void OnBackToMapButtonClicked()
    {
        playerObj=GameObject.FindWithTag("Player");
        playerObj.SetActive(false);
        loadMapEvent.RaiseEvent(null,this);
    }

    private void OnRestButtonClicked()
    {
        restEffect.Execute(player,player);
        studyButton.SetEnabled(false);
        restButton.SetEnabled(false);
    }
    
    
}
