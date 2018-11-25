using System.Collections.Generic;
using UnityBulletML.Bullets;
using UnityEngine;

public class XmasSnowflakeBehaviour4 : AbstractBossBehaviour
{
    [SerializeField] private List<BulletEmitter> _bulletEmitters;

    private List<BulletEmitter> _currentBulletEmitters;

    public override void StartBehaviour()
    {
        base.StartBehaviour();

        // Save the previous boss bullet emitters
        _currentBulletEmitters = Boss.GetBulletEmitters();

        // Replace boss bullet emitters
        Boss.SetBulletEmitters(_bulletEmitters);
    }

    public override void StopBehaviour()
    {
        base.StopBehaviour();

        // Don't forget to reapply the boss previous bullet emitters
        Boss.SetBulletEmitters(_currentBulletEmitters);
    }
}
