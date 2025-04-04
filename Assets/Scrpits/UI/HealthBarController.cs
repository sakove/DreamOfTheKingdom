using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;

    private float percentage=0;
    
    [Header("Elements")]
    public Transform healthBarTransform;
    private UIDocument healthBarDocument;
    private ProgressBar healthBar;
    
    private VisualElement defenseElement;
    private Label defenseAmountLabel;
    
    

    private void OnEnable()
    {
        currentCharacter = GetComponent<CharacterBase>();
        
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPosition,Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition,size,Camera.main);
        element.transform.position = rect.center;
    }




    public void InitHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        healthBar=healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        healthBar.highValue = currentCharacter.maxHp;
        MoveToWorldPosition(healthBar, healthBarTransform.position,Vector2.zero);
        
        defenseElement=healthBar.Q<VisualElement>("Defense");
        defenseAmountLabel=healthBar.Q<Label>("DefenseAmount");
        defenseElement.style.display = DisplayStyle.None;
        
        healthBar.style.display = DisplayStyle.Flex;
        
        CharacterBase character=GetComponent<CharacterBase>();
        character.ResetDefense();
        character.ResetBuffAndDebuffUI();
        if (character is Player)
        {
            (character as Player).NoIntentAndAfterDamageHP();
        }
    }

    
    private void Update()
    {
        UpdataHealthBar();
        UpdataDefense();
    }
    
    
    public void UpdataHealthBar()
    {
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }
        
        
        if (healthBar != null)
        {
            
            healthBar.title=$"{currentCharacter.CurrentHp}/{currentCharacter.MaxHp}";
            
            healthBar.value=currentCharacter.CurrentHp;
            
            healthBar.highValue=currentCharacter.MaxHp;
            
            
            percentage = (float)currentCharacter.CurrentHp/(float)currentCharacter.MaxHp;
            if (percentage < 0.3f) 
            {
                healthBar.AddToClassList("lowHealth");
                
                healthBar.RemoveFromClassList("highHealth");
                healthBar.RemoveFromClassList("mediumHealth");
            }
            else if (percentage < 0.6f)
            {
                healthBar.AddToClassList("mediumHealth");
                
                healthBar.RemoveFromClassList("highHealth");
                healthBar.RemoveFromClassList("lowHealth");
            }
            else
            {
                healthBar.AddToClassList("highHealth");
                
                healthBar.RemoveFromClassList("mediumHealth");
                healthBar.RemoveFromClassList("lowHealth");
            }
        }
    }
    public void UpdataDefense()
    {
        //防御值更新
        defenseElement.style.display = currentCharacter.defense.currentValue>0?DisplayStyle.Flex:DisplayStyle.None;
        defenseAmountLabel.text=currentCharacter.defense.currentValue.ToString();
    }
}
