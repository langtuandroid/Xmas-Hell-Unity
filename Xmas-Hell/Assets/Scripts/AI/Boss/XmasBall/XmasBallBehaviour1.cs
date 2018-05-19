using UnityEngine;

[CreateAssetMenu(fileName = "XmasBallBehaviour1", menuName = "AI/Behaviour/XmasBall/Behaviour1", order = 1)]
public class XmasBallBehaviour1 : AbstractBossBehaviour
{
    public override void Start()
    {
        base.Start();

        Boss.Speed = Boss.InitialSpeed * 2.5f;
        Boss.Animator.SetBool("Stunned", true);
    }

    public override void Update()
    {
        base.Update();

        Boss.transform.Translate(Vector3.down * Boss.Speed * Time.deltaTime);
    }
}
