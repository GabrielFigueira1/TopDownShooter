using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Fire and guns")]
    public Transform firePosition;
    public GameObject Bullet;
    private int selectedWeapon;

    public static GunStats activeWeapon;
    public WeaponChangeHandler weaponChange;

    [Header("Animations")]
    public Animator playerAnimation;

    [Header("Melee")]
    public Transform meleePoint;

    public float meleeRadius;

    private float meleeAttackTime = 0f;
    public bool isOnMeleeAtack;
    [SerializeField] private float meleeAttackDuration = 0.5f;

    private LayerMask enemyLayer = 1 << 9;
    [Header("References")]
    public PlayerStats playerStats;
    public Transform playerPivot;

    private List<int> hittedEnemys = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        SetupInitialWeapon();
        UpdateAmmoUI();
    }

    // Update is called once per frame
    void Update()
    {
        HandleFire();
        HandleReload();
        MeleeAtack();
    }

    // Metodo de atirar do player
    private void HandleFire()
    {
        // Pega a referencia da arma selecionada
        if (selectedWeapon != weaponChange.selectedWeapon)
        {
            activeWeapon.cancelReload(); // cancela um reload se estiver acontecendo
            selectedWeapon = weaponChange.selectedWeapon; // pega o numero referencia da arma selecionada
            activeWeapon = weaponChange.selectedWeaponReference; // pega a referencia da arma selecionada
            UpdateAmmoUI();
        }
        // Se pode atirar
        if (Input.GetMouseButtonDown(0) && activeWeapon.canShoot) //left button
        {
            activeWeapon.cancelReload(); // cancela um reload se estiver acontecendo
            activeWeapon.SpendAmmo(); // gasta municao
            Instantiate(Bullet, firePosition.position, firePosition.rotation); // instancia um objeto bullet
            playerAnimation.Play("Base Layer.Fire"); // executa animacao de tiro do player
        }
        else if (Input.GetMouseButtonDown(0) && activeWeapon.canReload)
        { // Sem municao. Da o reload ao tentar atirar
            activeWeapon.Reload();
        }
    }
    // Metodo para recarregar apertando a letra r
    private void HandleReload()
    {
        if (Input.GetButtonDown("Reload") && activeWeapon.canReload)
        {
            activeWeapon.Reload();
        }
    }

    // Metodo de ataque melee do player
    private void MeleeAtack()
    {
        HandleMelee();
        if (Input.GetMouseButtonDown(1) && CanMeleeAtack())
        { //right button
            isOnMeleeAtack = true;
            activeWeapon.cancelReload(); // cancela um reload se estiver acontecendo
            // Animacao de ataque melee
            playerAnimation.Play("Base Layer.Melee");
            // Atualiza o timer do attackRate 
            meleeAttackTime = Time.time;

        }
    }
    private void HandleMelee()
    {
        if (isOnMeleeAtack)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(meleePoint.position, meleeRadius, enemyLayer);
            // Repele os inimigos e aplica dano a eles
            foreach (Collider2D enemy in hit)
            {
                if (hittedEnemys.Contains(enemy.GetHashCode()))
                {
                   
                }
                else
                {
                    enemy.GetComponent<EnemyStats>().DoDamage(playerStats.meleeDamage); // aplica dano
                    var enemyTransform = enemy.GetComponent<Transform>();
                    var enemyRb = enemy.GetComponent<Rigidbody2D>();

                    Vector2 repelDirection = enemyTransform.position - playerPivot.position; // calcula a direcao que o inimigo vai ser repelido 
                    repelDirection.Normalize();

                    enemyRb.AddForce(repelDirection * playerStats.repelForce, ForceMode2D.Impulse);
                    hittedEnemys.Add(enemy.GetHashCode());
                }
            }
            if (meleeAttackDuration + meleeAttackTime < Time.time)
            {
                isOnMeleeAtack = false;
                hittedEnemys.Clear();
            }
        }
    }

    private bool CanMeleeAtack()
    {
        if (meleeAttackTime + playerStats.attackRate < Time.time || meleeAttackTime == 0f)
        {
            return true;
        }
        else
            return false;
    }

    private void SetupInitialWeapon()
    {
        selectedWeapon = weaponChange.selectedWeapon;
        activeWeapon = weaponChange.selectedWeaponReference;
    }
    /// <summary>
    /// Atualiza os valores da municao e icone da arma na UI
    /// </summary>
    private void UpdateAmmoUI()
    {
        activeWeapon.UpdateUI();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleePoint.position, meleeRadius);
    }
}
