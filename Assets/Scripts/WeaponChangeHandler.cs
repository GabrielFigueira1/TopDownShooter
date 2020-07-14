using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChangeHandler : MonoBehaviour
{
    enum weapons{
        pistol,
        rifle,
        shotgun
    }

    public int selectedWeapon;
    
    public GunStats selectedWeaponReference; // Referencia GunStats da arma selecionada

    // Start is called before the first frame update
    void Start()
    {
        ActiveSelectedWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        SelectWeapon();
    }
    // Metodo que cuida da selecao de uma arma
    private void SelectWeapon(){
        if(Input.GetButtonDown("PrimaryWeapon")){
            selectedWeapon = (int)weapons.pistol;
            ActiveSelectedWeapon();
        }
        if(Input.GetButtonDown("SecondaryWeapon")){
            selectedWeapon = (int)weapons.rifle;
            ActiveSelectedWeapon();
        }
    }
    // Ativa a arma selecionada e desativa as outras
    private void ActiveSelectedWeapon(){
        int i = 0; 
        foreach(Transform weapon in transform){
            if(i == selectedWeapon){ // pega a referencia da arma selecionada e seta ela como ativa
                selectedWeaponReference = weapon.GetComponent<GunStats>();
                weapon.gameObject.SetActive(true);
            }
            else{
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
    
}
