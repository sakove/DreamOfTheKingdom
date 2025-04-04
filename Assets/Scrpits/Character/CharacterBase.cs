using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CharacterBase : MonoBehaviour
{

    public int maxHp;

    public IntVariable hp;
    
    public IntVariable defense;
    
    public bool isDead;

    public GameObject buff;
    public GameObject debuff;

    public IntVariable sharpnessRound;
    public IntVariable weaknessRound;
    public IntVariable powerValue;
    public IntVariable burnValue;
    public IntVariable vulnerableRound;


    protected VisualElement rootElement;
    
    private VisualElement sharpnessElement;
    private Label sharpnessElementLabel;
    private VisualElement weaknessElement;
    private Label weaknessElementLabel;
    private VisualElement burnElement;
    private Label burnElementLabel;
    protected VisualElement agilityElement;
    protected Label agilityElementLabel;
    private VisualElement vulnerableElement;
    private Label vulnerableElementLabel;
    private VisualElement powerElement;
    private Label powerElementLabel;

    protected VisualElement intentElement;
    protected Label intentAmountLabel;
    public VisualElement showAfterDamageHP;
    public Label  AfterDamageHP;

    public int attackIncrement ;
    public  int defenseIncrement ;
    
    public float baseAttack = 1f;
    public float baseInjury = 1f;
    private float vulnerableEffect = 0.5f;
    private float sharpnessEffect = 0.5f;
    private float weaknessEffect = -0.25f;
    
    public int CurrentHp{get=>hp.currentValue;set=>hp.SetValue(value);}
    public int MaxHp { get => hp.maxValue; }
    
    protected Animator animator;

    
    [Header("广播")] public ObjectEventSO characterDeadEvent;
    
    
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        attackIncrement = 0;
        defenseIncrement = 0;
        baseAttack = 1f;
        baseInjury = 1f;

    }

    protected virtual void Start()
    {
        hp.maxValue=maxHp;
        CurrentHp=MaxHp;
        sharpnessRound.currentValue=sharpnessRound.maxValue;
        weaknessRound.currentValue=weaknessRound.maxValue;
        powerValue.currentValue=powerValue.maxValue;
        burnValue.currentValue=burnValue.maxValue;
        vulnerableRound.currentValue=vulnerableRound.maxValue;
        ResetDefense();
        
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        
        intentElement = rootElement.Q<VisualElement>("Intent");
        intentAmountLabel=rootElement.Q<Label>("IntentAmount");
        intentElement.style.display = DisplayStyle.None;
        
        showAfterDamageHP=rootElement.Q<VisualElement>("ShowAfterDamageHP");
        AfterDamageHP=rootElement.Q<Label>("AfterDamageHP");
        showAfterDamageHP.style.display = DisplayStyle.None;
        
        ResetBuffAndDebuffUI();
        
    }
    

     public void SetupPowerValue(int value)
    {
        attackIncrement = powerValue.currentValue+value;
        powerValue.SetValue(powerValue.currentValue+value);
        if(value >0) buff.SetActive(true);
        else if(value < 0) debuff.SetActive(true);
    }
    public void SetupBurnValue(int value)
    {
        burnValue.SetValue(burnValue.currentValue+value);
        if(value > 0) debuff.SetActive(true);
        else if(value < 0) buff.SetActive(true);
    }
    public void SetupSharpness(int round)
    {
        baseAttack = Mathf.Min(baseAttack,1f)+sharpnessEffect;
        var newRound = round + sharpnessRound.currentValue;
        sharpnessRound.SetValue(newRound);
        buff.SetActive(true);
    }
    public void SetupWeakness(int round)
    {
        baseAttack = Mathf.Max(baseAttack,1f)+weaknessEffect;
        var newRound = round + weaknessRound.currentValue;
        weaknessRound.SetValue(newRound);
        debuff.SetActive(true);
    }
    public void SetupVulnerable(int round)
    {
        baseInjury = Mathf.Min(baseInjury,1f)+vulnerableEffect;
        var newRound = round + vulnerableRound.currentValue;
        vulnerableRound.SetValue(newRound);
        debuff.SetActive(true);
    }
    
    public void UpdateSharpnessRound()
    {
        if (sharpnessRound.currentValue-1==0)baseAttack = baseAttack-sharpnessEffect;
        sharpnessRound.SetValue(Mathf.Max(sharpnessRound.currentValue-1,0));
    }
    
    public void UpdateWeaknessRound()
    {
        if(weaknessRound.currentValue-1==0)baseAttack = baseAttack-weaknessEffect;
        weaknessRound.SetValue(Mathf.Max(weaknessRound.currentValue-1,0));
    }
    
    public void UpdateBurnValue()
    {
        //结算灼烧伤害
        TakeDamage(burnValue.currentValue);
        
        burnValue.SetValue(Mathf.Max(burnValue.currentValue-1,0));
    }
    
    public void UpdateVulnerableRound()
    { 
        if(vulnerableRound.currentValue-1==0)baseInjury =baseInjury-vulnerableEffect;
        vulnerableRound.SetValue(Mathf.Max(vulnerableRound.currentValue-1,0));
    }
    public  void TakeDamage(int damage)
    {
        int currentDamage=Mathf.RoundToInt(damage * baseInjury) - defense.currentValue;
        int currentDefense=defense.currentValue - Mathf.RoundToInt(damage * baseInjury);
        if(currentDamage > 0)
        { 
            currentDefense = 0;
          animator.SetTrigger("hit");
        }
        else
        {
            currentDamage = 0;
        }
        
        defense.SetValue(currentDefense);
        
        if (CurrentHp > currentDamage)
        {
            CurrentHp-=currentDamage;
        }
        else
        {
            CurrentHp=0;
            //当前人物死亡
            isDead = true;
            animator.SetBool("isDead",isDead);
            characterDeadEvent.RaiseEvent(this,this);
        }
        
    }

    public void UpdateDefense(int amount)
    {
        var value=defense.currentValue+amount;
        defense.SetValue(value);
        
    }

    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void HealHealth(int amount)
    {
        CurrentHp=Mathf.Min(CurrentHp+amount,maxHp);
        buff.SetActive(true);
    }

    public void ResetBuffAndDebuffUI()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        sharpnessElement = rootElement.Q<VisualElement>("Sharpness");
        sharpnessElementLabel = rootElement.Q<Label>("SharpnessRound");
        sharpnessElement.style.display = DisplayStyle.None;
        weaknessElement = rootElement.Q<VisualElement>("Weakness");
        weaknessElementLabel = rootElement.Q<Label>("WeaknessRound");
        weaknessElement.style.display = DisplayStyle.None;
        burnElement = rootElement.Q<VisualElement>("Burn");
        burnElementLabel = rootElement.Q<Label>("BurnValue");
        burnElement.style.display = DisplayStyle.None;
        vulnerableElement = rootElement.Q<VisualElement>("Vulnerable");
        vulnerableElementLabel = rootElement.Q<Label>("VulnerableRound");
        vulnerableElement.style.display = DisplayStyle.None;
        powerElement = rootElement.Q<VisualElement>("Power");
        powerElementLabel = rootElement.Q<Label>("PowerValue");
        powerElement.style.display = DisplayStyle.None;
        agilityElement = rootElement.Q<VisualElement>("Agility");
        agilityElementLabel = rootElement.Q<Label>("AgilityValue");
        agilityElement.style.display = DisplayStyle.None;
    }
    
    public void UpdateSharpnessUI()
    {
        if (sharpnessElement == null||sharpnessElementLabel==null)
        {
            sharpnessElement = rootElement.Q<VisualElement>("Sharpness");
            sharpnessElementLabel = rootElement.Q<Label>("SharpnessRound");
        }
        if (sharpnessRound.currentValue > 0)
        {
            sharpnessElement.style.display = DisplayStyle.Flex;
            sharpnessElementLabel.text = sharpnessRound.currentValue.ToString();
        }
        else
        {
            sharpnessElement.style.display = DisplayStyle.None;
        }
    }
    public void UpdateWeaknessUI()
    {
        if (weaknessElement== null||weaknessElementLabel==null)
        {
            weaknessElement = rootElement.Q<VisualElement>("Weakness");
            weaknessElementLabel = rootElement.Q<Label>("WeaknessRound");
        }
        if (weaknessRound.currentValue > 0)
        {
            weaknessElement.style.display = DisplayStyle.Flex;
            weaknessElementLabel.text = weaknessRound.currentValue.ToString();
        }
        else
        {
            weaknessElement.style.display = DisplayStyle.None;
        }
    }
    public void UpdatePowerUI()
    {
        if (powerElement == null||powerElementLabel==null)
        {
            powerElement = rootElement.Q<VisualElement>("Power");
            powerElementLabel = rootElement.Q<Label>("PowerValue");
        }
        if (powerValue.currentValue != 0)
        {
            powerElement.style.display = DisplayStyle.Flex;
            powerElementLabel.text = powerValue.currentValue.ToString();
        }
        else
        {
            powerElement.style.display = DisplayStyle.None;
        }
    }
    
    public void UpdateBurnUI()
    {
        if (burnElement == null||burnElementLabel==null)
        {
            burnElement = rootElement.Q<VisualElement>("Burn");
            burnElementLabel = rootElement.Q<Label>("BurnValue");
        }
        if (burnValue.currentValue != 0)
        {
            burnElement.style.display = DisplayStyle.Flex;
            burnElementLabel.text = burnValue.currentValue.ToString();
        }
        else
        {
            burnElement.style.display = DisplayStyle.None;
        }
    }
    public void UpdateVulnerableUI()
    {
        if (vulnerableElement == null||vulnerableElementLabel==null)
        {
            vulnerableElement = rootElement.Q<VisualElement>("Vulnerable");
            vulnerableElementLabel = rootElement.Q<Label>("VulnerableRound");
        }
        if (vulnerableRound.currentValue != 0)
        {
            vulnerableElement.style.display = DisplayStyle.Flex;
            vulnerableElementLabel.text = vulnerableRound.currentValue.ToString();
        }
        else
        {
            vulnerableElement.style.display = DisplayStyle.None;
        }
    }

    public void NewLife()
    {
        isDead = false;
        CurrentHp=MaxHp;
        EndOfBattle();
    }

    public void EndOfBattle()
    {
        attackIncrement = 0;
        defenseIncrement = 0;
        defense.currentValue=0; 
        sharpnessRound.currentValue=0; 
        weaknessRound.currentValue=0; 
        powerValue.currentValue=0; 
        burnValue.currentValue=0; 
        vulnerableRound.currentValue=0;
        baseAttack = 1f;
        baseInjury = 1f;
    }
    
}
