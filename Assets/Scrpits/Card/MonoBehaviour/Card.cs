using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [Header(header:"组件")]
   public SpriteRenderer sprite;

   public TextMeshPro costText, descriptionText, typeText,nameText;
   
   public CardDataSO cardData;

   public Player player;
      
   public bool isAnimating=false;
   public bool isAvailable=false;
   
   
   [Header(header:"原始数据")]
   public Vector3 originalPosition;
   public Quaternion originalRotation;
   public int originalLayerOrder;

   
   [Header("广播事件")]
   public ObjectEventSO discardCardEvent;
   
   public IntEventSO costEvent;
   private void Start()
   {
      Init(cardData);
   }


   public void Init(CardDataSO card)
   {
      cardData = card;
      sprite.sprite = card.cardImage;
      costText.text = card.cost.ToString();

      descriptionText.text = card.cardDescription;
      
      nameText.text = card.cardName;
      typeText.text = card.cardType switch
      {
         CardType.Attack => "攻击牌",
         CardType.Skill => "技能牌",
         CardType.Abilities => "能力牌",
         _ => throw new System.NotImplementedException(),
      };
      
      player=GameObject.FindWithTag("Player").GetComponent<Player>();
   }
   
   public void UpdateAttackValue()
   {
      if (!cardData.isAttackValue) return;
      string newText = Regex.Replace(cardData.cardDescription, @"<color=""white"">(\d+)</color>点伤害", match =>
      {
         int value = int.Parse(match.Groups[1].Value);
         int newValue = Mathf.RoundToInt((value + player.attackIncrement) * player.baseAttack);
         return $"<color=\"white\">{newValue}</color>点伤害";
      });
      
      descriptionText.text = newText;
   }

   public void UpdateDefenseValue()
   {
      if (!cardData.isDefenseValue) return;
      string newText = Regex.Replace(cardData.cardDescription, @"<color=""white"">(\d+)</color>点护甲", match =>
      {
         int value = int.Parse(match.Groups[1].Value);
         int newValue = value + player.defenseIncrement;
         return $"<color=\"white\">{newValue}</color>点护甲";
      });
      descriptionText.text = newText;
   }
   public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
   {
      originalPosition = position;
      originalRotation = rotation;
      originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      if(isAnimating)return;
      transform.position = transform.rotation.z==0?originalPosition + Vector3.up*1f:originalPosition + Vector3.up*1.2f;

      transform.rotation = Quaternion.identity;
      GetComponent<SortingGroup>().sortingOrder = 50;
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      if(isAnimating)return;
      ResetCardTransform();
   }

   public void ResetCardTransform()
   {
      transform.position = originalPosition;
      transform.rotation = originalRotation;
      GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
   }

   public void ExecuteCardEffect(CharacterBase from, CharacterBase target)
   {
      //减少对应能量,回收卡牌
      costEvent.RaiseEvent(cardData.cost,this);
      discardCardEvent.RaiseEvent(this,this);
      foreach (var effect in cardData.effects)
      {
         effect.Execute(from, target);
      }
   }

   public void UpdateCardState()
   {
      isAvailable = cardData.cost <= player.CurrentMana;
      costText.color=isAvailable ? Color.cyan: Color.red;
   }
   
   
}
