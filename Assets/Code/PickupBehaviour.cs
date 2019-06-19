using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] int type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            switch (type)
            {
                default:   // healing
                    other.transform.GetComponent<PlayerBehaviour>().TakeDamage(-4);
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeScore(2);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
