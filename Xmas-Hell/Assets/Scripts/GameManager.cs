using System;
using TMPro;
using UnityBulletML.Bullets;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Boss
    public BossStore BossStore;

    [HideInInspector]
    public AbstractBoss Boss;

    [HideInInspector]
    public Player Player;

    // Screen
    public ScreenManager GameScreenManager;

    // Game area
    public Canvas GameCanvas;
    public GameArea GameArea;

    // Timer
    public TextMeshProUGUI TimerText;
    private float _gameTimer;

    // Bullet engine
    public BulletManager BulletManager;

    // Pause
    private bool _pause;

    public bool Pause
    {
        get { return _pause; }
    }

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

        BulletManager.LoadPatterns();

        Reset();
    }

    public void Reset()
    {
        _gameTimer = 0f;
    }

    public void PauseGame()
    {
        _pause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _pause = false;
        Time.timeScale = 0.01f;
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
        // Back
        if (Input.GetKeyDown(KeyCode.Escape))
            GameScreenManager.GoBack();

        // Pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_pause)
                ResumeGame();
            else
                PauseGame();
        }

        // Debug
#if DEBUG
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            BulletManager.Clear();
        }
#endif
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
