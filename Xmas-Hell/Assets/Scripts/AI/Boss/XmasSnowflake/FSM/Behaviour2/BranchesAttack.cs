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
                var dynamicBranch1 = Instantiate(Branch1Prefab, Boss.transform);
                dynamicBranch1.transform.localPosition = branch1.localPosition;
                dynamicBranch1.transform.localRotation = branch1.localRotation;
                dynamicBranch1.transform.localScale = branch1.localScale;
                branch1.gameObject.SetActive(false);
            }

            _branch1Group.gameObject.SetActive(false);

            for (var i = 0; i < _branch2Group.childCount; i++)
            {
                var branch2 = _branch2Group.GetChild(i);
                var dynamicBranch2 = Instantiate(Branch2Prefab, Boss.transform);
                dynamicBranch2.transform.localPosition = branch2.localPosition;
                dynamicBranch2.transform.localRotation = branch2.localRotation;
                dynamicBranch2.transform.localScale = branch2.localScale;
                branch2.gameObject.SetActive(false);
            }

            _branch2Group.gameObject.SetActive(false);
        }

        private void ShowAllBranches()
        {
            _branch1Group.gameObject.SetActive(true);
            _branch2Group.gameObject.SetActive(true);
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }
    }
}