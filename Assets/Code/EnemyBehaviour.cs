using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] int maxSpawnTimes = 3;
    [SerializeField] float speed = 5, maxHP = 3, shootDelay = 2, shootVariance = .25f, despawnRange = 25f;
    [SerializeField] Vector3 projectileOffset;
    [SerializeField] GameObject projectile, explosion;
    [SerializeField] GameObject[] pickups;

    float shootTimer, curHP;
    Rigidbody rb;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity);
        if (hitInfo.collider == null || hitInfo.transform.tag != "Wall")
            ReSpawn();

        rb = GetComponent<Rigidbody>();

        curHP = maxHP;

        if (player.position.z < transform.position.z)
            transform.rotation = new Quaternion(0, 1f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Kill();

        if (transform.position.y < -5f)
            ReSpawn();
    }

    void ReSpawn()
    {
        maxSpawnTimes--;
        if (maxSpawnTimes == 0)
            Destroy(gameObject);
        else
            transform.position = player.GetComponent<EnemySpawn>().RandomSpawnPosition();
    }

    void Move()
    {
        rb.velocity = transform.forward * speed;

        if (Vector3.Distance(player.position, transform.position) >= despawnRange)
            Destroy(gameObject);
    }

    void Kill()
    {
        if (shootTimer <= 0)
        {
            shootTimer = Random.Range(shootDelay * (1 - shootVariance), shootDelay * (1 + shootVariance));

            Transform shot = Instantiate(projectile, transform.position + projectileOffset, new Quaternion()).transform;
            shot.LookAt(player);
        }
        else
            shootTimer -= Time.deltaTime;
    }

    public void TakeDamage(float dmg)
    {
        if (curHP > 0)
        {
            curHP -= dmg;

            if (curHP <= 0)
            {
                foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
                    rend.enabled = false;

                explosion.SetActive(true);
                Destroy(gameObject, 2);

                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().ChangeScore(0);
                if (player.GetComponent<PlayerBehaviour>().InDanger() && Random.Range(0, 6) == 0)
                    Instantiate(pickups[0], transform.position + new Vector3(0, 1, 0), Quaternion.Euler(-60, 90, 0));
                else if (pickups.Length > 1 && Random.Range(0, 16) == 0)
                    Instantiate(pickups[Random.Range(1, pickups.Length)], transform.position + new Vector3(0, 1, 0), Quaternion.Euler(-60, 90, 0));

                GetComponent<EnemyBehaviour>().enabled = false;
            }
        }
    }
}
