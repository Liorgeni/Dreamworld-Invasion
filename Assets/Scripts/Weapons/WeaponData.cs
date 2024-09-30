
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]

public class WeaponData : ScriptableObject
{
    public string weaponName;    
    public int damage;            
    public float fireRate;         
    public float range;
    public int ammoCapacity;
    public int numberOfClips;  
    public float reloadTime;       
    public GameObject weaponPrefab;
    //    public ParticleSystem muzzleFlash; 
    public float ammoConsumptionRate;

}
