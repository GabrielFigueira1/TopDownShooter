﻿using System.Collections;
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
    [Header("Userfull")]
    public bool isReloading;
    public bool isFullEmpty;
    public bool canReload;
    public bool canShoot;
    private float endReloadTime;

    private void Update()
    {
        HandleReloadTime();
        CheckBullets();
    }
    // Consome municao
    public void SpendAmmo()
    {
        loadedAmmo -= 1;
    }
    
    // Verifica se ainda ha balas
    private void CheckBullets(){
        if (extraAmmo == 0 && loadedAmmo == 0)
            isFullEmpty = true;
        else
            isFullEmpty = false;
    }
    // Inicia o processo de reload
    public void Reload()
    {
        if (extraAmmo > 0)
        {
            isReloading = true;
            endReloadTime = Time.time + reloadTime;
        }
    }
     // Metodo
    private void HandleReloadTime()
    {   
        if (Time.time > endReloadTime && isReloading)
        {
            ReloadUpdate();
            isReloading = false;
        }

        if (loadedAmmo < maxLoadedAmmo && extraAmmo > 0 && !isReloading)
            canReload = true;
        else 
            canReload = false;
        
    }
    // Atualiza os valores de municao apos o reload
    private void ReloadUpdate()
    {
        if (extraAmmo >= maxLoadedAmmo)
        {   // testa se ha municao suficiente para um pente inteiro
            extraAmmo -= maxLoadedAmmo - loadedAmmo;
            loadedAmmo = maxLoadedAmmo;
            
        }
        else if (extraAmmo > 0)
        {   // testa se ha alguma municao no inventario
            loadedAmmo += extraAmmo;
            if (loadedAmmo > maxLoadedAmmo){
                extraAmmo = loadedAmmo - maxLoadedAmmo;
                loadedAmmo = maxLoadedAmmo;
            }
            else if (loadedAmmo <= maxLoadedAmmo){
                extraAmmo = 0;
            }
        }
        else
        {
        }
    }

    // Verifica se a arma esta carregada
    public bool IsLoaded()
    {
        if (loadedAmmo > 0)
        {
            return true;
        }
        else
            return false;
    }

}



