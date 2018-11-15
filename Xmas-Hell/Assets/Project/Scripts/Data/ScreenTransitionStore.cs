using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScreenTransitionStore", menuName = "Screen/ScreenTransitionStore", order = 1)]
public class ScreenTransitionStore : ScriptableObject
{
    [SerializeField] private List<ScreenTransition> _screenTransitionPrefabs;

    public List<ScreenTransition> ScreenTransitionPrefabs => _screenTransitionPrefabs;
}
