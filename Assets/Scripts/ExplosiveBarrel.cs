using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject explosionPrefab;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Explosion" || collision.gameObject.tag == "Damage")
        {
            Instantiate(explosionPrefab,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
