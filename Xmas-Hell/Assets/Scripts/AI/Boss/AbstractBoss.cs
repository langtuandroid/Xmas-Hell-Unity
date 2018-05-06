using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBoss : MonoBehaviour {

    [SerializeField]
    List<AbstractBossBehaviour> Behaviours;

    protected int CurrentBehaviourIndex;
    protected int PreviousBehaviourIndex;

    public AbstractBoss()
    {
    }

    private void Start()
    {
        PreviousBehaviourIndex = -1;
        CurrentBehaviourIndex = 0;

        if (Behaviours.Count > 0)
        {
            // Make the each behaviour have access to this class
            foreach (var behaviour in Behaviours)
                behaviour.Initialize(this);
        }
    }

    void Update()
    {
    }

    private void UpdateBehaviour()
    {
        UpdateBehaviourIndex();

        if (CurrentBehaviourIndex != PreviousBehaviourIndex)
        {
            if (PreviousBehaviourIndex >= 0)
                Behaviours[PreviousBehaviourIndex].Stop();

            // TODO: Trigger signal to clear all bullets
            // TODO: Make sure we restore the initial boss state for transition

            if (Behaviours.Count > 0)
                Behaviours[CurrentBehaviourIndex].Start();
        }

        if (Behaviours.Count > 0)
            Behaviours[CurrentBehaviourIndex].Update();

        PreviousBehaviourIndex = CurrentBehaviourIndex;
    }

    protected void UpdateBehaviourIndex()
    {
        if (Behaviours.Count == 0)
            return;

        if (Behaviours[CurrentBehaviourIndex].IsBehaviourEnded())
        {
            CurrentBehaviourIndex++;

            if (CurrentBehaviourIndex >= Behaviours.Count)
                Destroy(this);
        }
    }
}
