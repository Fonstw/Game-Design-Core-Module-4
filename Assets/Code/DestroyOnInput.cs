using UnityEngine;

public class DestroyOnInput : MonoBehaviour
{
    public int levelNo = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("currentLevel", 0) >= levelNo)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("LeftHorizontal") != 0 || Input.GetAxis("LeftVertical") != 0 || Input.GetAxis("RightHorizontal") != 0 || Input.GetAxis("RightVertical") != 0 || Input.GetButton("Submit"))
            Destroy(gameObject);
    }
}
