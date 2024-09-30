using UnityEngine;

[System.Serializable]
public class WeaponState
{
    public int currentAmmo;
    public int currentClips;

    public WeaponState(int ammo, int clips)
    {
        currentAmmo = ammo;
        currentClips = clips;
    }
}
