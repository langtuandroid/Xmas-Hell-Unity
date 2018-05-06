using System;
using UnityEngine;

[Serializable]
public class AbstractBossBehaviour : ScriptableObject
{
    [SerializeField]
    public float InitialBehaviourLife;

    protected AbstractBoss Boss;
    protected float CurrentBehaviourLife;
    protected bool BehaviourEnded;

    public AbstractBossBehaviour()
    {
    }

    public float GetCurrentBehaviourLife()
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

    public void Start()
    {
    }

    public void Stop()
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
