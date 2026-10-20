using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDestroyer : MonoBehaviour
{
    void Start()
    {
        float particleLifetime = GetComponent<ParticleSystem>().main.startLifetime.constantMax;
        Destroy(gameObject, particleLifetime);
    }
}
