using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    [SerializeField] bool resetGame = false;

    // Start is called before the first frame update
    void Start()
    {
        if (resetGame) PlayerPrefs.SetInt("CurrentLevel", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
}
