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

    public Vector2 minBondarys;
    public Vector2 maxBondarys;

    // Start is called before the first frame update
    void Start()
    {
        threshold = CalculateThreshold();
        Camera.main.transform.position = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 follow = player.transform.position;

        /*Calcula a distancia do objeto a camera*/
        float xDifference = Vector2.Distance(Vector2.right*transform.position.x, Vector2.right*follow.x);
        float yDifference = Vector2.Distance(Vector2.up*transform.position.y, Vector2.up*follow.y);
    
        Vector3 newPosition = transform.position;
        if(Mathf.Abs(xDifference)>=threshold.x){
            newPosition.x = follow.x;
        }
        if(Mathf.Abs(yDifference)>=threshold.y){
            newPosition.y = follow.y;
        }
        
        
        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
        
        

        transform.position = new Vector3
        (
        Mathf.Clamp (transform.position.x, minBondarys.x, maxBondarys.x),
        Mathf.Clamp (transform.position.y, minBondarys.y, maxBondarys.y),
        transform.position.z
        );
        
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

        Gizmos.DrawLine(Vector3.up*(minBondarys.y-Camera.main.orthographicSize), Vector3.up*(maxBondarys.y+Camera.main.orthographicSize));

        Gizmos.DrawLine(Vector3.right*(minBondarys.x-Camera.main.orthographicSize*Camera.main.aspect), Vector3.right*(maxBondarys.x+Camera.main.orthographicSize*Camera.main.aspect));

        
    }
}
