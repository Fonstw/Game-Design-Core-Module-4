using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldEnemyBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 5, maxHP = 3, fireRate = 2, fireVariance = .25f, maxPlayerDistance = 35;
    [SerializeField] GameObject projectile;

    float curHP, fireTimer;
    PlayerBehaviour player;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // find your target for future reference
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();

        if (player.transform.position.z < transform.position.z)
            transform.rotation = new Quaternion(0, 0, 0, 1f);
        else
            transform.rotation = new Quaternion(0, 1f, 0, 0);
        
        // find your solidity for future reference
        rb = GetComponent<Rigidbody>();

        // set HP to a minimum of 1
        curHP = Mathf.Clamp(maxHP, 1, Mathf.Infinity);

        // set fire timer so I don't shoot immediately at spawn
        ResetFireTimer();
    }

    // Update is called once per frame
    void Update()
    {
        // do not do duck
        Move();

        // shoot every once in a while
        HandleShooting();
    }

    void ResetFireTimer()
    {
        // shoot randomly between ex. 75% and 125% of fireRate
        fireTimer = fireRate * Random.Range(1f - fireVariance, 1f + fireVariance);
    }

    void HandleShooting()   // the police of Texas should get around this someday
    {
        // count down
        fireTimer -= Time.deltaTime;
        // if done counting (=supposed to shoot)
        if (fireTimer <= 0)
        {
            // make a projectile (at my own position)
            GameObject shot = Instantiate(projectile, transform.position, new Quaternion());
            // tell it to look at where the player is now
            shot.transform.LookAt(player.transform.position);
            // Set fire timer again so I don't suddenly start shooting an endless stream of projectiles out of the blue
            ResetFireTimer();
        }
    }

    void Move()
    {
        // move forward according to your speed and the flow of time (=speed doesn't change when framerate does)
        rb.velocity = -transform.forward * speed;

        if (Vector3.Distance(transform.position, player.transform.position) > maxPlayerDistance)
            Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        // substract dmg (possible negative for healing) from curHP
        // to a minimum of 0 (=dead) and a maximum of maxHP (duh!)
        curHP = Mathf.Clamp(curHP - dmg, 0, maxHP);

        if (curHP <= 0)
            Destroy(gameObject);
    }
}
