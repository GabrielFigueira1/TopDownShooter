using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 150f;

    private float xAxis;
    private float yAxis;
    private Vector2 Axis;

    private Vector3 mousePos;
    private float angle;
    private Vector2 angleVector;
    public Transform pivot;

    public Rigidbody2D rb;
    public Animator playerAnimation;

    private float orthographicSize = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        calculateOrthographicSize();
        rotatePlayer();
        updateAxisInputs();
        updateWalkAnimation();
    }

    void FixedUpdate() {
        
        rb.velocity = Axis*speed*Time.deltaTime; //Movimentacao
    }
    void LateUpdate() {
        cameraZoomEfect(orthographicSize);
    }

    private void updateAxisInputs(){
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        Axis = new Vector2(xAxis, yAxis);
        Axis.Normalize();
    }

    private void rotatePlayer(){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angleVector = mousePos - pivot.position;
        angle = Mathf.Atan2(angleVector.y, angleVector.x) * Mathf.Rad2Deg;
        pivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    void updateWalkAnimation(){
        if(rb.velocity.magnitude > 0.1)
            playerAnimation.SetBool("isWalking", true);
        else
            playerAnimation.SetBool("isWalking", false);
    }

    private void cameraZoomEfect(float newOrthoSize){

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, newOrthoSize, 0.05f);
    }
    private void calculateOrthographicSize(){
       if(Mathf.Abs(pivot.position.x - mousePos.x) < 4.5f && Mathf.Abs(pivot.position.y - mousePos.y) < 3f){
           orthographicSize = 5f;
       }
        else
            orthographicSize = 5.2f;
    }

}

