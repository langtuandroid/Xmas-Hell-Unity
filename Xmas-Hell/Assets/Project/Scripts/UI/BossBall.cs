using UnityEngine;
using UnityEngine.UI;

public class BossBall : MonoBehaviour
{
    public EBoss BossType;
    public Animator Animator;
    public GameObject BossPanel;
    public Image Image;

    [Header("Database")]
    [SerializeField] private BossStore _bossStore;

    private EBossBallState _state = EBossBallState.Unknown;

    public void Start()
    {
        Animator.speed = Random.Range(0.5f, 1.5f);
        Animator.Play("Jiggle", 0, Random.Range(0f, 1f));
    }

    public void SetState(EBossBallState state)
    {
        _state = state;
        Image.sprite = _bossStore.GetBossBallSprite(BossType, _state);
    }

    public void SetBossType(EBoss bossType)
    {
        BossType = bossType;
        Image.sprite = _bossStore.GetBossBallSprite(bossType, _state);
    }

    public void OnClick()
    {
        SessionData.SelectedBoss = BossType;
        BossPanel.SetActive(true);
    }
}
