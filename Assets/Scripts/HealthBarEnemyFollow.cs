using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemyFollow : MonoBehaviour
{
    public Transform enemy;

    public float yOffset;
    public float xOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(enemy.position.x+xOffset, enemy.position.y+yOffset, transform.position.z);
    }
}
