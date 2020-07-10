using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector2 followOffset;
    public Rigidbody2D rb;

    public float speed = 2f;
    private Vector2 threshold;

    private Vector3 newPosition;
    private float moveSpeed;
    
    [Range(0, 1f)]
    public float cameraDistanceFactor;
    // Start is called before the first frame update
    void Start()
    {
        threshold = CalculateThreshold();
        Camera.main.transform.position = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {   
        Vector3 playerToPoint = (- player.position + Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector2 follow = player.position + (playerToPoint * cameraDistanceFactor);

        /*Calcula a distancia do objeto a camera*/
        float xDifference = Vector2.Distance(Vector2.right*transform.position.x, Vector2.right*follow.x);
        float yDifference = Vector2.Distance(Vector2.up*transform.position.y, Vector2.up*follow.y);
    
        newPosition = transform.position;
        if(Mathf.Abs(xDifference)>=threshold.x){
            newPosition.x = follow.x;
        }
        if(Mathf.Abs(yDifference)>=threshold.y){
            newPosition.y = follow.y;
        }
        
        moveSpeed = rb.velocity.magnitude > speed ? moveSpeed = Mathf.Lerp(moveSpeed, rb.velocity.magnitude, Time.deltaTime*2.3f): speed;
        
    }
    void FixedUpdate(){
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed*Time.deltaTime);

    }
    private Vector3 CalculateThreshold(){
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize*aspect.width/aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Vector2 border = CalculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x*2, border.y*2, 1));
    }
}
