using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public WeaponData currentWeapon;  // Reference to the ScriptableObject
    public ParticleSystem m16Muzzle;  // M16 Muzzle Flash
    public ParticleSystem pistolMuzzle;  // Pistol Muzzle Flash
    public ParticleSystem flameMuzzle;  // Flamethrower Flame Particle System

    public int currentAmmo;
    public int currentClips;
    private bool isReloading = false;
    private bool isFiring = false; // For the flamethrower
    private bool canShoot = true;
    private float ammoAccumulator = 0f; // To accumulate ammo consumption over time
    private float damageCooldown = 0f; // Cooldown to throttle damage application
    public float damageInterval = 0.5f; // Interval in seconds between damage ticks
    void Start()
    {
        InitializeWeapon();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (currentWeapon.weaponName == "Flamethrower")
        {
            HandleFlamethrowerInput(); // Special input handling for the flamethrower
        }
        else
        {
            HandleStandardWeaponInput();
        }

        // Reload if needed
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentClips > 0 || currentAmmo <= 0 && !isReloading && currentClips > 0)
        {
            StartCoroutine(Reload());
        }
    }
    public void InitializeWeapon()
    {
        // Ensure the weapon data (like ammo, clips, etc.) is initialized properly
        currentAmmo = currentWeapon.ammoCapacity;
        currentClips = currentWeapon.numberOfClips;

        Debug.Log("Weapon initialized: " + currentWeapon.weaponName);
    }
    // Flamethrower continuous firing logic
    void HandleFlamethrowerInput()
    {
        if (Input.GetButton("Fire1") && currentAmmo > 0 && !isReloading)
        {
            StartFiringFlamethrower();
        }
        else
        {
            StopFiringFlamethrower();
        }
    }

    // Regular weapon logic
    void HandleStandardWeaponInput()
    {
        if (Input.GetButton("Fire1") && currentAmmo > 0 && !isReloading && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        PlayCurrentWeaponMuzzleFlash(); // Play muzzle flash for standard weapons

        // Raycasting to detect hit
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, currentWeapon.range))
        {
            // Apply damage if hit enemy
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(currentWeapon.damage);
            }



            ExplosiveBarrel barrel = hit.transform.GetComponent<ExplosiveBarrel>();
            if (barrel != null)
            {
                // Deal damage to the barrel
                barrel.TakeDamage(currentWeapon.damage);
            }
        }



        StartCoroutine(HandleFireRate());
    }

    // Start continuous flamethrower firing
    void StartFiringFlamethrower()
    {
        if (!isFiring)
        {
            isFiring = true;
            flameMuzzle.Play(); // Start the flame particle system
        }

        // Accumulate ammo consumption based on the consumption rate and deltaTime
        ammoAccumulator += currentWeapon.ammoConsumptionRate * Time.deltaTime;

        // Consume ammo once the accumulated value reaches or exceeds 1
        int ammoToConsume = Mathf.FloorToInt(ammoAccumulator);

        // Subtract ammo if we have accumulated enough
        if (ammoToConsume > 0)
        {
            currentAmmo -= ammoToConsume;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, currentWeapon.ammoCapacity);
            ammoAccumulator -= ammoToConsume; // Deduct the consumed ammo from the accumulator
        }

        // Debugging
        Debug.Log("Current Ammo: " + currentAmmo + ", Ammo Consumed: " + ammoToConsume);

        ApplyFlameDamage();
    }
    // Stop continuous flamethrower firing
    void StopFiringFlamethrower()
    {
        if (isFiring)
        {
            isFiring = false;
            flameMuzzle.Stop(); // Stop the flame particle system
        }
    }

    // Apply damage in a cone shape for the flamethrower
    void ApplyFlameDamage()
    {
        RaycastHit hit;

        // Update the cooldown timer
        damageCooldown -= Time.deltaTime;

        // Perform a raycast in the forward direction with the flamethrower's range
        if (Physics.Raycast(transform.position, transform.forward, out hit, currentWeapon.range))
        {
            // Check if the hit object has an EnemyHealth component (for enemies)
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth != null && damageCooldown <= 0f) // Apply damage only if cooldown is over
            {
                // Deal damage and reset the cooldown timer
                enemyHealth.TakeDamage(Mathf.FloorToInt(currentWeapon.damage));
                damageCooldown = damageInterval; // Reset cooldown
                Debug.Log("Damaged enemy: " + hit.transform.name);
            }

            // Check if the hit object is an ExplosiveBarrel (for barrels)
            ExplosiveBarrel barrel = hit.transform.GetComponent<ExplosiveBarrel>();
            if (barrel != null && damageCooldown <= 0f) // Apply damage only if cooldown is over
            {
                // Deal damage to the barrel (reduce health) and reset the cooldown timer
                barrel.TakeDamage(Mathf.FloorToInt(currentWeapon.damage)); // Apply the damage
                damageCooldown = damageInterval; // Reset cooldown
                Debug.Log("Damaged barrel: " + hit.transform.name);
            }
        }
    }
    //void ApplyFlameDamage()
    //{
    //    RaycastHit hit;

    //    // Perform a raycast in the forward direction with the flamethrower's range
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, currentWeapon.range))
    //    {
    //        // Check if the hit object has an EnemyHealth component (for damageable objects like enemies)
    //        EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
    //        if (enemyHealth != null)
    //        {
    //            // Deal damage over time based on the flamethrower's damage stat
    //            enemyHealth.TakeDamage(Mathf.FloorToInt(currentWeapon.damage * Time.deltaTime));
    //        }

    //        // Handle other objects, e.g., explosive barrels or destructible objects
    //        ExplosiveBarrel barrel = hit.transform.GetComponent<ExplosiveBarrel>();
    //        if (barrel != null)
    //        {
    //            barrel.Explode(); // Trigger the explosion
    //        }

    //        // Log the hit object for debugging purposes
    //        Debug.Log("Flamethrower hit: " + hit.transform.name);
    //    }
    //}

    IEnumerator HandleFireRate()
    {
        canShoot = false;
        yield return new WaitForSeconds(currentWeapon.fireRate);
        canShoot = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentAmmo = currentWeapon.ammoCapacity;
        currentClips--;
        isReloading = false;
    }

    void PlayCurrentWeaponMuzzleFlash()
    {
        switch (currentWeapon.weaponName)
        {
            case "Rifle":
                PlayWeaponPS(m16Muzzle);
                break;
            case "Pistol":
                PlayWeaponPS(pistolMuzzle);
                break;
            case "Flamethrower":
                PlayWeaponPS(flameMuzzle);
                break;
            default:
                Debug.LogWarning("No muzzle flash assigned for this weapon.");
                break;
        }
    }

    void PlayWeaponPS(ParticleSystem muzzle)
    {
        if (muzzle != null)
        {
            muzzle.Play();
        }
    }
}
