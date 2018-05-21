﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Boss
    public BossStore BossStore;
    public AbstractBoss Boss;
    public GameObject LifeBar;
    public float LifeBarBlinkTime = 0.2f;

    // Timer
    public TextMeshProUGUI TimerText;

    // Boss life bar
    private RectTransform _lifeBarRectTransform;
    private float _lifeBarTotalWidth;
    private RawImage _lifeBarRawImage;
    private float _lifeBarBlinkTimer;
    private Color _lifeBarOriginalColor;
    private bool _bossTookDamage;

    // Timer
    private float _gameTimer;

    void Awake()
    {
        if (LifeBar)
        {
            var lifeBarRect = LifeBar.GetComponent<RectTransform>();

            if (lifeBarRect)
            {
                _lifeBarRectTransform = lifeBarRect;
                _lifeBarTotalWidth = _lifeBarRectTransform.rect.width;
            }

            var lifeBarImage = LifeBar.GetComponent<RawImage>();

            if (lifeBarImage)
            {
                _lifeBarRawImage = lifeBarImage;
                _lifeBarOriginalColor = lifeBarImage.color;
            }
        }

        if (!Boss)
            Boss = FindObjectOfType<AbstractBoss>();

        if (Boss)
            Boss.OnTakeDamage.AddListener(OnBossTakeDamage);
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

        if (_bossTookDamage)
        {
            if (_lifeBarBlinkTimer > 0)
            {
                _lifeBarBlinkTimer -= Time.deltaTime;
            }
            else
            {
                _lifeBarRawImage.color = _lifeBarOriginalColor;
                _bossTookDamage = false;
            }
        }
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

    private void OnBossTakeDamage()
    {
        if (Boss && LifeBar && _lifeBarRectTransform != null)
        {
            _lifeBarRectTransform.sizeDelta = new Vector2(_lifeBarTotalWidth * Boss.GetLifePercentage(), _lifeBarRectTransform.rect.height);

            _lifeBarRawImage.color = Color.white;

            if (!_bossTookDamage)
                _lifeBarBlinkTimer = LifeBarBlinkTime;

            _bossTookDamage = true;
        }
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
