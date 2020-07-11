using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Header("Fire and guns")]
    public Transform firePosition;
    public GameObject Bullet;

    [Header("Animations")]
    public Animator playerAnimation;
    
    [Header("Melee")]
    public Transform meleePoint;

    public float meleeRadius;

    private float meleeAttackTime = 0f;

    private LayerMask enemyLayer = 1 << 9;
    [Header ("References")]
    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleFire();
        MeleeAtack();
    }

    // Metodo de atirar do player
    private void HandleFire()
    {
        if (Input.GetMouseButtonDown(0)) //left button
        {
            Instantiate(Bullet, firePosition.position, firePosition.rotation);
            playerAnimation.Play("Base Layer.Fire");
        }
    }

    // Metodo de ataque melee do player
    private void MeleeAtack()
    {
        if(Input.GetMouseButtonDown(1) && CanMeleeAtack()){ //right button
            // Animacao de ataque melee
            playerAnimation.Play("Base Layer.Melee");

            // Detecta hits em inmigos
            Collider2D[] hit = Physics2D.OverlapCircleAll(meleePoint.position, meleeRadius, enemyLayer);
            // Repele os inimigos e aplica dano a eles
            foreach (Collider2D enemy in hit)
            {
                enemy.GetComponent<EnemyStats>().DoDamage(playerStats.meleeDamage);
            }
            // Atualiza o timer do attackRate 
                meleeAttackTime = Time.time;

        }
    }

    private bool CanMeleeAtack(){
        if(meleeAttackTime + playerStats.attackRate < Time.time || meleeAttackTime == 0f){
            return true;
        }
        else
            return false;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(meleePoint.position, meleeRadius);
    }

}
