using System.Collections.Generic;
using UnityEngine;

namespace BossBehaviourState
{
    public class BranchesAttack : BossStateMachineBehaviour
    {
        public GameObject Branch1Prefab;
        public GameObject Branch2Prefab;

        private Transform _branch1Group;
        private Transform _branch2Group;
        private List<XmasSnowflakeBranch> _branches = new List<XmasSnowflakeBranch>(8);

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // TODO: Find a more reliable way to find this children
            _branch1Group = Boss.gameObject.transform.Find("AnimationRoot/body/branch1-group");
            _branch2Group = Boss.gameObject.transform.Find("AnimationRoot/body/branch2-group");

            ReplaceAllBranches();
        }

        private void ReplaceAllBranches()
        {
            for (var i = 0; i < _branch1Group.childCount; i++)
            {
                var branch1 = _branch1Group.GetChild(i);
                var dynamicBranch1 = Instantiate(Branch1Prefab, branch1.position, branch1.rotation);
                var snowflakeBranch = dynamicBranch1.GetComponent<XmasSnowflakeBranch>();
                snowflakeBranch.SetBoss(Boss);

                _branches.Add(snowflakeBranch);
            }

            _branch1Group.gameObject.SetActive(false);

            for (var i = 0; i < _branch2Group.childCount; i++)
            {
                var branch2 = _branch2Group.GetChild(i);
                var dynamicBranch2 = Instantiate(Branch2Prefab, branch2.position, branch2.rotation);
                var snowflakeBranch = dynamicBranch2.GetComponent<XmasSnowflakeBranch>();
                snowflakeBranch.SetBoss(Boss);

                _branches.Add(snowflakeBranch);
            }

            _branch2Group.gameObject.SetActive(false);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            for (int i = 0; i < _branches.Count; i++)
                Destroy(_branches[i].gameObject);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            for (int i = 0; i < _branches.Count; i++)
            {
                var branch = _branches[i];

                if (!branch.IsAlive)
                {
                    _branches.Remove(branch);
                    // TODO: Trigger pattern
                    Destroy(branch.gameObject);
                }
            }

            if (_branches.Count == 0)
                animator.SetTrigger("RespawnBranches");
        }
    }
}