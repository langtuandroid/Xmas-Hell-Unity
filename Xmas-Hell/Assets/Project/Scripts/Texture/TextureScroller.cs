using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public Vector2 ScrollSpeed;
    public Renderer Renderer;

    private Vector2 _savedOffset;

    void Start()
    {
        _savedOffset = Renderer.material.mainTextureOffset;
    }

    void Update()
    {
        Renderer.material.mainTextureOffset += ScrollSpeed * Time.deltaTime;
    }

    void OnDisable()
    {
        Renderer.material.mainTextureOffset = _savedOffset;
    }
}
