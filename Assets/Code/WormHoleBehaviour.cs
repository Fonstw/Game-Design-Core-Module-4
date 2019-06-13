using UnityEngine;
using UnityEngine.SceneManagement;

public class WormHoleBehaviour : MonoBehaviour
{
    public bool active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.tag == "Player")
        {
            PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        }
    }
}