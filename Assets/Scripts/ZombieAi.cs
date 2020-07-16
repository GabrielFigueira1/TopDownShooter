using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAi : MonoBehaviour
{
    public enum state{
        idle,
        test,
        chase,
        pounce,
        attack
    }
    public enum test{
        chase,
        pounce,
        attack,
    }

    [Header("Components")]
    public Rigidbody2D rb;
    public Transform pivot;
    public Transform player;
    public Animator zombieAnimations;
    public EnemyStats enemyStats;
    public GameObject playerObject;

    [Header("Movimentation")]
    public float speed = 6f; //Velocidade do zumbi

    ///<summary>
    /// Vetor normalizado que aponta do zumbi para o player
    ///</summary>
    private Vector2 playerDirection; 
    ///<summary>
    /// Distância entre o inimigo e o player
    ///</summary>
    private float playerDistance;
    ///<summary>
    /// Angulo em z graus para o zumbi virar para o player
    ///</summary>
    private float angleToPlayer;    
    ///<summary>
    /// Indica se o zumbi pode dar dano
    ///</summary>
    private bool canDamage = true; 
    public float idleWalkSpeed = 1f;
    private Quaternion randomStartingAngle;
    public float randomAngle;
    public float frontCollisionRay = 1f;
    public float lerpingIdleSpeed = 1f;

    private bool isLerping;
    private float lerpAmount;

    public float sightRaycastOffset;
    public float sightRange = 5f;
    [Header("Pounce Parameters")]
    public float maxTestRange = 3f;
    public float minTestRange = 2f;
    public float pounceForce = 3f;
    public float pounceDuration = 1f;
    private float pounceEndTime ;
    private bool isPouncing = false;
    [Header("Attack Parameters")]
    public float attackRange = 1f;
    private int testResult;

    private int layerSolid = 1 << 8; //layer 8 solid

    [SerializeField]private int actualState;
    [SerializeField]private float nextTestTime;
    [SerializeField]private float maxRandomTimeBetweenTests = 2f;
    [SerializeField]private float minRandomTimeBetweenTests = 1f;
    private bool alreadyPounced;


    void Awake() {
        UpdateRandomAngle();
    }
    void Update()
    {
        RunStateMachine();
    }
    void FixedUpdate()
    {
        IdlePhysics();
        PouncePhysics();
        ChasePhysics();
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
    //Faz o zombie andar na direção do player
    {
        playerDirection =  (player.position - pivot.position).normalized;
        playerDistance = Vector2.Distance(pivot.position, player.position);
        angleToPlayer = Mathf.LerpAngle(angleToPlayer,
                        Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg, 6f*Time.deltaTime);
    }
    private void MoveToPlayer()
    {
        rb.AddForce(playerDirection * speed);
    }
    //Faz o zombie girar na direção do player
    private void RotateToPlayer()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angleToPlayer);
    }
    private void UpdateRandomAngle(){
        randomAngle = Random.Range(0f, 360f);
        transform.Rotate(new Vector3(0f,0f, randomAngle), Space.Self);
        randomStartingAngle = transform.rotation; 
    }
    
    private void LerpTolerpAmount(){
        if(isLerping){
            transform.Rotate(new Vector3(0f,0f, lerpingIdleSpeed), Space.Self);
            lerpAmount -= Time.fixedDeltaTime;
            if (lerpAmount <= 0)
                isLerping = false;
        }
    }
    private void IdlePhysics(){
        LerpTolerpAmount();
        if (!isLerping){
            rb.AddRelativeForce(Vector2.right * idleWalkSpeed);
            RaycastHit2D ray = Physics2D.Raycast(pivot.position, pivot.transform.TransformDirection(Vector2.right), frontCollisionRay, layerSolid);
            if (ray && !isLerping)
            {
                lerpAmount = Random.Range(0.7f, 1.5f) * Random.Range(-1, 2);
                isLerping = true;
            }
        }
    }

    private void ChasePhysics(){
        if(actualState == (int)state.chase){
            RotateToPlayer();
            MoveToPlayer();
        }
    }
    ///<summary>
    /// Atualiza a animação de andar do zombie verificando a velocidade dele
    ///</summary>
    private void UpdateWalkAnimation()
    {
        if (rb.velocity.magnitude > 0.2)
        {
            zombieAnimations.SetBool("isWalking", true);
        }
        else
            zombieAnimations.SetBool("isWalking", false);
    }

    /*Metodos de IA*/
   /*
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
    */

    //Testa se o player entrou dentro do alcance de visao do zombie
    private bool isOnSight()
    {
        if (Vector2.Distance(player.position, pivot.position) < sightRange)
        {
            return true;
        }
        else
            return false;
    }
    
    ///<summary>
    /// Traça raycasts para simular a visão do zombie
    ///</summary>
    private bool isSeeing()
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
    ///<summary>
    /// Retorna true se o zombie detectar o player
    ///</summary>
    private bool PlayerDetected(){
        if(isOnSight() && isSeeing()){
            return true;
        }
        else{
            return false;
        }
    }
    ///<summary>
    /// Retorna true se o player sair do alcance do zombie
    ///</summary>
    private bool PlayerRunnedAway(){
        if(!isOnSight() && !isSeeing()){
            return true;
        }
        return false;
    }
    ///<summary>
    ///Testa se o player esta no alcance para realiza os testes
    ///</summary>
    private bool IsOnTestRange(){
        if (Vector2.Distance(player.position, pivot.position) < maxTestRange
            && Vector2.Distance(player.position, pivot.position) > attackRange)
        {
            return true;
        }
        else
            return false;
    }
    private bool TestTimer(){
        if(nextTestTime < Time.time){
            return true;
        }
        return false;
    }
    private bool IsAbleToTest(){
        if(IsOnTestRange() && TestTimer()){
            return true;
        }
        return false;
    }
    private bool PounceEnded(){
        return false;
    }
    private bool IsOnAttackRange(){
        
        if(playerDistance < attackRange){
            return true;
        }
        return false;
    }
    private void Idle(){
        Debug.Log("IDLE");
    }
    private void Chase(){
        Debug.Log("CHASE");
    }
    ///<summary>
    ///Testa se o zombie vai dar um bote ou perseguir
    ///</summary>
    private void RandomZombieTest(){
        nextTestTime = Time.time + Random.Range(minRandomTimeBetweenTests, maxRandomTimeBetweenTests);
        testResult = Random.Range(0, 2);
        Debug.Log("TEST");
    }
    private void PounceReset(){
        alreadyPounced = false;
    }
    private void Pounce()
    {
        isPouncing = true;
        Debug.Log("POUNCE");
    }
    private void PouncePhysics(){
        if(isPouncing == true){
            if(!alreadyPounced){
                pounceEndTime = Time.time + pounceDuration;
                rb.AddForce(playerDirection.normalized * pounceForce, ForceMode2D.Impulse);
                rb.angularVelocity = 0f;
                zombieAnimations.Play("Base Layer.hit_zombie");
                alreadyPounced = true;
            }
            if(Time.time > pounceEndTime){
                isPouncing = false;
            }
        }
    }
    private void Attack(){
        Debug.Log("ATTACK");
    }

    //Freeze para previnir bugs na tela de game over
    private void Freeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    /// <summary>
    /// Classe que os processa os estados da máquina de estados
    /// </summary>
    private void RunStateMachine()
    {
        GetPlayerDirection();
        if(IsOnAttackRange()){
            actualState = (int)state.attack;
        }
        switch (actualState)
        {
            case (int)state.idle:
                if (PlayerDetected())
                {
                    actualState = (int)state.chase;
                }
                else
                {
                    Idle();
                }
                break;

            case (int)state.chase:
                if (PlayerRunnedAway())
                {
                    actualState = (int)state.idle;
                }
                if (IsAbleToTest())
                {
                    actualState = (int)state.test;
                }
                else
                {
                    Chase();
                }
                break;

            case (int)state.test:
                RandomZombieTest();
                PounceReset();
                if (testResult == (int)test.pounce)
                {
                    actualState = (int)state.pounce;
                }
                if (testResult == (int)test.chase)
                {
                    actualState = (int)state.chase;
                }
                break;
            case (int)state.pounce:
                if (!isPouncing && !alreadyPounced)
                {
                    Pounce();
                }
                else if(IsAbleToTest() && alreadyPounced)
                    actualState = (int)state.test;
                else if(alreadyPounced)
                    actualState = (int)state.chase;
                break;
            case (int)state.attack:
                if (!IsOnAttackRange())
                {
                    actualState = (int)state.chase;
                }
                else
                {
                    Attack();
                }
                break;
        }
    }
    /*Gizmos*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(pivot.position, sightRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pivot.position, maxTestRange);
        Gizmos.DrawWireSphere(pivot.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(pivot.position, playerDistance > sightRange ? sightRange * playerDirection : playerDistance * playerDirection);
        Gizmos.DrawRay(new Vector3(sightRaycastOffset * Mathf.Sin(-angleToPlayer * Mathf.Deg2Rad), sightRaycastOffset * Mathf.Cos(-angleToPlayer * Mathf.Deg2Rad), 0f)
        + pivot.position, playerDistance > sightRange ? sightRange * playerDirection : playerDistance * playerDirection);
        Gizmos.DrawRay(new Vector3(-sightRaycastOffset * Mathf.Sin(-angleToPlayer * Mathf.Deg2Rad), -sightRaycastOffset * Mathf.Cos(-angleToPlayer * Mathf.Deg2Rad), 0f)
        + pivot.position, playerDistance > sightRange ? sightRange * playerDirection : playerDistance * playerDirection);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pivot.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(pivot.position, frontCollisionRay*pivot.transform.TransformDirection(Vector2.right));

    }
    
}
    
