using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieOptimization : MonoBehaviour
{
    public Transform player;
    public GameObject zombie;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerPivot").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) < 8f)
            zombie.SetActive(true);
        if(Vector2.Distance(transform.position, player.position) > 10f)
            zombie.SetActive(false);
    }
}
