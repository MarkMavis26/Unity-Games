using UnityEngine;
using TMPro;




public class PlayerHealth : MonoBehaviour
{
    public float health = 100;

    public TextMeshProUGUI healthText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damageAmount)
    {
        if (health > damageAmount)
        {
            //take damage
            health -= damageAmount;

            healthText.text = health.ToString();

            if (health <= 0)
            {
                //dead
            }
        }
        else
        {
            //dead
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Health")
        {
            Destroy(other.gameObject);

            health += 20;

            healthText.text = health.ToString();
        }

    }

}
