using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransitionManager : MonoBehaviour
{
    [SerializeField] private ScreenTransitionStore _screenTransitionStore = null;

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
            _instance = this;
            Initialize(_screenTransitionStore);
            DontDestroyOnLoad(_instance);
        }
    }

    public void Initialize(ScreenTransitionStore screenTransitionStore)
    {
        // Instantiate all screen transition with don't destroy on load
        foreach (var prefab in screenTransitionStore.ScreenTransitionPrefabs)
        {
            ScreenTransition screenTransitionInstance = Instantiate(prefab, transform);
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
