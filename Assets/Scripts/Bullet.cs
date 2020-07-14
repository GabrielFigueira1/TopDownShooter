using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;

    private Vector2 shootAngle;
    public float timeToAutoDestroy = 5f;
    public float speed;

    private GunStats gun;
    private LayerMask solidLayer = 1 << 8 | 1 << 9;
    // Start is called before the first frame update
    void Start()
    {
        gun = PlayerCombat.activeWeapon; // encontra a referencia
        GetAngle();
        rb.velocity = shootAngle * speed;
        Destroy(gameObject, timeToAutoDestroy);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        RaycastCollision();

    }

    private void GetAngle()
    {
        shootAngle = transform.rotation * Vector2.right;
        shootAngle.Normalize();
    }

    private void RaycastCollision()
    {
        RaycastHit2D ray;
        ray = Physics2D.Raycast(transform.position, shootAngle, speed * Time.fixedDeltaTime, solidLayer);
        if (ray)
        {
            transform.position = ray.point;

            if (ray.collider.tag.Equals("Solid"))
                Destroy(gameObject, 0.02f);
            if (ray.collider.tag.Equals("Enemy"))
            {
                var enemy = ray.collider.GetComponent<EnemyStats>();
                enemy.DoDamage(gun.damage);
                Destroy(gameObject, 0.02f);
            }
            Destroy(gameObject, 0.1f);
            rb.velocity = Vector3.zero;
        }
    }
}
