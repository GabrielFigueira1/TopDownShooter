using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public HealthBarEnemy bar;
    public float life = 10f;       //Vida do zumbi
    private float maxLife = 10f;
    public float enemyDamage = 2f;    //Dano base do zumbi
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoDamage(float damage){
        life -= damage;
        bar.UpdateBar(life/maxLife);
        if(life <=0 ){
            Destroy(transform.parent.gameObject);
    
        }
    }
}
