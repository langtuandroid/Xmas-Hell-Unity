using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Boss
    public BossStore BossStore;

    [HideInInspector]
    public AbstractBoss Boss;

    [HideInInspector]
    public Player Player;

    // Game area
    public Canvas GameCanvas;
    public GameArea GameArea;

    // Timer
    public TextMeshProUGUI TimerText;
    private float _gameTimer;

    public float GameTimer
    {
        get { return _gameTimer; }
    }

    // FSM
    private Animator _fsm;

    public void Initialize()
    {
        if (!Boss)
            Boss = FindObjectOfType<AbstractBoss>();

        if (!Boss)
        {
            Debug.Log("No boss found");
            return;
        }

        _fsm = GetComponent<Animator>();

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Player.OnPlayerDeath.AddListener(OnPlayerDeath);

        Reset();
    }

    public void Reset()
    {
        _gameTimer = 0f;
    }

    void OnBossDeath()
    {
        Debug.Log("End game: boss death");
        _fsm.SetTrigger("BossDeath");
    }

    void OnPlayerDeath()
    {
        Debug.Log("End game: player death");
        _fsm.SetTrigger("PlayerDeath");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ScreenManager.GoBack();
    }

    void FixedUpdate()
    {
        _gameTimer += Time.deltaTime;

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Timer
        TimerText.text = TimeSpan.FromSeconds(_gameTimer).ToString(@"mm\:ss\.fff");
    }

    public void LoadBoss(EBoss BossType)
    {
        // TODO: Should create a new instance of a boss according to the given type
        var bossPrefab = BossStore.GetBossPrefab(BossType);

        if (bossPrefab)
        {
            var bossObject = Instantiate(bossPrefab);
            Boss = bossObject.GetComponent<AbstractBoss>();
        }
    }
}
