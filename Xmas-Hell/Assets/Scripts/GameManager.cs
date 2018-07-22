using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Boss
    public BossStore BossStore;

    [HideInInspector]
    public AbstractBoss Boss;

    // Game area
    public Canvas GameCanvas;
    public GameArea GameArea;

    // Timer
    public TextMeshProUGUI TimerText;

    // Timer
    private float _gameTimer;

    void Awake()
    {
        if (!Boss)
            Boss = FindObjectOfType<AbstractBoss>();

        if (!Boss)
        {
            Debug.Log("No boss found");
            return;
        }
    }

    private void Start()
    {
        _gameTimer = 0f;

        LoadBoss(SessionData.SelectedBoss);
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
        TimerText.text = TimeSpan.FromSeconds((double)_gameTimer).ToString(@"mm\:ss\.fff");
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
