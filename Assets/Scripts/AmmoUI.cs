using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private GunStats gun; // Temporario
    [SerializeField] private TextMeshProUGUI loadedAmmo;
    [SerializeField] private TextMeshProUGUI extraAmmo;
    // Start is called before the first frame update
    void Start()
    {
        loadedAmmo.text = gun.loadedAmmo.ToString();
        extraAmmo.text = gun.extraAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        loadedAmmo.text = gun.loadedAmmo.ToString();
        extraAmmo.text = gun.extraAmmo.ToString();
    }
}
