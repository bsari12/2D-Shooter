using Unity.Mathematics;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public AudioClip beepSFX;
    public GameObject explosionPrefab;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player" || collision.gameObject.tag == "Enemy")
        {
            AudioManager.instance.PlaySFX(beepSFX);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Explosion" || collision.gameObject.tag == "Damage")
        {
            Instantiate(explosionPrefab,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
