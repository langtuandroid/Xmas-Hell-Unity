using System;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ScreenCornerToGameObjectDictionary _gameAreaCorners;
    [SerializeField] private RectTransform _rectTransform;

    private Vector2 _initialGameCanvasLocalScale;

    public void Awake()
    {
        _initialGameCanvasLocalScale = _canvas.gameObject.transform.localScale;
    }

    public GameObject GetCorner(EScreenCorner corner)
    {
        if (!_gameAreaCorners.ContainsKey(corner))
            throw new Exception("No anchor found for this screen corner (" + corner.ToString() + ")");

        return _gameAreaCorners[corner];
    }

    public RectTransform GetRectTransform()
    {
        return _rectTransform;
    }

    public Rect GetWorldRect()
    {
        var cameraOrthographicSize = Camera.main.orthographicSize;
        var scale = (cameraOrthographicSize / (((RectTransform)_canvas.gameObject.transform).rect.height / 200f)) / 100f;
        var localGameCanvasScale = new Vector2(scale, scale);

        return _rectTransform.GetWorldRect(localGameCanvasScale);
    }

    public Rect GetRect()
    {
        return _rectTransform.rect;
    }

    public Vector2 NormalizedToWorldPoint(Vector2 position)
    {
        return GetWorldRect().min + (GetWorldRect().size * position);
    }
}
