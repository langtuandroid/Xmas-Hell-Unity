using System;
using UnityEngine;

[Serializable]
public class XmasBallBehaviour1 : AbstractBossBehaviour
{
    public override void Update()
    {
        base.Update();

        Boss.transform.Translate(Vector3.down * Time.deltaTime);
    }
}
