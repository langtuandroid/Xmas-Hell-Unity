using System;
using UnityEngine;

[Serializable]
public class AbstractBossBehaviour : ScriptableObject
{
    [SerializeField]
    public float InitialBehaviourLife;

    protected AbstractBoss Boss;
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

    public void Initialize(AbstractBoss boss)
    {
        Boss = boss;
    }

    public virtual void Start()
    {
        Reset();
    }

    public virtual void Reset()
    {
        CurrentBehaviourLife = InitialBehaviourLife;
        BehaviourEnded = false;
        Stop();
    }

    public virtual void Stop()
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

    public virtual void Update()
    {
        CheckBehaviourIsEnded();
    }
}
