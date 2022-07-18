using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHit : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] Rigidbody rb;
    [Range (0.01f,10)][SerializeField] float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = (gamemanager.instance.player.transform.position - transform.position).normalized * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null)
        {
            IDamagable isDamagable = other.GetComponent<IDamagable>();
            isDamagable.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
