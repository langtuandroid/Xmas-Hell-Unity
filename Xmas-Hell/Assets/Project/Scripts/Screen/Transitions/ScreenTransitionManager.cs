using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransitionManager : MonoBehaviour
{
    #region Serialize field

    [SerializeField] private ScreenTransitionStore _screenTransitionStore = null;
    [SerializeField] private Canvas _canvas = null;
    [SerializeField] private int _canvasSortOrder = 10;

    #endregion

    private static ScreenTransitionManager _instance = null;
    private static ScreenTransition _currentTransition = null;
    private static Dictionary<string, ScreenTransition> _screenTransitionInstances = new Dictionary<string, ScreenTransition>();

    public static ScreenTransitionManager Instance => _instance;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_currentTransition != null)
        {
            HideTransition();
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            Initialize(_screenTransitionStore);
        }
    }

    public void Initialize(ScreenTransitionStore screenTransitionStore)
    {
        if (_instance)
            return;

        _instance = this;
        _canvas.sortingOrder = _canvasSortOrder;

        // Instantiate all screen transition with don't destroy on load
        foreach (var prefab in screenTransitionStore.ScreenTransitionPrefabs)
        {
            ScreenTransition screenTransitionInstance = Instantiate(prefab, _canvas.transform);
            screenTransitionInstance.gameObject.SetActive(false);
            _screenTransitionInstances.Add(screenTransitionInstance.Name, screenTransitionInstance);
        }

        if (_screenTransitionInstances.Count > 0)
        {
            _currentTransition = _screenTransitionInstances.First().Value;
        }
        else
        {
            Debug.LogWarning("No screen transition found!");
        }

        DontDestroyOnLoad(_instance);
    }

    public static void ShowTransition(string transitionName = null)
    {
        if (transitionName != null && _screenTransitionInstances.ContainsKey(transitionName))
        {
            // Make sure to disable the previous one
            _currentTransition.gameObject.SetActive(false);
            _currentTransition = _screenTransitionInstances[transitionName];
        }

        if (_currentTransition == null)
            return;

        _currentTransition.gameObject.SetActive(true);
        _currentTransition.Animator.SetTrigger("ShowTransition");
    }

    public static void HideTransition(bool instant = false)
    {
        if (_currentTransition == null)
            return;

        if (instant)
            _currentTransition.gameObject.SetActive(false);
        else
            _currentTransition.Animator.SetTrigger("HideTransition");
    }
}
