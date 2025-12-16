using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public bool explosive;
    public GameObject explosionbPrefab;

    void Start()
    {
        Invoke("DestroySelf", 5f);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroySelf();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        DestroySelf();
    }

    void DestroySelf()
    {
        if (explosive)
        {
            Instantiate(explosionbPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
