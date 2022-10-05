using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [SerializeField] private Text ammoText;

    public void UpdateAmmo(int count)
    {
        ammoText.text = "Ammo: " + count;
    }

}
