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

    private LayerMask enemyLayer = 1 << 9;
    [Header("References")]
    public PlayerStats playerStats;
    public Transform playerPivot;



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
            selectedWeapon = weaponChange.selectedWeapon;
            activeWeapon = weaponChange.selectedWeaponReference;
            UpdateAmmoUI();
        }
        // Se pode atirar
        if (Input.GetMouseButtonDown(0) && activeWeapon.canShoot) //left button
        {
            activeWeapon.SpendAmmo();
            Instantiate(Bullet, firePosition.position, firePosition.rotation);
            playerAnimation.Play("Base Layer.Fire");
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
        if (Input.GetMouseButtonDown(1) && CanMeleeAtack())
        { //right button
            // Animacao de ataque melee
            playerAnimation.Play("Base Layer.Melee");

            // Detecta hits em inmigos
            Collider2D[] hit = Physics2D.OverlapCircleAll(meleePoint.position, meleeRadius, enemyLayer);
            // Repele os inimigos e aplica dano a eles
            foreach (Collider2D enemy in hit)
            {
                enemy.GetComponent<EnemyStats>().DoDamage(playerStats.meleeDamage); // aplica dano
                var enemyTransform = enemy.GetComponent<Transform>();
                var enemyRb = enemy.GetComponent<Rigidbody2D>();

                Vector2 repelDirection = enemyTransform.position - playerPivot.position; // calcula a direcao que o inimigo vai ser repelido 
                repelDirection.Normalize();

                enemyRb.AddForce(repelDirection * playerStats.repelForce, ForceMode2D.Impulse);
            }
            // Atualiza o timer do attackRate 
            meleeAttackTime = Time.time;

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

    private void UpdateAmmoUI()
    {
        activeWeapon.UpdateUI();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(meleePoint.position, meleeRadius);
    }
}
