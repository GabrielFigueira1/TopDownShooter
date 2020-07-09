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
    public float dashForce = 4f;

    private bool isDashing = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        UpdateAxisInputs();
        UpdateWalkAnimation();
        GetDashButton();
    }

    void FixedUpdate()
    {
        SmoothMovement();
        Dash();
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
        angle = Mathf.Atan2(angleVector.y, angleVector.x) * Mathf.Rad2Deg;
        pivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void UpdateWalkAnimation()
    {
        if (rb.velocity.magnitude > 0.1)
            playerAnimation.SetBool("isWalking", true);
        else
            playerAnimation.SetBool("isWalking", false);
    }

    private void SmoothMovement()
    {
        rb.AddForce(Axis * acceleration);
    }

    private void GetDashButton()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isDashing = true;
        }
    }

    private void Dash()
    {
        if (isDashing)
        {
            if (rb.velocity.magnitude < 0.1 && Axis.magnitude == 0)
                rb.AddForce(angleVector.normalized * dashForce, ForceMode2D.Impulse);
            else
                rb.AddForce(rb.velocity.normalized * dashForce, ForceMode2D.Impulse);
            isDashing = false;
        }
    }
}