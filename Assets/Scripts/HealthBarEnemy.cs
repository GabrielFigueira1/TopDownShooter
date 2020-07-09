using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemy : MonoBehaviour
{
    public Transform lifeBar;
    public Transform whiteBar;
    public float effectSpeed;
    private float actualLife;
    private float whiteBarLife;
    private const int speedNormalization = 100;

    // Start is called before the first frame update
    void Start()
    {
        whiteBarLife = whiteBar.localScale.x;
        actualLife = whiteBar.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        whiteBarEffect(effectSpeed);
    }
    //altera a escala da healthBar
    public void UpdateBar(float newLife)
    {
        lifeBar.localScale = new Vector3(newLife, lifeBar.localScale.y, lifeBar.localScale.z);
        actualLife = newLife;
    }
    private void whiteBarEffect(float speed)
    {
        if (whiteBarLife > actualLife)
        {
            whiteBarLife -= Time.deltaTime * effectSpeed/speedNormalization;
            whiteBar.localScale = new Vector3(whiteBarLife, whiteBar.localScale.y, whiteBar.localScale.z);
            
        }
    }
}
