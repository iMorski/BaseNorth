using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private GameObject GunFireFX;
    [SerializeField] private Transform GunFireSpawn;
    [SerializeField] private AudioClip KnifeStrike;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private AudioClip PistolStrike;
    [SerializeField] private AudioClip MachineStrike;

    public void Fire()
    {
        Instantiate(GunFireFX, GunFireSpawn);
    }

    public void PlayKnifeSound()
    {
        Audio.PlayOneShot(KnifeStrike,0.05f);
    }
    
    public void PlayPistolSound()
    {
        Audio.PlayOneShot(PistolStrike,0.03f);
    }
    
    public void PlayMachineSound()
    {
        Audio.PlayOneShot(MachineStrike,0.02f);
    }
}
