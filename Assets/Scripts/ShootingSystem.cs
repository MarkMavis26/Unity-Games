// using TMPro;
// using UnityEngine;

// public class ShootingSystem : MonoBehaviour
// {
//     public Transform firePoint;
//     public GameObject bulletPrefab;
//     public GameObject muzzleFlash;
//     public TextMeshProUGUI ammoText;
//     public Camera playerCamera;
//     private Camera combatCamera;


//     public AudioClip shootSFX;
//     public AudioClip noammoSFX;
//     public AudioClip ammocollectSFX;
//     public AudioSource audioSource;

//     public float fireRate = 1f;
//     public float bullets = 5;
//     public float bulletVelocity = 20f;
//     private float nextTimeToFire = 0f;

//     void Start()
//     {
//         combatCamera = GetComponentInChildren<Camera>();
//     }

//     void Update()
//     {

//     }

//     public void Shoot()
//     {
//         if (Time.time >= nextTimeToFire)
//         {
//             nextTimeToFire = Time.time + 1f / fireRate;
//             FireBullet();
//         }
//     }

//     private void FireBullet()
//     {
//         // //check if player has enough bullets
//         if (bullets > 0)
//         {
//             GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
//             GameObject muzzle = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
//             Rigidbody bulletRb = bullet.transform.GetChild(0).GetComponent<Rigidbody>();
//                 //reduce the bullet count
//             bullets -= 1;
//             ammoText.text = bullets.ToString();

//             RaycastHit hit;
//             Vector3 linearVelocity;
//             if (Physics.Raycast(combatCamera.transform.position, combatCamera.transform.forward, out hit, Mathf.Infinity))
//             {
//                 //LineRenderer lr = bullet.AddComponent<LineRenderer>();
//                 //lr.positionCount = 2;
//                 //lr.SetPosition(0, combatCamera.transform.position);
//                 //lr.SetPosition(1, combatCamera.transform.position + combatCamera.transform.forward * hit.distance);
//                 //Debug.DrawRay(combatCamera.transform.position, combatCamera.transform.forward * hit.distance, Color.yellow);

//                 Debug.Log("Did Hit: " + combatCamera.transform.position + " - " + (combatCamera.transform.forward * hit.distance) + " - " + hit.distance);
//                 linearVelocity = (hit.point - firePoint.position).normalized * bulletVelocity;
//             }
//             else 
//             {
//                 Debug.DrawRay(combatCamera.transform.position, combatCamera.transform.forward * 1000, Color.white);
//                 Debug.Log("Did not Hit");
//                 linearVelocity = transform.forward * bulletVelocity;
//             }

//             if (bulletRb != null)
//             {
//                 Debug.Log("Bullet direction: " + linearVelocity + ", from: " + firePoint.position);

//                 bulletRb.AddForce(linearVelocity, ForceMode.VelocityChange);
//             }

//             audioSource.clip = shootSFX;
//             audioSource.Play();

//             Destroy(bullet, 10f);
//             Destroy(muzzle, 2f);
//         }
//         else
//         {
//             audioSource.clip = noammoSFX;
//             audioSource.Play();
//         }
//     }
//     //this gets called when you walk into any trigger in your game
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.tag == "Ammo")
//         {
//             //Destroy the ammo pickup
//             Destroy(other.gameObject);
//             audioSource.clip = ammocollectSFX;
//             audioSource.Play();
//             //add 5 more bullets
//             bullets += 20;
//             //another way of writing this: bullets = bullets + 5;
//             ammoText.text = bullets.ToString();
//         }
//     }
// }
using TMPro;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoText;
    public float fireRate = 1f;
    private float nextTimeToFire = 0f;
    public float bullets = 5;
    public float bulletVelocity = 10f;

    public AudioClip shootSFX;
    AudioSource audioSource;
    private Camera combatCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        combatCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if(Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if(bullets > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            GameObject muzzle = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.transform.GetChild(0).GetComponent<Rigidbody>();

            bullets--;
            ammoText.text = bullets.ToString();

            RaycastHit hit;
            Vector3 linearVelocity;
            if (Physics.Raycast(combatCamera.transform.position, combatCamera.transform.forward, out hit, Mathf.Infinity))
            {
                /*
                LineRenderer lr = bullet.AddComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.SetPosition(0, combatCamera.transform.position);
                lr.SetPosition(1, combatCamera.transform.position + combatCamera.transform.forward * hit.distance);
                */
                //Debug.DrawRay(combatCamera.transform.position, combatCamera.transform.forward * hit.distance, Color.yellow);
                Debug.Log("Did Hit: " + combatCamera.transform.position + " - " + (combatCamera.transform.forward * hit.distance) + " - " + hit.distance);
                linearVelocity = (hit.point - firePoint.position).normalized * bulletVelocity;
            }
            else
            {
                Debug.DrawRay(combatCamera.transform.position, combatCamera.transform.forward * 1000, Color.white);
                Debug.Log("Did not Hit");
                linearVelocity = transform.forward * bulletVelocity;
            }

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = linearVelocity;
            }

            /*
            LineRenderer lr = bullet.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, combatCamera.transform.position);
            lr.SetPosition(1, firePoint.position + 100*bulletRb.linearVelocity);
            */

            audioSource.Stop();
            audioSource.clip = shootSFX;
            audioSource.time = 0.8f;
            audioSource.Play();

            Destroy(bullet, 10f);
            Destroy(muzzle, 2f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ammo")
        {
            Destroy(other.gameObject);

            bullets += 20;
            ammoText.text = bullets.ToString();
        }
    }
}