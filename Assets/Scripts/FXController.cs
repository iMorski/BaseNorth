using UnityEngine;

public class FXController : MonoBehaviour
{
    [SerializeField] private GameObject GunFireFX;
    [SerializeField] private Transform GunFireSpawnPosition;
    
    [SerializeField] private GameObject BloodSplatFX;
    [SerializeField] private Transform BloodSplatSpawnPosition;

    public void MakeShoot()
    {
        Instantiate(GunFireFX, GunFireSpawnPosition);
    }

    public void GetShoot()
    {
        Instantiate(BloodSplatFX, BloodSplatSpawnPosition);
    }
}
