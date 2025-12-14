using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public WeaponSO weaponHeld;
    public bool weaponEquipped;
    public bool reloading;
    public int currentAmmo;

    public float moveSpeed;
    public float health =100f;
    public GameObject deadPlayerPrefab;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public GameObject armObj;
    public SpriteRenderer heldGunSpriteRenderer;
    public GameObject muzzleFlash;

    public AudioClip hitSFX;
    public AudioClip footStepSFX;
    public AudioClip reloadSFX;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveVelocity;
    private float nextFireTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer =GetComponent<SpriteRenderer>();
        StartCoroutine(PlayFootsteps());
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleWeaponEquip();
        HandleReloading();
        HandleDropping();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveVelocity;
        HandleRotation();
    }


    void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (weaponHeld && weaponEquipped)
        {
            moveVelocity = new Vector2(moveX, moveY).normalized * moveSpeed * weaponHeld.moveSpeed;
        }
        else
        {
            moveVelocity = new Vector2(moveX,moveY).normalized*moveSpeed;
        }
        
    }

    void HandleRotation()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - transform.position);
        direction.Normalize();

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = targetAngle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Damage")
        {
            health -=10;
            AudioManager.instance.PlaySFX(hitSFX);
            StartCoroutine(BlinkRed());

            if (health <= 0)
            {
                Die();
            }
        }
    }
    void Die()
    {
        if(!this.enabled) return;
        spriteRenderer.enabled = false;
        armObj.SetActive(false);
        rb.linearVelocity =Vector2.zero;
        Instantiate(deadPlayerPrefab, transform.position, transform.rotation);
        this.enabled = false;
    }
    IEnumerator BlinkRed()
    {
        spriteRenderer.color =Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (moveVelocity.magnitude > 0.1f)
            {
                AudioManager.instance.PlaySFX(footStepSFX, 0.5f);
            }
            if (health <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.35f);
        }
    }
    void HandleShooting()
    {
        if (Input.GetMouseButton(0) && weaponEquipped)
        {
            if(Time.time >= nextFireTime && currentAmmo > 0)
            {
                nextFireTime = Time.time +weaponHeld.fireRate;

                Quaternion baseRotation =transform.rotation;
                float spreadAngle = Random.Range(-weaponHeld.bloom*20f, weaponHeld.bloom*20f);
                Quaternion bulletRotation = baseRotation*Quaternion.Euler(0,0,spreadAngle);

                AudioManager.instance.PlaySFX(weaponHeld.shootingSound,0.2f);
                Object.FindFirstObjectByType<ScreenShake>().Shake(weaponHeld.screenShakeIntensity,weaponHeld.screenShakeIntensity,0.1f);

                Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);
                Transform muzzleFlashTransform = Instantiate(muzzleFlash, bulletSpawnPoint.position, bulletRotation, bulletSpawnPoint).transform;

                if(weaponHeld.name == "Ak-47")
                {
                    muzzleFlashTransform.localPosition = new Vector2(1f,muzzleFlashTransform.localPosition.y);
                }
                else
                {
                    muzzleFlashTransform.localPosition = new Vector2(0.1f,muzzleFlashTransform.localPosition.y);
                }

                currentAmmo--;
            }
            
        }
    }

    void HandleWeaponEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponEquipped = !weaponEquipped;
            if(weaponEquipped && weaponHeld != null)
            {
                armObj.SetActive(true);
                heldGunSpriteRenderer.sprite =weaponHeld.gunTopDownViewSprite;
            }
            else
            {
                armObj.SetActive(false);
                weaponEquipped = false;
            }
        }
    }
    void HandleReloading()
    {
        if(weaponEquipped && Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            if(currentAmmo < weaponHeld.magSize)
            {
                StartCoroutine(ReloadRoutine());
            }
        }
    }

    IEnumerator ReloadRoutine() 
    {
        reloading = true;

        Animator armsAnim = armObj.GetComponent<Animator>();
        armsAnim.Play("Arms_Reloading");
        AudioManager.instance.PlaySFX(reloadSFX,0.35f);
        yield return new WaitForSeconds(0.8f);

        currentAmmo = weaponHeld.magSize;
        armsAnim.Play("Arms_NotReloading");
        reloading = false;

    }
    public void PickUpWeapon(WeaponSO weaponSO)
    {
        DropGun();
        weaponHeld = weaponSO;
        currentAmmo = weaponHeld.magSize;
        heldGunSpriteRenderer.sprite =weaponHeld.gunTopDownViewSprite;
    }
    void DropGun()
    {
        if(weaponHeld != null)
        {
            Instantiate(weaponHeld.groundWeaponPrefab, transform.position, transform.rotation);
            weaponHeld = null;
            armObj.SetActive(false);
            weaponEquipped = false;

        }
    }
    void HandleDropping()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropGun();
        }
    }
}
