using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public int numberOfCards;
    public int cardRarity;
    
    public CardManager cardManager;
    
    public VisualTreeAsset cardTemplate;
    
    private VisualElement rootElement;
    
    private VisualElement cardContainer;
    
    private CardDataSO currentCardData;
    
    private Button confirmButton;
    
    private List<Button> cardButtons=new List<Button>();
    private List<CardDataSO>  cardDataList = new List<CardDataSO>();
    [Header("事件广播")]
    public ObjectEventSO finishPickCardEvent;

    private void OnEnable()
    {
        cardDataList.Clear();
        cardButtons.Clear();
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer= rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");
        confirmButton.style.display = DisplayStyle.None;
        
        cardDataList=cardManager.RandomlyGetNewCardDataList(cardRarity, numberOfCards);
        
        for (int i = 0; i < numberOfCards; i++)
        {
            var card = cardTemplate.Instantiate();
            InitCard(card, cardDataList[i]);
            cardContainer.Add(card);
            var button = card.Q<Button>("Card");
            button.clicked += ()=>OnCardClicked(button, cardDataList[cardButtons.IndexOf(button)]);
            cardButtons.Add(button);
        }
        confirmButton.clicked += OnConfirmButtonClicked;
    }

    private void OnConfirmButtonClicked()
    {
        cardManager.UnlockCard(currentCardData);
        finishPickCardEvent.RaiseEvent(null,this);
    }

    public void OnCardClicked(Button button,CardDataSO data)
    {
        currentCardData=data;
        
        confirmButton.style.display = DisplayStyle.Flex;

        for (int i = 0; i < numberOfCards; i++)
        {
            if(cardButtons[i]==button)
                cardButtons[i].SetEnabled(false);
            else cardButtons[i].SetEnabled(true);
        }
    }


    public void InitCard(VisualElement card, CardDataSO cardData)
    {
        var cardSpriteElement = card.Q<VisualElement>("Sprite");
        var cardCostElement = card.Q<Label>("SlotAmount");
        var cardDescriptionElement = card.Q<Label>("DescriptionLabel");
        var cardTypeElement = card.Q<Label>("TypeLabel");
        var cardNameElement = card.Q<Label>("NameLabel");
        
        cardSpriteElement.style.backgroundImage=new StyleBackground(cardData.cardImage);
        cardNameElement.text = cardData.name;
        cardCostElement.text = cardData.cost.ToString();
        cardDescriptionElement.text = cardData.cardDescription;
        cardTypeElement.text=cardData.cardType switch
        {
            CardType.Attack => "攻击牌",
            CardType.Skill => "技能牌",
            CardType.Abilities => "能力牌",
            _ => throw new System.NotImplementedException(),
        };
    }
    
}
