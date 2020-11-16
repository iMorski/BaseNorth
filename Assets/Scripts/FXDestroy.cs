using UnityEngine;

public class FXDestroy : MonoBehaviour
{
    private ParticleSystem Particle;

    public void Awake() 
    {
        Particle = GetComponent<ParticleSystem>();
    }
 
    public void Update() 
    {
        if (Particle && !Particle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
