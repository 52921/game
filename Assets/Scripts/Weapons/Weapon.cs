using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [System.Serializable]
    public class WeaponStats
    {
        public string weaponName;
        public float damage = 25f;
        public float fireRate = 0.1f;
        public int magCapacity = 30;
        public float reloadTime = 2f;
        public float bulletSpeed = 100f;
        public float range = 1000f;
    }
    
    [SerializeField] private WeaponStats stats = new WeaponStats();
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource fireSound;
    [SerializeField] private AudioSource reloadSound;
    
    private int currentAmmo;
    private bool isReloading = false;
    private float lastFireTime = 0f;
    private Animator animator;
    
    private void Start()
    {
        currentAmmo = stats.magCapacity;
        animator = GetComponent<Animator>();
    }
    
    public void Fire()
    {
        if (isReloading || currentAmmo <= 0)
            return;
        
        if (Time.time - lastFireTime < stats.fireRate)
            return;
        
        lastFireTime = Time.time;
        currentAmmo--;
        
        // Fire animation
        if (animator != null)
            animator.SetTrigger("Fire");
        
        // Muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();
        
        // Fire sound
        if (fireSound != null)
            fireSound.PlayOneShot(fireSound.clip);
        
        // Spawn bullet
        SpawnBullet();
    }
    
    private void SpawnBullet()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
            return;
        
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * stats.bulletSpeed;
        }
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(stats.damage, stats.range);
        }
    }
    
    public void Reload()
    {
        if (isReloading || currentAmmo == stats.magCapacity)
            return;
        
        StartCoroutine(ReloadCoroutine());
    }
    
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        
        // Reload animation
        if (animator != null)
            animator.SetTrigger("Reload");
        
        // Reload sound
        if (reloadSound != null)
            reloadSound.PlayOneShot(reloadSound.clip);
        
        yield return new WaitForSeconds(stats.reloadTime);
        
        currentAmmo = stats.magCapacity;
        isReloading = false;
    }
    
    public int GetCurrentAmmo() => currentAmmo;
    public int GetMagCapacity() => stats.magCapacity;
    public string GetWeaponName() => stats.weaponName;
    public float GetDamage() => stats.damage;
    public bool IsReloading() => isReloading;
}
