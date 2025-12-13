using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
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
        Destroy(gameObject);
    }
}
