using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;

    private Vector2 shootAngle;
    public float timeToDestroy;
    public float speed = 10f;

    private GunStats gun;
    // Start is called before the first frame update
    void Start()
    {
        gun = PlayerCombat.activeWeapon; // encontra a referencia
        GetAngle();
        rb.velocity = shootAngle * speed;
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
            return;

        if (other.gameObject.tag.Equals("Solid"))
            Destroy(gameObject, 0.02f);

        if (other.gameObject.tag.Equals("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<EnemyStats>();
            enemy.DoDamage(gun.damage);
            Destroy(gameObject, 0.02f);
        }
    }

    private void GetAngle()
    {
        shootAngle = transform.rotation * Vector2.right;
    }

}
