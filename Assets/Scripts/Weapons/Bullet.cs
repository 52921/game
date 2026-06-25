using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private float range;
    private Vector3 spawnPosition;
    private float distanceTraveled = 0f;
    
    public void Initialize(float bulletDamage, float bulletRange)
    {
        damage = bulletDamage;
        range = bulletRange;
        spawnPosition = transform.position;
    }
    
    private void Update()
    {
        distanceTraveled = Vector3.Distance(transform.position, spawnPosition);
        
        if (distanceTraveled > range)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
