using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private float bulletLife = 0f;
    [SerializeField] private float radius = 0; //radius for AOA's
    [SerializeField] private float followSpeed;
    [SerializeField] private float bulletDamage;

    private GameObject player;
    private bool kirinActive = false;
    [SerializeField] private GameObject shrapnel;
    [SerializeField] private GameObject Kirin_Lightning;
    [SerializeField] private bool enemyBullets = false;

    [SerializeField] private string bulletTypeString;
    [SerializeField] private LayerMask detectionLayer; // Set this in the Inspector to specify which layers the circle/AOA should detect

    private void OnDrawGizmos()
    {
        // Draw the detection circle in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (enemyBullets == true)
        {
            Physics2D.IgnoreLayerCollision(3, 7);
        }
        bulletLife = 0f;
    }
    private void FixedUpdate()
    {
        BulletType(bulletTypeString);
    }

    public void BulletType(string bulletType)
    {
        if (bulletType == "Homing Bullet") //homing bullet
        {
            Vector3 targetPos = player.transform.position;
            float speed = followSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
        }
        if (bulletType == "Pierce")
        {
            GameCurrency gameCurrency = FindObjectOfType<GameCurrency>();

            if (gameCurrency.Points <= 0)
            {
                return;
            }
            else
            {
                bulletLife = 1f;
                gameCurrency.Points -= 1;
            }
        }
        if (bulletType == "Kirin") //lightning AOE move
        {
            kirinActive = true;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, detectionLayer);
            foreach (Collider2D col in colliders)
            {
                col.GetComponent<Health>().Damage(bulletDamage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health healthScript = collision.GetComponent<Health>();

        if (kirinActive == true && healthScript != null)
        {
            Instantiate(Kirin_Lightning, collision.transform.position, Quaternion.identity);
            gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            kirinActive = false;
            radius = 1.5f;
        }
        if (collision.gameObject.CompareTag("Enemy") && healthScript != null)
        {
            healthScript.Damage(bulletDamage);
            Instantiate(shrapnel, collision.transform.position, Quaternion.identity);
            Destroy(gameObject, bulletLife); //add time to make a bullet that goes through enemies
        }
        if(enemyBullets ==  true && collision.gameObject.CompareTag("Enemy") && healthScript != null)
        {
            healthScript.Damage(bulletDamage);
            Instantiate(shrapnel, collision.transform.position, Quaternion.identity);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (enemyBullets == true && col.gameObject.CompareTag("Player"))
        {
            Health dealDamage = col.gameObject.GetComponent<Health>();
            dealDamage.Damage(bulletDamage);
            Destroy(gameObject, bulletLife);
        }
    }
}  
