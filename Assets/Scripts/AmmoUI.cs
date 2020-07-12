using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadedAmmo;
    [SerializeField] private TextMeshProUGUI extraAmmo;
    [SerializeField] private Image reloadBar;

    private float reloadTime;
    private float reloadStartTime;
    // Start is called before the first frame update
    void Update(){
        UpdateRealoadBar();
    }
    void Start()
    {
        loadedAmmo.text = "0";
        extraAmmo.text = "0";
    }
    // Atualiza os valor da municao
    public void UpdateAmmoUI(int loaded, int extra){
        loadedAmmo.text = loaded.ToString();
        extraAmmo.text = extra.ToString();
    }
    // Atualiza o filling da barra de reload
    private void UpdateRealoadBar(){
        reloadBar.fillAmount = Mathf.Clamp01(Mathf.Abs((reloadStartTime - Time.time)/reloadTime));      
    }
    // Atualiza os valores para os calculos da barra de reload
    public void ReloadBar(bool isReloading, float reloadTime, float reloadStartTime){
        this.reloadTime = reloadTime;        
        this.reloadStartTime = reloadStartTime;        
        
        if (isReloading){
            reloadBar.gameObject.SetActive(true);
            UpdateRealoadBar();
        }
        else{
            reloadBar.gameObject.SetActive(false);
        }
        

    }   
}
