using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("HUD")]
    public Image healhBar;
    public Image whiteHealthBar;
    public ManagerScript gameManager;

    [Header("Life")]
    private float maxLife = 100f;
    public float life;

    public float fillSpeed = 1f;

    [Header("Melee")]
    public float meleeDamage = 1f;
    public float attackRate = 1f;
    public float repelForce;

    // Start is called before the first frame update
    void Start()
    {
        life = maxLife;
        healhBar.fillAmount = NormalizedLife(life);
        whiteHealthBar.fillAmount = NormalizedLife(life);
    }

    // Update is called once per frame
    void Update()
    {
        updateWhiteBar();
    }
    public void DoDamage(float damage)
    {
        life -= damage;
        healhBar.fillAmount = NormalizedLife(life);
        if (life <= 0)
        {
            gameObject.SetActive(false);
            whiteHealthBar.fillAmount = 0f;
            gameManager.GameOver();
            // Destroy(gameObject);
        }
    }
    private float NormalizedLife(float life)
    {
        return life / maxLife;
    }

    public void setMaxLife(float newMaxLife)
    {
        maxLife = newMaxLife;
    }
    private void updateWhiteBar()
    {
        if (whiteHealthBar.fillAmount > healhBar.fillAmount)
        {
            whiteHealthBar.fillAmount -= Time.deltaTime * fillSpeed;
        }
        else whiteHealthBar.fillAmount = healhBar.fillAmount;
    }
}
