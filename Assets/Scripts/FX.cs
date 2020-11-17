using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private GameObject GunFireFX;
    [SerializeField] private Transform GunFireSpawnPosition;

    public void Fire()
    {
        Instantiate(GunFireFX, GunFireSpawnPosition);
    }
}
