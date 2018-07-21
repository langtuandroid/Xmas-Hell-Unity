using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class StringToFloatDictionary : SerializableDictionary<string, float> { }

namespace BossBehaviourState
{
    public class MakeRandomChoice : BossStateMachineBehaviour
    {
        [FormerlySerializedAsAttribute("NextStateTrigger")]
        public StringToFloatDictionary NextStateTriggers;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            string nextState = "";

            // Make sure the sum of weight is equal to 1
            var sum = NextStateTriggers.Values.Sum();

            if (sum < 0.99f || sum > 1f)
                throw new Exception("Sum of all next state triggers weight must be equal to 1!");

            var sortedNextStateTriggers = NextStateTriggers.OrderBy(pair => pair.Value);

            var randomValue = UnityEngine.Random.value;
            var previousWeight = 0f;

            foreach (var item in sortedNextStateTriggers)
            {
                nextState = item.Key;
                var currentWeight = item.Value + previousWeight;

                if (currentWeight > randomValue)
                    break;

                previousWeight = currentWeight;
            }

            if (string.IsNullOrEmpty(nextState))
            {
                Debug.Log("State is null!");
            }

            Debug.Log("Next state: " + nextState);

            animator.SetTrigger(nextState);
        }
    }
}