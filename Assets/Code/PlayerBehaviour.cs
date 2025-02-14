﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] float speed = 5, shootDelay = 1f / 12f, maxHP = 100, inincibilityWindow = 2f;
    [SerializeField] RectTransform hpFront;
    [SerializeField] GameObject projectile, explosion;
    [SerializeField] Collider trigger;
    [SerializeField] Color neutralColour, invincibleColour, dangerColour;

    float shootTimer = 0, grabTimer = 0, curHP, hpFrontWidth, invincibilityTimer, explosionTimer;
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
        if (explosionTimer <= 0)
        {
            if (curHP > 0)
                explosion.SetActive(false);
            else
                SceneManager.LoadScene("Lose");
        }
        else
            explosionTimer -= Time.deltaTime;

        if (invincibilityTimer > 0)
        {
            hpFront.GetComponent<Image>().color = invincibleColour;
            invincibilityTimer -= Time.deltaTime;
        }
        else if (InDanger())
            hpFront.GetComponent<Image>().color = dangerColour;
        else
            hpFront.GetComponent<Image>().color = neutralColour;

        if (curHP > 0)
        {
            Move();
            Drop();

            if (hold == null)
                Kill();
        }
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
            Instantiate(projectile, transform.position - transform.right * .42f + transform.forward * .1f, transform.rotation);
            Instantiate(projectile, transform.position + transform.right * .42f + transform.forward * .1f, transform.rotation);
        }
        else
            shootTimer += Time.deltaTime;
    }

    public void TakeDamage(float dmg)
    {
        if (dmg > 0 && invincibilityTimer <= 0)
        {
            explosion.SetActive(false);
            explosion.SetActive(true);
            explosionTimer = inincibilityWindow;
        }
        if ((dmg > 0 && invincibilityTimer <= 0) || dmg < 0)
        {
            invincibilityTimer = inincibilityWindow;

            curHP = Mathf.Clamp(curHP - dmg, 0, maxHP);
            hpFront.sizeDelta = new Vector2(hpFrontWidth * curHP / maxHP, hpFront.sizeDelta.y);
        }

        if (curHP <= 0)
        {
            CommandToDrop();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    public bool InDanger()
    {
        return curHP < maxHP/2;
    }
}
