using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AbstractBoss Boss;
    public GameObject LifeBar;
    public float LifeBarBlinkTime = 0.2f;

    private RectTransform _lifeBarRectTransform;
    private float _lifeBarTotalWidth;
    private RawImage _lifeBarRawImage;
    private float _lifeBarBlinkTimer;
    private Color _lifeBarOriginalColor;
    private bool _bossTookDamage;

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

        if (Boss)
            Boss.OnTakeDamage.AddListener(OnBossTakeDamage);
    }

    private void Update()
    {
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
    }

}
