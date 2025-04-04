using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Player:CharacterBase
{
    public IntVariable playerMana;
    public IntVariable agilityValue;
    
    public int maxMana;
    
    public int CurrentMana{get=>playerMana.currentValue;set=>playerMana.SetValue(value);}

    public int MaxMana { get => playerMana.maxValue; }

    public void SetupAgilityValue(int value)
    {
        defenseIncrement = agilityValue.currentValue+value;
        agilityValue.SetValue(agilityValue.currentValue+value);
        if(value >0) buff.SetActive(true);
        else if(value < 0) debuff.SetActive(true);
    }

    public void NoIntentAndAfterDamageHP()
    {
        intentElement = rootElement.Q<VisualElement>("Intent");
        intentAmountLabel=rootElement.Q<Label>("IntentAmount");
        showAfterDamageHP=rootElement.Q<VisualElement>("ShowAfterDamageHP");
        AfterDamageHP=rootElement.Q<Label>("AfterDamageHP");
        
        intentElement.style.display = DisplayStyle.None;
        showAfterDamageHP.style.display = DisplayStyle.None;
    }
    public void UpdateAgilityUI()
    {
        if (agilityElement == null||agilityElementLabel==null)
        {
            agilityElement = rootElement.Q<VisualElement>("Agility");
            agilityElementLabel = rootElement.Q<Label>("AgilityValue");
        }
        if (agilityValue.currentValue != 0)
        {
            agilityElement.style.display = DisplayStyle.Flex;
            agilityElementLabel.text = agilityValue.currentValue.ToString();
        }
        else
        {
            agilityElement.style.display = DisplayStyle.None;
        }
    }
    protected override void Start()
    {
        base.Start();
        playerMana.maxValue =maxMana;
        CurrentMana=MaxMana;
        agilityValue.currentValue=agilityValue.maxValue;
    }
    

    public void NewTurn()
    {
        CurrentMana=MaxMana;
    }

    public void UpdateMana(int cost)
    {
        CurrentMana-=cost;
        if (CurrentMana <= 0) CurrentMana = 0;
    }

    public void OnNewGame()
    {
        NewLife();
    }

    public void OnBattleEnd()
    { 
        EndOfBattle();
    }
}
