using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 50f;
    public WeaponSO weaponHeld;
    public float bloom;
    public float fireRate;


    public GameObject deadEnemyPrefab;
    public AudioClip hitSFX;
    public SpriteRenderer heldGunSpriteRenderer;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;    

    public float sightDistance=5f;
    public LayerMask playerLayer;
    private float nextFireTime = 0f;

    private Transform player;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;

    private SpriteRenderer spriteRenderer;
    private List<Transform> patrolPoints = new List<Transform>();

    private enum EnemyState {Patrolling, Chasing}
    private EnemyState currentState = EnemyState.Patrolling;

    void Awake()
    {
        spriteRenderer =GetComponent<SpriteRenderer>();
        GameObject[] patrolObject = GameObject.FindGameObjectsWithTag("Patrol_Point");

        foreach(GameObject obj in patrolObject)
        {
            patrolPoints.Add(obj.transform);
        }
        heldGunSpriteRenderer.sprite = weaponHeld.gunTopDownViewSprite;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                LookForPlayer();
                break;

            case EnemyState.Chasing:
                ChasePlayer();
                break;
        }
        RotateTowardsMovement();
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

        Instantiate(deadEnemyPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    IEnumerator BlinkRed()
    {
        spriteRenderer.color =Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    private void Patrol()
    {
        if(patrolPoints.Count == 0)return;
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex +1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
    private void LookForPlayer() 
    {
        Vector2 direction = (player.position - transform.position);
        float distance = direction.magnitude;
        if(distance <= sightDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position,direction.normalized,distance,playerLayer);
            if(hit.collider !=null && hit.collider.CompareTag("Player"))
            {
                currentState = EnemyState.Chasing;
            }
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = player.position -transform.position;
        float distance =direction.magnitude;
        if(distance> 5f)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
            Shoot();
        }
        if (distance > sightDistance)
        {
            currentState = EnemyState.Patrolling;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    private void RotateTowardsMovement()
    {
        if(currentState == EnemyState.Chasing && player != null)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0,angle);
        }
        else
        {
            Vector3 velocity = agent.velocity;
            if(velocity.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x)*Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0,angle);
            }
        }


    }
    void Shoot()
    {
        if(Time.time<nextFireTime || weaponHeld == null) return;

        nextFireTime = Time.time + fireRate;

        float spreadAngle = Random.Range(-bloom*20f, weaponHeld.bloom*20f);
        Quaternion bulletRotation = transform.rotation*Quaternion.Euler(0,0,spreadAngle);

        AudioManager.instance.PlaySFX(weaponHeld.shootingSound,0.2f);

        Instantiate(bulletPrefab, heldGunSpriteRenderer.transform.position, bulletRotation);

        Transform flash = Instantiate( muzzleFlash,heldGunSpriteRenderer.transform.position, bulletRotation,heldGunSpriteRenderer.transform).transform;

        if(weaponHeld.name == "Ak-47")
        {
            flash.localPosition = new Vector2(1f,flash.localPosition.y);
        }
        else
        {
            flash.localPosition = new Vector2(0.1f,flash.localPosition.y);
        }
    }
}
