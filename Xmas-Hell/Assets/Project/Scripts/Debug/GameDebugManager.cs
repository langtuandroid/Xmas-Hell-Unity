using UnityBulletML.Bullets;
using UnityEngine;

public class GameDebugManager : MonoBehaviour
{
    [Header("Bullet rendering")]
    [SerializeField] private bool _renderBulletWithMainCamera = true;
    [SerializeField] private Camera _mainCamera = null;
    [SerializeField] private BulletRenderer _bulletRenderer = null;

    void Start()
    {
        if (Debug.isDebugBuild)
        {
            if (_renderBulletWithMainCamera)
            {
                _bulletRenderer.SetRenderingCamera(null);
                _mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("EnemyBullet");
            }
        }
    }
}
