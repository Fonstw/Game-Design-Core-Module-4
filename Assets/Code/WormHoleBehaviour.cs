using UnityEngine;
using UnityEngine.SceneManagement;

public class WormHoleBehaviour : MonoBehaviour
{
    public bool active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.tag == "Player")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}