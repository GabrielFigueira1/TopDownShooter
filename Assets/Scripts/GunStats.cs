using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats : MonoBehaviour
{
    [Header("Stats")]
    public float damage;
    public float fireRate;
    public bool isAutomatic;
    public float reloadTime;
    public int maxLoadedAmmo;
    public int maxExtraAmmo;
    [Header("Inventory")]
    public int loadedAmmo;
    public int extraAmmo;
    

// Consome municao
public void SpendAmmo(){
    loadedAmmo -= 1;
    Debug.Log("Loaded ammo: " + loadedAmmo);
}
// Recarrega a arma
public void Reload(){
    if (extraAmmo >= maxLoadedAmmo){    // testa se ha municao suficiente para um pente inteiro
        loadedAmmo = maxLoadedAmmo;
        extraAmmo -= loadedAmmo;
    }
    else if (extraAmmo > 0){            // testa se ha alguma municao no inventario
        loadedAmmo = extraAmmo;
        extraAmmo = 0;
    }
    else
    {
    }
}
// Verifica se a arma esta carregada
public bool IsLoaded(){
    if(loadedAmmo > 0){
        return true;
    }
    else
        return false;
}

}



