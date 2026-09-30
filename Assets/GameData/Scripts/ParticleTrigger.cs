using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private void OnTriggerEnter(Collider other)
    {
        IGravityWhippable whippable =
            other.GetComponent<IGravityWhippable>();

        if (whippable != null)
        {
            particle.Play();
        }
    }
}