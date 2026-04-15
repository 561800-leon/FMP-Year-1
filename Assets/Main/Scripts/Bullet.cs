using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Weapon weapon;


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" + collision.gameObject.name + "!");
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            weapon.bulletVelocity = 5f;
             Destroy(gameObject);   
        }

    }
}
