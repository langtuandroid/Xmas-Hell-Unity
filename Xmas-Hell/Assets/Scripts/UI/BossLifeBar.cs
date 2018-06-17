using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BossLifeBar : MonoBehaviour
{
    public float LifeBarBlinkTime = 0.2f;
    public Color Color;

    private AbstractBoss _boss;
    private float _lifeBarTotalWidth;
    private RawImage _lifeBarRawImage;
    private float _lifeBarBlinkTimer;
    private Color _lifeBarOriginalColor;
    private bool _bossTookDamage;

    public void Initialize(AbstractBoss boss)
    {
        _boss = boss;

        var lifeBarImage = GetComponentInChildren<RawImage>();

        if (lifeBarImage)
        {
            _lifeBarRawImage = lifeBarImage;
            _lifeBarTotalWidth = lifeBarImage.rectTransform.rect.width;
            _lifeBarRawImage.color = Color;
            _lifeBarOriginalColor = Color;
        }

        _boss.OnTakeDamage.AddListener(OnBossTakeDamage);
    }

    public void Update()
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
        if (_boss && _lifeBarRawImage != null)
        {
            _lifeBarRawImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _lifeBarTotalWidth * _boss.GetLifePercentage());
            _lifeBarRawImage.color = Color.white;

            if (!_bossTookDamage)
                _lifeBarBlinkTimer = LifeBarBlinkTime;

            _bossTookDamage = true;
        }
    }
}
