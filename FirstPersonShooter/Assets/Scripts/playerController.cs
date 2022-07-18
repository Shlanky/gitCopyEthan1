using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamagable
{
    [Header("Components")]
    [SerializeField] CharacterController controller;

    [Header("----------------------------------")]
    [Header("Player Attributes")]
    [Range(5,20)] [SerializeField] int HP;
    [Range(3, 6)] [SerializeField] float playerSpeed;
    [Range(1.5f, 4)] [SerializeField] float sprintMulti;
    [Range(1, 4)] [SerializeField] int jumps;
    [Range(6, 10)] [SerializeField] float jumpHeight;
    [Range(15, 30)] [SerializeField] float gravityValue;
  
    

    [Header("----------------------------------")]
    [Header("Player Weapon Stats")]
    [Range(0.1f, 3)] [SerializeField] float shootRate;
    [Range(1, 10)] [SerializeField] int weaponDamage;
    [Header("----------------------------------")]
    [Header("Effects")]
    [SerializeField] GameObject hitEffectSpark;
    [SerializeField] GameObject muzzleFlash;

    Vector3 playerVelocity;
    Vector3 move;
    int timesJumped;

    bool isSprinting = false;
    float playerSpeedOrig;
    bool canShoot = true;
    int hpOrig;
    Vector3 playerSpawnPos;

    public Vector3 pushback= Vector3.zero;
    [SerializeField] int pushResolve;
    private void Start()
    {
        

        playerSpeedOrig = playerSpeed;
        hpOrig = HP;
        playerSpawnPos = transform.position;

        
    }

    void Update()
    {
        if (!gamemanager.instance.pause)
        {
            pushback = Vector3.Lerp(pushback, Vector3.zero, Time.deltaTime * pushResolve);
            movePlayer();
            sprint();
            StartCoroutine(shoot());
        }
    }

    private void movePlayer()
    {
        if((controller.collisionFlags & CollisionFlags.Above) !=0)
        {
            playerVelocity.y -=3;
        }
        //if we land reset the player velocity and the jump counter
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            timesJumped = 0;
            playerVelocity.y = 0f;
        }
        // get the input from Unity's input system
        move = (transform.right * Input.GetAxis("Horizontal"))
            + (transform.forward * Input.GetAxis("Vertical"));
        // add our vector to the character controller move
        controller.Move(move * Time.deltaTime * playerSpeed);
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && timesJumped < jumps)
        {
            timesJumped++;
            playerVelocity.y = jumpHeight;
        }
        // add gravity
        playerVelocity.y -= gravityValue * Time.deltaTime;
        // add gravity back into the character controller move
        controller.Move(playerVelocity + pushback * Time.deltaTime);
    }
    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed = playerSpeed * sprintMulti;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed = playerSpeedOrig;
        }
    }

    IEnumerator shoot()
    {
        RaycastHit hit;

        
        // To Help visualize the lazer
        Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);
        if(Input.GetButton("Shoot") && canShoot)
        { 
        canShoot = false;

        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0)), out hit))
        {
                Instantiate(hitEffectSpark, hit.point, hitEffectSpark.transform.rotation);
                if (hit.collider.GetComponent<IDamagable>() != null)
                {
                    IDamagable isDamagable = hit.collider.GetComponent<IDamagable>();
                    if (hit.collider is SphereCollider)
                        isDamagable.takeDamage(10000);
                    else
                    isDamagable.takeDamage(weaponDamage);
                }
        }
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
        yield return new WaitForSeconds(shootRate);
        canShoot = true;
        }
    }
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        updatePlayerHP();
        StartCoroutine(damageFlash());
        if (HP <= 0)
        {
            // kill player
            gamemanager.instance.playerDead();
        }

    }
    IEnumerator damageFlash()
    {
        gamemanager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(.01f);
        gamemanager.instance.playerDamageFlash.SetActive(false);
    }
    public void giveHP(int amount)
    {
        if(HP < hpOrig)
        {
            HP += amount;
            
        }
        if(HP > hpOrig)
        {
            HP = hpOrig;
        }
            gamemanager.instance.HPBar.fillAmount = (float)HP / (float)hpOrig;
    }
    public void updatePlayerHP()
    {
        gamemanager.instance.HPBar.fillAmount = (float)HP / (float)hpOrig;
    }
    public void respawn()
    {
        HP = hpOrig;
        updatePlayerHP();
        controller.enabled = false;
        transform.position = playerSpawnPos;
        controller.enabled = true;
        pushback = Vector3.zero;
    }
}
