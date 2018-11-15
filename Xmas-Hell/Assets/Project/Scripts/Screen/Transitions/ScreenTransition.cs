using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Animator _animator;

    public string Name => _name;
    public Animator Animator => _animator;
}
