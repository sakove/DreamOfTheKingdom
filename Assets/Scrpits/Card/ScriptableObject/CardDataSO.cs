using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public int rarity;
    public int cardID;
    public bool isAttackValue;
    public bool isDefenseValue;
    public string cardName;
    public Sprite cardImage;
    public int cost;
    public CardType cardType;
    [TextArea]
    public string cardDescription;
    
    //执行的实际效果
    public List <CardEffectSO> effects;
} 
