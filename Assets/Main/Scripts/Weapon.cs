using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;

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
            // Smoothly return to original position
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPosition, Time.deltaTime * 10f);
        }
    }

    public void FireWeapon()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(bulletSpawn.forward * bulletVelocity, ForceMode.Impulse);
        }

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
    }

    public IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}