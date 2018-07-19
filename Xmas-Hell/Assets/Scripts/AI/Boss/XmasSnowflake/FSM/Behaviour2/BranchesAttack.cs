using UnityEngine;

namespace XmasSnowflakeBehaviour2FSM
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
                dynamicBranch1.GetComponent<XmasSnowflakeBranch>().SetBoss(Boss);
                branch1.gameObject.SetActive(false);
            }

            _branch1Group.gameObject.SetActive(false);

            for (var i = 0; i < _branch2Group.childCount; i++)
            {
                var branch2 = _branch2Group.GetChild(i);
                var dynamicBranch2 = Instantiate(Branch2Prefab, branch2.position, branch2.rotation);
                dynamicBranch2.GetComponent<XmasSnowflakeBranch>().SetBoss(Boss);
                branch2.gameObject.SetActive(false);
            }

            _branch2Group.gameObject.SetActive(false);
        }

        private void ShowAllBranches()
        {
            _branch1Group.gameObject.SetActive(true);
            _branch2Group.gameObject.SetActive(true);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            ShowAllBranches();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }
    }
}