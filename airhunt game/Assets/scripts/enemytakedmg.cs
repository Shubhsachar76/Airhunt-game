using UnityEngine;

public class Target : MonoBehaviour
{
    public int health = 50;

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            CannonManager cm = FindObjectOfType<CannonManager>();
            if (cm != null)
                cm.OnBirdKilled();
                
            Destroy(gameObject);
        }
    }
}