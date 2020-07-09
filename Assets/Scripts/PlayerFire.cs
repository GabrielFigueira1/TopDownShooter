using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Transform firePosition;
    public GameObject Bullet;
    public Animator playerAnimation;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Instantiate(Bullet, firePosition.position, firePosition.rotation);
            playerAnimation.Play("Base Layer.Fire");
        }
    }
}
