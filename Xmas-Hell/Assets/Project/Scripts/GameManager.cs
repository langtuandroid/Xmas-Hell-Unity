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

    public Player Player;

    // Screen
    public ScreenManager GameScreenManager;

    // UI
    [SerializeField] private GamePanel _gamePanel;

    // Camera
    public CameraManager CameraManager;

    // Game area
    public Canvas GameCanvas;
    public GameArea GameArea;

    // Timer
    public TextMeshProUGUI TimerText;
    private float _gameTimer;

    // Bullet engine
    public BulletManager BulletManager;
    public BulletPhysics BulletPhysics;

    // Pause
    private bool _pause;

    private bool _gameIsFinished;
    private bool _gameIsWon;

    public bool Pause
    {
        get { return _pause; }
    }

    public float GameTimer
    {
        get { return _gameTimer; }
    }

    public bool GameIsFinished => _gameIsFinished;

    // FSM
    private Animator _fsm;

    public void Initialize()
    {
        if (!Boss)
            Boss = FindObjectOfType<AbstractBoss>();

        if (!Boss)
        {
            Debug.Log("No boss found");
        }
        else
        {
            Boss.OnDeath.AddListener(OnBossDeath);
        }

        _fsm = GetComponent<Animator>();

        Player.OnPlayerDeath.AddListener(OnPlayerDeath);

        BulletManager.LoadPatterns();
        BulletPhysics.OnCollision.AddListener(OnBulletCollision);

        CameraManager.OnCameraZoomFinished.AddListener(OnCameraZoomFinished);

        Reset();
    }

    public void Reset()
    {
        _gamePanel.gameObject.SetActive(false);
        _gameTimer = 0f;
        _gameIsFinished = false;

        Player.Initialize();
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

    #region Game states

    public void OnBossDeath()
    {
        if (_gameIsFinished)
            return;

        EndGame(true);
        _fsm.SetTrigger("BossDeath");
    }

    public void OnPlayerDeath()
    {
        if (_gameIsFinished)
            return;

        EndGame(false);
        _fsm.SetTrigger("PlayerDeath");
    }

    #endregion

    public void PlayerDeath()
    {
        Player.Kill();
        Boss.Pause();
        BulletManager.Pause();
    }

    public void BossDeath()
    {
        Boss.Kill();
        BulletManager.Clear();
    }

    public void OnPlayerExplosion()
    {
        Player.Destroy();
        BulletManager.Resume();

        if (!_gameIsWon)
            Boss.Resume();
    }

    public void ShowEndGamePanel()
    {
        _gamePanel.gameObject.SetActive(true);
        _gamePanel.Initialize(_gameIsWon);
    }

    void OnBulletCollision(Bullet bullet)
    {
        Debug.Log("Player hit by a bullet!");
        OnPlayerDeath();
    }

    #region Camera

    public void CameraZoomOnPlayer(float time)
    {
        CameraManager.ZoomTo(3f, Player.transform.position, time);
    }

    public void CameraZoomOut(float time)
    {
        CameraManager.ZoomTo(9.6f, Vector2.zero, time);
    }

    public void OnCameraZoomFinished()
    {
        _fsm.SetTrigger("CameraZoomFinished");
    }

    #endregion

    private void EndGame(bool win)
    {
        _gameIsFinished = true;
        _gameIsWon = win;
    }

    private void Update()
    {
        // Back
        if (Input.GetKeyDown(KeyCode.Escape))
            GameScreenManager.GoBack();

#if DEBUG
        // Pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_pause)
                ResumeGame();
            else
                PauseGame();
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            BulletManager.Clear();
        }

        // Camera shake
        if (Input.GetKeyUp(KeyCode.S))
        {
            CameraManager.Shake(0.5f, 0.5f);
        }
#endif
    }

    void FixedUpdate()
    {
        if (!_gameIsFinished)
        {
            _gameTimer += Time.deltaTime;
        }

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
