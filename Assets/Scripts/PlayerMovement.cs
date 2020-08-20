using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float xAxis;
    private float yAxis;
    private Vector2 Axis;

    private Vector3 mousePos;
    private float angle;
    private Vector2 angleVector;
    public Transform pivot;

    public Rigidbody2D rb;
    public Animator playerAnimation;

    private Vector2 playerVelocity;
    public float acceleration;

    [Range(1,2)]
    public float defaultSprintMultiplier;
    private float sprintMultiplier = 1f;

    public float mouseDeadZone = 1f;
    public float turnRate;

    //Audio
    private bool isPlayingFootsteps;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isGamePaused){
            RotatePlayer();
            UpdateAxisInputs();
            UpdateWalkAnimation();
            UpdateSprintButton();
        }
    }

    void FixedUpdate()
    {
        SmoothMovement();
    }

    private void UpdateAxisInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        Axis = new Vector2(xAxis, yAxis);
        Axis.Normalize();
    }

    private void RotatePlayer()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angleVector = mousePos - pivot.position;

        if(angleVector.magnitude > mouseDeadZone){
            angle = Mathf.Atan2(angleVector.y, angleVector.x) * Mathf.Rad2Deg;
            pivot.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        
    }
    private void UpdateWalkAnimation()
    {
        if (rb.velocity.magnitude > 0.1){
            playerAnimation.SetBool("isWalking", true);
            if(!isPlayingFootsteps){
                isPlayingFootsteps = true;
                FindObjectOfType<AudioManager>().Play("Footsteps");
            }
        }
        else{
            if(isPlayingFootsteps){
                FindObjectOfType<AudioManager>().Stop("Footsteps");
            }
            isPlayingFootsteps = false;
            playerAnimation.SetBool("isWalking", false);
        }
    }

    private void SmoothMovement()
    {
        rb.AddForce(Axis * acceleration * sprintMultiplier);
    }

    private void UpdateSprintButton()
    {
        if(Input.GetButton("Run"))
            sprintMultiplier = defaultSprintMultiplier;
    
        else
            sprintMultiplier = 1f;            
    }
    /*
    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(pivot.position, mouseDeadZone);
    }
    */
}