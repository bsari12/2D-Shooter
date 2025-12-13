using UnityEngine;

public class PickUp : MonoBehaviour
{
    public WeaponSO weaponSO;
    private bool touchingPlayer;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && touchingPlayer)
        {
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            player.PickUpWeapon(weaponSO);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchingPlayer = true;
            spriteRenderer.color =Color.blue;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchingPlayer = false;
            spriteRenderer.color =Color.white;
        }
    }
}
