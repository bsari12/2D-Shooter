using UnityEditor.Callbacks;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
}
