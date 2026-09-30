using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        IGravityWhippable whippable =
            other.GetComponent<IGravityWhippable>();

        if (whippable != null)
        {
            particle.Play();
            audioSource.Play();
        }
    }
}