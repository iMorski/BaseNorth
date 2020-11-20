using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private GameObject GunFireFX;
    [SerializeField] private Transform GunFireSpawn;

    public void Fire()
    {
        Instantiate(GunFireFX, GunFireSpawn);
    }
}
