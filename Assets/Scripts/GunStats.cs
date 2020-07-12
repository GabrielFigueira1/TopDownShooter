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
    [Header("Userfull")]
    public bool isReloading;
    public bool isFullEmpty;
    private float endReloadTime;

    private void Update()
    {
        HandleReloadTime();
    }
    // Consome municao
    public void SpendAmmo()
    {
        loadedAmmo -= 1;
    }
    // Inicia o processo de reload
    public void Reload()
    {
        if (extraAmmo > 0)
        {
            isReloading = true;
            endReloadTime = Time.time + reloadTime;
        }
        else
        {
            isFullEmpty = true;
        }
    }
    // Testa se o reload ja acabou
    private void HandleReloadTime()
    {
        if (Time.time > endReloadTime && isReloading)
        {
            ReloadUpdate();
            isReloading = false;
        }
        else
            return;
    }
    // Atualiza os valores de municao apos o reload
    private void ReloadUpdate()
    {
        if (extraAmmo >= maxLoadedAmmo)
        {   // testa se ha municao suficiente para um pente inteiro
            loadedAmmo = maxLoadedAmmo;
            extraAmmo -= loadedAmmo;
        }
        else if (extraAmmo > 0)
        {   // testa se ha alguma municao no inventario
            loadedAmmo = extraAmmo;
            extraAmmo = 0;
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



