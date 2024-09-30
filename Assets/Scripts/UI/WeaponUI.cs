using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponUI : MonoBehaviour
{
    public WeaponController weaponController; // Reference to the WeaponController
    public TextMeshProUGUI ammoText; // Reference to the UI Text element for displaying ammo

    void Update()
    {
        // Check if weaponController is assigned
        if (weaponController != null)
        {
            UpdateAmmoUI();
        }
    }

    // Function to update the UI Text based on the current weapon's stats
    private void UpdateAmmoUI()
    {
        if (ammoText != null && weaponController.currentWeapon != null)
        {
            ammoText.text = weaponController.currentWeapon.weaponName + "  |   " + weaponController.currentClips + "\n" + weaponController.currentAmmo + " / " + weaponController.currentWeapon.ammoCapacity;
                           
        }
    }
}
