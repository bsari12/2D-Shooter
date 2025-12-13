using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public Sprite gunTopDownViewSprite;
    public GameObject groundWeaponPrefab;
    public AudioClip shootingSound;
    [Range(0f,1f)]
    public float moveSpeed;
    [Range(0f,1f)]
    public float bloom;
    public float fireRate;
    public int magSize;
    public float screenShakeIntensity;

}
