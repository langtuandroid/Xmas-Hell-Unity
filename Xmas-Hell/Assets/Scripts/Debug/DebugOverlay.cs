using TMPro;
using UnityBulletML.Bullets;
using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    [Header("Manager references")]
    [SerializeField] private BulletManager _bulletManager;

    [Header("UI references")]
    [SerializeField] private TextMeshProUGUI _bulletCounter;

    void Update()
    {
        _bulletCounter.text = _bulletManager.Bullets.Count.ToString();
    }
}
