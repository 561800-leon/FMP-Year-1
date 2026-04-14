using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;
    public Camera playerCamera;
    public MouseMovement mouseMovement;
    public PlayerMovement playerMovement;

    public float fireRate = 0.1f;
    private float nextFireTime = 0f;

    // Shake settings
    public float shakeAmount = 0.03f;
    public float shakeSpeed = 20f;
    private Vector3 originalLocalPosition;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    void Update()
    {
        bool isFiring = Input.GetKey(KeyCode.Mouse0);

        if (isFiring && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            FireWeapon();
        }

        HandleShake(isFiring);

        if (isFiring)
        {
            mouseMovement.mouseSensitivity = 300f;
            playerMovement.speed = 7.2f;
        }
        else
        {
            mouseMovement.mouseSensitivity = 500f;
            playerMovement.speed = 12f;

        }

    }



    void HandleShake(bool isFiring)
    {
        if (isFiring)
        {
            float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * shakeAmount;
            float y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * shakeAmount;

            transform.localPosition = originalLocalPosition + new Vector3(x, y, 0);
        }
        else
        {
            
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPosition, Time.deltaTime * 10f);
        }
    }

   

    public void FireWeapon()
    {
        if (bulletPrefab == null || bulletSpawn == null || playerCamera == null)
        {
            Debug.LogError("Missing references!");
            return;
        }

        
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f); // far point if nothing hit
        }

        
        Vector3 direction = (targetPoint - bulletSpawn.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * bulletVelocity, ForceMode.Impulse);
        }

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
    }

    public IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}