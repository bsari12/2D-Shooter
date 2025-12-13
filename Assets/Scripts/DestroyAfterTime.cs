using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time;
    void Start()
    {
        Invoke("DestroySelf", time);
    }

    // Update is called once per frame
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
