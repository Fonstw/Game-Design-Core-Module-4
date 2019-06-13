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
        nextSpawn -= Time.deltaTime;

        if (nextSpawn <= 0)
        {
            Vector3 spawnOffset;

            if (rb.velocity.z > 0)
                spawnOffset = new Vector3(0, 0, spawnDistance);
            else if (rb.velocity.z < 0)
                spawnOffset = new Vector3(0, 0, -spawnDistance);
            else
            {
                if (Random.Range(0, 2) == 1)
                    spawnOffset = new Vector3(0, 0, -spawnDistance);
                else
                    spawnOffset = new Vector3(0, 0, spawnDistance);
            }

            Vector3 newPos = transform.position + spawnOffset;
            newPos = new Vector3(Random.Range(betweenX[0], betweenX[1]), newPos.y, newPos.z);

            GameObject newBorn = Instantiate(enemies[Random.Range(0, enemies.Length)], newPos, new Quaternion());

            SetTime();
        }
    }

    void SetTime()
    {
        nextSpawn = Random.Range(spawnRate * (1 - rateFluctuation), spawnRate * (1 + rateFluctuation));
    }
}
