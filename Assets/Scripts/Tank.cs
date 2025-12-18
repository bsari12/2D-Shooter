using UnityEngine;

public class Tank : MonoBehaviour
{
    public float moveSpeed =5f;
    public float rotateSpeed = 200f;

    public Transform turret;
    public float turretTurnSpeed = 5f;
    public Transform rocketSpawnPoint;
    public GameObject rocketPrefab;
    public float fireRate = 1.5f;

    public AudioSource moveAudioSource;
    public float audioFadeSpeed = 2f;
    public AudioClip shootingSFX;

    private bool touchingPlayer;
    private SpriteRenderer spriteRenderer_Body;
    private SpriteRenderer spriteRenderer_Turret;
    private bool inTank;

    private Rigidbody2D rb;
    private Camera cam;

    private float moveInput;
    private float turnInput;

    private float lastShot;


    void Start()
    {
        spriteRenderer_Body = GetComponent<SpriteRenderer>();
        spriteRenderer_Turret = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        if(moveAudioSource != null)
        {
            moveAudioSource.loop = true;
            moveAudioSource.volume = 0f;
            moveAudioSource.Play();
        }
    }

    void Update()
    {
        HandleEnterExit();
        
        if(!inTank) return;

        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        HandleTurretRotation();
        HandleEngineSound();
        HandleShooting();
    }


    void FixedUpdate()
    {
        if(!inTank) return;

        HandleMovement();
    }


    void HandleEnterExit()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(touchingPlayer && !inTank)
            {
                Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.inVehicle = true;
                player.vehicleTransform = transform;
                inTank= true;
            }
            else
            {
                Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.inVehicle = false;
                inTank = false;
            }	
        }

    }

    void HandleMovement()
    {
        if(Mathf.Abs(moveInput) > 0.1f)
        {	
            float rotation = -turnInput * rotateSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + rotation);
        }
        
        Vector2 moveDir = transform.right * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDir);
    }

    void HandleTurretRotation()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = (mouseWorld - turret.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x)* Mathf.Rad2Deg;
        float smoothAngle = Mathf.LerpAngle(turret.eulerAngles.z, angle, Time.deltaTime * turretTurnSpeed);

        turret.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
    }

    void HandleEngineSound()
    {
        bool isMoving = Mathf.Abs(moveInput) > 0.1f || Mathf.Abs(turnInput) > 0.1f;
        float targetVolume = isMoving ? 1f : 0f;

        moveAudioSource.volume = Mathf.Lerp(moveAudioSource.volume, targetVolume, Time.deltaTime * audioFadeSpeed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchingPlayer =true;
            spriteRenderer_Body.color = Color.blue;
            spriteRenderer_Turret.color = Color.blue;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchingPlayer =false;
            spriteRenderer_Body.color = Color.white;
            spriteRenderer_Turret.color = Color.white;
        }
    }

    void HandleShooting()
    {
        if(Time.time> lastShot + fireRate && Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.PlaySFX(shootingSFX,0.2f);
            Object.FindFirstObjectByType<ScreenShake>().Shake(5, 5, 0.25f);
            Instantiate(rocketPrefab, rocketSpawnPoint.position, rocketSpawnPoint.rotation);
            lastShot = Time.time;
        }
    }



}
