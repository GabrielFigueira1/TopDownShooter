using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadedAmmo;
    [SerializeField] private TextMeshProUGUI extraAmmo;
    // Start is called before the first frame update
    void Start()
    {
        loadedAmmo.text = "0";
        extraAmmo.text = "0";
    }

    // Update is called once per frame
    public void updateAmmoUI(int loaded, int extra){
        loadedAmmo.text = loaded.ToString();
        extraAmmo.text = extra.ToString();
    }
}
