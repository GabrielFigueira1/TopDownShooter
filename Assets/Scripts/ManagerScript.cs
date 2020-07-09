using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ManagerScript : MonoBehaviour
{
    public PlayerStats playerStats;
    public Text gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        gameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Restart")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


    }
    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver(){
        gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0.25f;
    }
}
