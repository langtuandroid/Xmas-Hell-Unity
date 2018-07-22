using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent OnPlayerDeath = new UnityEvent();

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player killed by " + collision.gameObject.tag);

        OnPlayerDeath.Invoke();
    }
}
