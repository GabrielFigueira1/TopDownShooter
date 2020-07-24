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

    [SerializeField] private WeaponChangeHandler weaponChange;
    [SerializeField] private RectTransform weaponIcons;

    private float reloadTime;
    private float reloadStartTime;
    // Start is called before the first frame update
    void Update()
    {
        UpdateRealoadBar();
    }
    void Start()
    {

    }
    // Atualiza os valor da municao
    public void UpdateAmmoUI(int loaded, int extra)
    {
        loadedAmmo.text = loaded.ToString();
        extraAmmo.text = extra.ToString();
    }
    // Atualiza o filling da barra de reload
    private void UpdateRealoadBar()
    {
        reloadBar.fillAmount = Mathf.Clamp01(Mathf.Abs((reloadStartTime - Time.time) / reloadTime));
    }
    // Atualiza os valores para os calculos da barra de reload
    public void ReloadBar(bool isReloading, float reloadTime, float reloadStartTime)
    {
        this.reloadTime = reloadTime;
        this.reloadStartTime = reloadStartTime;

        if (isReloading)
        {
            reloadBar.gameObject.SetActive(true);
            UpdateRealoadBar();
        }
        else
        {
            reloadBar.gameObject.SetActive(false);
        }

    }
    public void UpdateWeaponIcon()
    {
        int i = 0;
        foreach (RectTransform icon in weaponIcons)
        {
            if (i == weaponChange.selectedWeapon)
            {
                icon.gameObject.SetActive(true);
            }
            else
            {
                icon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
