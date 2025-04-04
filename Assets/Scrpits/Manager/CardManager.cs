
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager:MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList;

    [Header(header: "卡牌库")] 
    public CardLibrarySO newGameCardLibrary;
    public CardLibrarySO currentCardLibrary;

   
    

    private void Awake()
    {
        InitializeCardDataList();
        
        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentCardLibrary.cardLibraryList.Add(item);
        }
    }

    public void OnNewGameInitCardLibrary()
    {
        currentCardLibrary.cardLibraryList.Clear();
        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentCardLibrary.cardLibraryList.Add(item);
        }
    }
    private void OnDisable()
    {
        currentCardLibrary.cardLibraryList.Clear();
    }
    

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData",null).Completed += OnCardDataLoaded;
    }
    
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList=new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("Mo CardData Found!");
        }
    }

    
    //获取卡牌物体
    public GameObject GetCardGameObject()
    {
        
        var cardObj= poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }

    public void DiscardCard(GameObject cardGameObject)
    {
        poolTool.ReturnObjectToPool(cardGameObject);
        
    }


    public List<CardDataSO> RandomlyGetNewCardDataList(int rarity,int number)
    {
        if (number <= 0||rarity<0||rarity>2) return null;
        List<CardDataSO>  readyCardList = new List<CardDataSO>();
        List<CardDataSO>  trueCardList = new List<CardDataSO>();
        List<int> numbers = new List<int>();
        int index;
        readyCardList.Clear();
        trueCardList.Clear();
        numbers.Clear();
        for (int i = 0; i < cardDataList.Count; i++)
        {
            if (cardDataList[i].rarity == rarity)
            {
                readyCardList.Add(cardDataList[i]);
                numbers.Add(readyCardList.Count-1);
            }
        }
        
        for (int i = 0; i < number; i++)
        {
            index =Random.Range(0, numbers.Count);
            trueCardList.Add(readyCardList[numbers[index]]);
            numbers.RemoveAt(index);
        }
        return trueCardList;
    }

    public void UnlockCard(CardDataSO cardData)
    {
        var newCard = new CardLibraryEntry();
        newCard.cardData = cardData;
        newCard.Amount = 0;
        foreach (var item in currentCardLibrary.cardLibraryList)
        {
            if (item.cardData == cardData)
            {
                int amount = item.Amount;
                currentCardLibrary.cardLibraryList.Remove(item);
                newCard.Amount = 1+amount;
                break;
            }
        }
        if(newCard.Amount<=0)newCard.Amount = 1;
        currentCardLibrary.cardLibraryList.Add(newCard);
    }
}
