using UnityEngine;

public class HitBoxTrigger : MonoBehaviour
{
    public int damage = 10;
    public void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
