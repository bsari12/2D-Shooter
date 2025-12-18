using UnityEditor.Callbacks;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float throwForce = 5f;
    public float detonateTime = 2f;
    public GameObject explosionPrefab;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right*throwForce);
        Invoke("Detonate", detonateTime);

    }
    private void Detonate()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
