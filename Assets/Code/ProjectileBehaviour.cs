using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 5, timeOut = 7, damage = 1;
    [SerializeField] string safe = "Player", target = "Enemy";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeOut -= Time.deltaTime;

        if (timeOut > 0)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
            print(other.name);

        if (other.tag == target)
        {
            if (other.GetComponentInParent<EnemyBehaviour>() != null)
                other.GetComponentInParent<EnemyBehaviour>().TakeDamage(damage);
            else if (other.name == "Player" && other.GetComponent<PlayerBehaviour>() != null)
                other.GetComponent<PlayerBehaviour>().TakeDamage(damage);
        }

        if (other.tag != safe && other.bounds.max.y > transform.position.y && !other.name.Contains("Structure"))
            Destroy(gameObject);
    }
}
