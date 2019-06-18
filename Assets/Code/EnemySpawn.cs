using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] float spawnRate, rateFluctuation, spawnDistance;
    [SerializeField] float[] betweenX;
    [SerializeField] GameObject[] enemies;

    float nextSpawn;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        SetTime();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nextSpawn <= 0)
        {
            GameObject newBorn = Instantiate(enemies[Random.Range(0, enemies.Length)], RandomSpawnPosition(), new Quaternion());

            SetTime();
        }
        else
            nextSpawn -= Time.deltaTime;
    }

    void SetTime()
    {
        nextSpawn = Random.Range(spawnRate * (1 - rateFluctuation), spawnRate * (1 + rateFluctuation));
    }

    public Vector3 RandomSpawnPosition()
    {
        if (rb.velocity.z > 0)
            return new Vector3(Random.Range(betweenX[0], betweenX[1]), transform.position.y, transform.position.z + spawnDistance);
        else if (rb.velocity.z < 0)
            return new Vector3(Random.Range(betweenX[0], betweenX[1]), transform.position.y, transform.position.z - spawnDistance);
        else
        {
            if (Random.Range(0, 2) == 1)
                return new Vector3(Random.Range(betweenX[0], betweenX[1]), transform.position.y, transform.position.z - spawnDistance);
            else
                return new Vector3(Random.Range(betweenX[0], betweenX[1]), transform.position.y, transform.position.z + spawnDistance);
        }
    }
}
