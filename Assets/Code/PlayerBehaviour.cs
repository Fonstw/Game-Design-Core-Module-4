using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 5, shootDelay = 1f / 12f, maxHP = 100, regenDelay = 2f, regenSpeed = 2f;
    [SerializeField] RectTransform hpFront;
    [SerializeField] GameObject projectile;
    [SerializeField] Collider trigger;

    float shootTimer = 0, grabTimer = 0, curHP, hpFrontWidth, regenTimer;
    Rigidbody rb;
    GameObject hold = null;

    void Start()
    {
        trigger = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

        curHP = Mathf.Clamp(maxHP, 1, Mathf.Infinity);
        hpFrontWidth = hpFront.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        Heal();
        Move();
        Drop();

        if (hold == null)
            Kill();
    }

    private void OnTriggerStay(Collider other)
    {
        if (hold == null && grabTimer <= 0 && other.tag == "Construct" && Input.GetButton("Grab"))
        {
            other.transform.SetParent(transform);
            other.transform.localPosition = new Vector3(0, 0, 1);
            hold = other.gameObject;
            grabTimer = .25f;
        }
    }

    public bool IsHolding()
    {
        return hold != null;
    }

    public void CommandToDrop()
    {
        if (IsHolding())
        {
            hold.transform.SetParent(null);
            hold = null;
        }
    }

    void Heal()
    {
        if (regenTimer <= 0 && curHP < maxHP)
            TakeDamage(-regenSpeed * Time.deltaTime);
        else
            regenTimer -= Time.deltaTime;
    }

    void Move()
    {
        Vector3 movement = new Vector3();

        if (Input.GetAxis("LeftHorizontal") != 0)
            movement.z = Input.GetAxis("LeftHorizontal");
        if (Input.GetAxis("LeftVertical") != 0)
            movement.x = -Input.GetAxis("LeftVertical");

        if (movement.magnitude > 1)
            movement = movement.normalized;
        movement *= speed;

        rb.velocity = movement;
    }

    void Drop()
    {
        if (grabTimer <= 0)
        {
            if (Input.GetButtonDown("Grab"))
            {
                CommandToDrop();
                grabTimer = .25f;
            }
        }
        else
            grabTimer -= Time.deltaTime;
    }

    void Kill()
    {
        shootTimer += Time.deltaTime;
        
        Vector3 direction = new Vector3();

        if (Input.GetAxis("RightHorizontal") != 0)
            direction.z = Input.GetAxis("RightHorizontal");
        if (Input.GetAxis("RightVertical") != 0)
            direction.x = -Input.GetAxis("RightVertical");

        if (direction == Vector3.zero)
            return;

        if (direction.magnitude > 1)
            direction = direction.normalized;

        transform.rotation = Quaternion.LookRotation(direction);

        if (shootTimer >= shootDelay)
        {
            shootTimer = 0;
            Instantiate(projectile, transform.position, transform.rotation);
        }
    }

    public void TakeDamage(float dmg)
    {
        curHP = Mathf.Clamp(curHP - dmg, 0, maxHP);
        hpFront.sizeDelta = new Vector2(hpFrontWidth * curHP / maxHP, hpFront.sizeDelta.y);

        if (dmg > 0)
            regenTimer = regenDelay;

        if (curHP <= 0)
            SceneManager.LoadScene(0);
    }
}
