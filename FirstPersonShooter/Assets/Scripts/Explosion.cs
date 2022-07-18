using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] int damage;
    [SerializeField] int pushBackAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            gamemanager.instance.playerScript.pushback = (gamemanager.instance.playerScript.transform.position - transform.position)*damage;
        if (other.GetComponent<IDamagable>() != null)
        {
            IDamagable isDamagable = other.GetComponent<IDamagable>();
            isDamagable.takeDamage(damage);
        }
    }
}
