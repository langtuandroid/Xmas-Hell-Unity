using UnityEngine;
using UnityEngine.UI;

public class BossBall : MonoBehaviour
{
    public EBoss BossType;
    public Animator Animator;
    public GameObject BossPanel;
    public Image Image;

    [Header("Relationship")]
    [SerializeField] private EBoss _leftBossRelationship = EBoss.Unknown;
    [SerializeField] private EBoss _rightBossRelationship = EBoss.Unknown;

    [Header("Database")]
    [SerializeField] private BossStore _bossStore;

    private EBossBallState _state = EBossBallState.Unknown;
    private MenuScreenManager _menuScreenManager;

    private void Awake()
    {
        _menuScreenManager = FindObjectOfType<MenuScreenManager>();
    }

    public void Start()
    {
        Animator.speed = Random.Range(0.5f, 1.5f);
        Animator.Play("Jiggle", 0, Random.Range(0f, 1f));

        CheckState();
    }

    private void CheckState()
    {
        var bossData = SaveSystem.GetBossData(BossType);
        var bossState = EBossBallState.Unknown;

        if (bossData.WinCounter > 0)
        {
            bossState = EBossBallState.Beaten;
        }
        else
        {
            if (_leftBossRelationship == EBoss.Unknown && _rightBossRelationship == EBoss.Unknown)
                bossState = EBossBallState.Available;
            else if (_bossStore.BossRelationships.ContainsKey(bossData.Type))
            {
                var bossRelationShip = _bossStore.BossRelationships[bossData.Type];
                var boss1Data = SaveSystem.GetBossData(bossRelationShip.Boss1);
                var boss2Data = SaveSystem.GetBossData(bossRelationShip.Boss2);

                if (boss1Data.WinCounter > 0 && boss2Data.WinCounter > 0)
                {
                    bossState = EBossBallState.Available;
                }
            }
        }

        SetState(bossState);
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
        if (_state == EBossBallState.Unknown)
            return;

        SessionData.SelectedBoss = BossType;
        _menuScreenManager.ShowBossPanel(BossType);
    }
}
