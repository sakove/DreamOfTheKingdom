
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Enemy:CharacterBase
{
    public IntVariable cardRarity;
    
    public EnemyActionDataSO actionDataSO;
    
    public EnemyAction currentAction;

    protected Player player;

    private int intentValue = 0;

    private void OnEnable()
    {
        NewLife();
    }

    private void Update()
    {
        if (currentAction.effect is null) return;
        intentValue = currentAction.effect.value;
        if (currentAction.effect.GetType() == typeof(DamageEffect))
        {
            intentValue = (int)((intentValue + attackIncrement) * baseAttack * player.baseInjury);
        }
        intentAmountLabel.text = intentValue.ToString();
    }
    

    public virtual void OnPlayerTurnBegin()
    {
        if(player  == null) player = GameObject.FindWithTag("Player").GetComponent<Player>();
        
        if (currentAction.effect!=null&&currentAction.effect.GetType() != typeof(DamageEffect)) currentAction = actionDataSO.actions[0];
        else
        {
            var randomIndex = Random.Range(0, actionDataSO.actions.Count);
            currentAction = actionDataSO.actions[randomIndex];
        }
        //更新意图
        intentElement.style.display=DisplayStyle.Flex;
        intentElement.style.backgroundImage = new StyleBackground(currentAction.intentSprite);
    }
    public virtual void OnEnemyTurnEnd()
    {
        intentElement.style.display = DisplayStyle.None;
    }
    
    public virtual void OnEnemyTurnBegin()
    {
        
        if(currentAction.effect == null||isDead)return;
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.Self:
            case EffectTargetType.All:
                Skill();
                break;
        }
    }
    
    public virtual void Skill()
    {
        StartCoroutine(ProcessDelayAction("skill"));
    }

    public virtual void Attack()
    {
        StartCoroutine(ProcessDelayAction("attack"));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime%1.0f>0.5f
                                         && !animator.IsInTransition(0)
                                         &&animator.GetCurrentAnimatorStateInfo(0).IsTag(actionName));
        if(actionName=="attack")                                  
            currentAction.effect.Execute(this,player);
        else 
            currentAction.effect.Execute(this,this);
    }
}
