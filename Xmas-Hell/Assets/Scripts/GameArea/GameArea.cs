using System;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ScreenCornerToGameObjectDictionary _gameAreaCorners;
    [SerializeField] private RectTransform _rectTransform;

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
        return _rectTransform.GetWorldRect(_canvas.gameObject.transform.localScale);
    }

    public Rect GetRect()
    {
        return _rectTransform.rect;
    }
}
