using UnityEngine;

namespace BossBehaviourState
{
    public class BranchesAttack : BossStateMachineBehaviour
    {
        public GameObject Branch1Prefab;
        public GameObject Branch2Prefab;

        private Transform _branch1Group;
        private Transform _branch2Group;

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
                snowflakeBranch.Initialize(Boss);
                snowflakeBranch.SetTarget(Boss.Player.transform);

                ((XmasSnowflakeBoss)Boss).AddBranch(snowflakeBranch);
            }

            _branch1Group.gameObject.SetActive(false);

            for (var i = 0; i < _branch2Group.childCount; i++)
            {
                var branch2 = _branch2Group.GetChild(i);
                var dynamicBranch2 = Instantiate(Branch2Prefab, branch2.position, branch2.rotation);

                var snowflakeBranch = dynamicBranch2.GetComponent<XmasSnowflakeBranch>();
                snowflakeBranch.Initialize(Boss);
                snowflakeBranch.SetTarget(Boss.Player.transform);

                ((XmasSnowflakeBoss)Boss).AddBranch(snowflakeBranch);
            }

            _branch2Group.gameObject.SetActive(false);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            ((XmasSnowflakeBoss)Boss).DestroyBranches();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            var branches = ((XmasSnowflakeBoss)Boss).Branches;

            for (int i = 0; i < branches.Count; i++)
            {
                var branch = branches[i];

                if (!branch.IsAlive)
                {
                    branches.Remove(branch);
                    Destroy(branch.gameObject);
                }
            }

            if (branches.Count == 0)
                animator.SetTrigger("RespawnBranches");
        }
    }
}