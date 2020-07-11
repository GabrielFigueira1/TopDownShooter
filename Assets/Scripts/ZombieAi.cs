using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAi : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public Transform pivot;
    public Transform player;
    public Animator zombieAnimations;
    public EnemyStats enemyStats;
    public GameObject playerObject;

    [Header("Movimentation")]
    public float speed = 6f; //Velocidade do zumbi

    private Vector2 playerDirection; //Vetor que aponta do zumbi para o player
    private float playerDistance;
    private float angleToPlayer;    //Angulo em z graus para o zumbi virar para o player

    private bool canDamage = true;  //Indica se o zumbi pode dar dano

    public float sightRaycastOffset;
    public float sightRange = 5f;
    [Header("Pounce Parameters")]
    public float maxPounceRange = 3f;
    public float minPounceRange = 2f;
    public float pounceForce = 3f;
    public float pounceTime = 1f;
    private bool isPouncing = false;

    int layerSolid = 1 << 8; //layer 8 solid

    void Update()
    {

        isOnLineOfSight();
        if (playerObject.activeSelf)
        {
            GetPlayerDirection();
            updateWalkAnimation();
        }
        else
            Freeze();

    }
    void FixedUpdate()
    {
        if (playerObject.activeSelf)
            Patrol();
        else
            Freeze();
    }

    /*Colisao que inicia uma corrotina de dano no player se o zumbi colidir com ele*/
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (canDamage)
            {
                var playerStats = other.gameObject.GetComponent<PlayerStats>();
                StartCoroutine("DamagePlayer", playerStats);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            //StopCoroutine("DamagePlayer");
        }
    }
    /*Corrotina de dano do zumbi aplicado no player*/
    IEnumerator DamagePlayer(PlayerStats playerStats)
    {
        zombieAnimations.Play("Base Layer.hitting_zombie");
        zombieAnimations.SetBool("isHitting", true);
        int i;
        playerStats.DoDamage(enemyStats.enemyDamage);
        canDamage = false;
        for (i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(0.8f);
        zombieAnimations.SetBool("isHitting", false);
        canDamage = true;
    }

    //Metodos de movimento
    private void GetPlayerDirection()
    {
        playerDirection = (player.position - pivot.position).normalized;
        playerDistance = Vector2.Distance(pivot.position, player.position);
        angleToPlayer = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
    }
    private void Move()
    {
        rb.velocity = playerDirection * speed;
    }
    private void RotateToPlayer()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
    }
    private void updateWalkAnimation()
    {
        if (rb.velocity.magnitude > 0.2)
        {
            zombieAnimations.SetBool("isWalking", true);
        }
        else
            zombieAnimations.SetBool("isWalking", false);
    }

    /*Metodos de IA*/
    private void Patrol()
    {
        if (isOnLineOfSight()) //testes do raycast
        {
            if (isWithinPounceRange() && !isPouncing)
            {
                RotateToPlayer();
                StartCoroutine("PounceTimer", pounceTime);
            }
            else if (isWithinReachOfSight() && !isPouncing)
            {
                Move();
                RotateToPlayer();
            }
            else if (!isWithinPounceRange() && !isWithinReachOfSight() && !isPouncing)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
            else if (isPouncing)
            {
                RotateToPlayer();
            }
        }
    }
    /*Metodos do pounce*/
    private void Pounce(float force)
    {
        rb.AddForce(playerDirection.normalized * force, ForceMode2D.Impulse);
    }
    IEnumerator PounceTimer(float time)
    {
        isPouncing = true;
        rb.angularVelocity = 0f;
        yield return new WaitForSeconds(time);
        Pounce(pounceForce);
        zombieAnimations.Play("Base Layer.hit_zombie");
        yield return new WaitForSeconds(time / 2);
        isPouncing = false;

    }
    //Testa se o player entrou dentro do alcance de visao do zombie
    private bool isWithinReachOfSight()
    {
        if (Vector2.Distance(player.position, pivot.position) < sightRange)
        {
            return true;
        }
        else
            return false;
    }
    //Testa se o player esta no alcance do pounce
    private bool isWithinPounceRange()
    {
        if (Vector2.Distance(player.position, pivot.position) < maxPounceRange && Vector2.Distance(player.position, pivot.position) > minPounceRange)
        {
            return true;
        }
        else
            return false;
    }
    //Raycast para simular a linha de visao do zombie
    private bool isOnLineOfSight()
    {
        RaycastHit2D raySolid1;
        RaycastHit2D raySolid2;
        RaycastHit2D raySolid3;
        raySolid1 = Physics2D.Raycast(pivot.position, playerDirection, playerDistance > sightRange ? sightRange : playerDistance, layerSolid);
        raySolid2 = Physics2D.Raycast(new Vector2(sightRaycastOffset * Mathf.Sin(-angleToPlayer * Mathf.Deg2Rad) + pivot.position.x,
        sightRaycastOffset * Mathf.Cos(-angleToPlayer * Mathf.Deg2Rad) + pivot.position.y), playerDirection,
        playerDistance > sightRange ? sightRange : playerDistance, layerSolid);
        raySolid3 = Physics2D.Raycast(new Vector2(-sightRaycastOffset * Mathf.Sin(-angleToPlayer * Mathf.Deg2Rad) + pivot.position.x,
        -sightRaycastOffset * Mathf.Cos(-angleToPlayer * Mathf.Deg2Rad) + pivot.position.y), playerDirection,
        playerDistance > sightRange ? sightRange : playerDistance, layerSolid);

        if (raySolid1 || raySolid2 || raySolid3)
        {
            if (raySolid1.transform != null && raySolid1.transform.gameObject.CompareTag("Player"))
            {

                return true;
            }
            if (raySolid2.transform != null && raySolid2.transform.gameObject.CompareTag("Player"))
            {

                return true;
            }
            if (raySolid3.transform != null && raySolid3.transform.gameObject.CompareTag("Player"))
            {

                return true;
            }
            else

                return false;
        }
        else
        {

            return false;
        }
    }

    //Freeze para previnir bugs na tela de game over
    private void Freeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    /*Gizmos*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(pivot.position, sightRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pivot.position, maxPounceRange);
        Gizmos.DrawWireSphere(pivot.position, minPounceRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(pivot.position, playerDistance > sightRange ? sightRange * playerDirection : playerDistance * playerDirection);
        Gizmos.DrawRay(new Vector3(sightRaycastOffset * Mathf.Sin(-angleToPlayer * Mathf.Deg2Rad), sightRaycastOffset * Mathf.Cos(-angleToPlayer * Mathf.Deg2Rad), 0f)
        + pivot.position, playerDistance > sightRange ? sightRange * playerDirection : playerDistance * playerDirection);
        Gizmos.DrawRay(new Vector3(-sightRaycastOffset * Mathf.Sin(-angleToPlayer * Mathf.Deg2Rad), -sightRaycastOffset * Mathf.Cos(-angleToPlayer * Mathf.Deg2Rad), 0f)
        + pivot.position, playerDistance > sightRange ? sightRange * playerDirection : playerDistance * playerDirection);
    }
}
