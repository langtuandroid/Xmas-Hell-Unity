using System;
using UnityEngine;

[Serializable]
public class AbstractBossBehaviour : MonoBehaviour
{
    [SerializeField] public RuntimeAnimatorController FSM;
    [SerializeField] public float InitialBehaviourLife;

    public AbstractBoss Boss;
    protected float CurrentBehaviourLife;
    protected bool BehaviourEnded = false;

    public AbstractBossBehaviour()
    {
    }

    public float GetLife()
    {
        return CurrentBehaviourLife;
    }

    public float GetLifePercentage()
    {
        return CurrentBehaviourLife / InitialBehaviourLife;
    }

    public virtual void Initialize(AbstractBoss boss)
    {
        Boss = boss;
        CurrentBehaviourLife = InitialBehaviourLife;
        BehaviourEnded = false;
    }

    public virtual void StartBehaviour()
    {
        Reset();
        Boss.Animator.runtimeAnimatorController = FSM;
    }

    public virtual void Reset()
    {
        CurrentBehaviourLife = InitialBehaviourLife;
        BehaviourEnded = false;
        StopBehaviour();
    }

    public virtual void StopBehaviour()
    {
    }

    public bool IsBehaviourEnded()
    {
        return BehaviourEnded;
    }

    public virtual void TakeDamage(float amount)
    {
        CurrentBehaviourLife -= amount;
    }

    protected virtual void CheckBehaviourIsEnded()
    {
        if (CurrentBehaviourLife < 0)
            BehaviourEnded = true;
    }

    public virtual void Step()
    {
        CheckBehaviourIsEnded();
    }
}
