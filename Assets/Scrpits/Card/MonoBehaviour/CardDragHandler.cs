using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;
    public GameObject currentArrow;
    
    private Card currentCard;
    private bool canMove;
    private bool canExecute;
    
    private CharacterBase targetCharacter;
    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }

    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable) return;
        switch (currentCard.cardData.cardType)
        {
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                break;
            case CardType.Skill:
            case CardType.Abilities:
                canMove = true;
                break;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable) return;
        if (canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y,10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            
            currentCard.transform.position = worldPos;
            canExecute = worldPos.y > 1f;
        }
        else
        {
            if(eventData.pointerEnter== null)return;
            if (eventData.pointerEnter.CompareTag("Enemy"))
            {
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                if(targetCharacter.isDead)return;
                
                canExecute = true;
                ShowDamage(targetCharacter);
                
                return;
            }
            canExecute = false;
            targetCharacter = null;
        }
    }
    
    public void ShowDamage(CharacterBase enemy)
    {
        if (!currentCard.cardData.isAttackValue) return;
        Match match= Regex.Match(currentCard.cardData.cardDescription, @"<color=""white"">(\d+)</color>点伤害");
        if (match.Success)
        {
            int damage = Mathf.RoundToInt(Mathf.RoundToInt((int.Parse(match.Groups[1].Value)+currentCard.player.attackIncrement)*currentCard.player.baseAttack)*enemy.baseInjury);
            enemy.showAfterDamageHP.style.display=DisplayStyle.Flex;
            enemy.AfterDamageHP.text=enemy.CurrentHp+enemy.defense.currentValue-damage>0?(enemy.CurrentHp+Mathf.Min(0,enemy.defense.currentValue-damage)).ToString():"死";
        }
    }

    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!currentCard.isAvailable) return;
        if(currentArrow != null)
            Destroy(currentArrow);
        if (canExecute)
        {
            currentCard.ExecuteCardEffect(currentCard.player, targetCharacter);
        }
        else
        {
            currentCard.ResetCardTransform();
            currentCard.isAnimating =false;
        }
        
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().showAfterDamageHP.style.display=DisplayStyle.None;
        }

    }
}
