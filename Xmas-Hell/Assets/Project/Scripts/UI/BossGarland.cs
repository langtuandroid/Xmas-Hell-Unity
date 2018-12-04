using UnityEngine;
using UnityEngine.UI;

public class BossGarland : MonoBehaviour
{
    [SerializeField] private EBoss _relatedBoss;
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _image;

    private EBossBallState _state = EBossBallState.Unknown;

    public void Start()
    {
        CheckState();
    }

    private void CheckState()
    {
        var bossData = SaveSystem.GetBossData(_relatedBoss);
        var bossState = EBossBallState.Unknown;

        if (bossData.WinCounter > 0)
        {
            bossState = EBossBallState.Beaten;
        }

        SetState(bossState);
    }

    public void SetState(EBossBallState state)
    {
        _state = state;

        if (_state == EBossBallState.Beaten)
        {
            _image.color = Color.green;
        }
        else
        {
            _image.color = Color.red;
        }
    }
}
