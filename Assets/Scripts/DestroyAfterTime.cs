using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time;
    public float timeToDestroyCollider;

    void Start()
    {
        Invoke("DestroySelf", time);
        if(timeToDestroyCollider !=0)
        {
            Invoke("DestroyCollider", timeToDestroyCollider);
        }
    }

    // Update is called once per frame
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    void DestroyCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
