using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;

    public CardLayoutManager cardLayoutManager;
    
    public Vector3 deckPosition;
    
    private List<CardDataSO> drawDeck = new(); //抽牌堆

    private List<CardDataSO> discardDeck = new(); //弃牌堆

    private List<Card> handCardObjectList = new(); //手牌

    [Header("事件广播")] 
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;

    [Header("每回合抽牌数")]
    public int cardsDrawn;
    
    
    public void InitializeDeck()
    {
        drawDeck.Clear();
        foreach (var entry in cardManager.currentCardLibrary.cardLibraryList)
        {
            for (int i = 0; i < entry.Amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }

        //洗牌,更新抽牌堆和弃牌堆数字
        ShuffleDeck();
    }
    
    [ContextMenu("测试抽牌")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }

    
    public void NewTurnDrawCard()
    {
        DrawCard(cardsDrawn);
    }
    
    
    [ContextMenu("测试弃牌")]
    public void TestDiscardCard()
    {
        DiscardDeck(handCardObjectList[0]);
    }
    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                //洗牌
                foreach (var VARIABLE in discardDeck)
                {
                    drawDeck.Add(VARIABLE);
                }
                ShuffleDeck();
            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            
            //更新UI
            drawCountEvent.RaiseEvent(drawDeck.Count,this);
            
            var card=cardManager.GetCardGameObject().GetComponent<Card>();
            //初始化
            card.Init(currentCardData);
            card.UpdateDefenseValue();
            card.UpdateAttackValue();
            card.transform.position=deckPosition;
            
            handCardObjectList.Add(card);

            var delay = i * 0.2f;
            
            SetCardLayout(delay);
        }
    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard=handCardObjectList[i];
            
            CardTransform cardTransform=cardLayoutManager.GetCardTransform(i, handCardObjectList.Count);
            
            currentCard.UpdateCardState();
            
            currentCard.isAnimating = true;
            //抽卡动画
            currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete += () =>
            {
                currentCard.transform.DOLocalMove(cardTransform.pos, 0.5f).onComplete = () =>
                {
                    currentCard.isAnimating = false;
                };
                currentCard.transform.DORotateQuaternion(cardTransform.rot, 0.5f);
                
            };

            
            //设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rot);
        }
    }

    private void ShuffleDeck()
    {
        discardDeck.Clear();
        
        //更新UI显示数量
        drawCountEvent.RaiseEvent(drawDeck.Count,this);
        discardCountEvent.RaiseEvent(discardDeck.Count, this);

        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp=drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }
    
    //弃牌逻辑
    public void DiscardDeck(object obj)
    {
        Card card=obj as Card;
        
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);
        
        cardManager.DiscardCard(card.gameObject);
        
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
        SetCardLayout(0f);
    }

    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            discardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
        }
        handCardObjectList.Clear();
        discardCountEvent.RaiseEvent(discardDeck.Count, this);
    }

    public void ReleaseAllCards(object obj)
    {
        foreach (var card in handCardObjectList)
        {
            cardManager.DiscardCard(card.gameObject);
        }
        handCardObjectList.Clear();
        InitializeDeck();
    }

}
