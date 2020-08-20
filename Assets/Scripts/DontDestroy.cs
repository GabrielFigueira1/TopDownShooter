using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    public GameObject[] ob;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        foreach(GameObject o in ob){
            DontDestroyOnLoad(o);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
