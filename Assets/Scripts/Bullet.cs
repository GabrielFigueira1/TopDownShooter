using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    
    private Vector2 shootAngle;

    public float speed = 10f;
    public float bulletDamage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GetAngle();
        rb.velocity = shootAngle * speed;
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag.Equals("Player") )
            return;
        
        if (other.gameObject.tag.Equals("Solid"))
            Destroy(gameObject, 0.02f);
        
        if (other.gameObject.tag.Equals("Enemy")){
            var enemy = other.gameObject.GetComponent<EnemyStats>();
            enemy.DoDamage(bulletDamage);
            Destroy(gameObject, 0.02f);
        }
    }

    private void GetAngle(){
        shootAngle = transform.rotation * Vector2.right;
    }

}
