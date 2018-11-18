using TMPro;
using UnityBulletML.Bullets;
using UnityEngine;

public class GameDebugOverlay : MonoBehaviour
{
    [Header("Manager references")]
    [SerializeField] private BulletManager _bulletManager;

    [Header("UI references")]
    [SerializeField] private TextMeshProUGUI _bulletCounter;
    [SerializeField] private TextMeshProUGUI _fpsCounter;

    // FPS counter
    private float _deltaTime = 0.0f;

    private void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        _bulletCounter.text = _bulletManager.Bullets.Count.ToString();

        // FPS counter
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float msec = _deltaTime * 1000.0f;
        float fps = 1.0f / _deltaTime;
        _fpsCounter.text = string.Format("{0:0.} FPS ({1:0.0}ms)", fps, msec);
    }
}
