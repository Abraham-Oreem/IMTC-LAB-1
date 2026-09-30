using UnityEngine;

public class RotationEffect : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform target;

    [Header("Rotation")]
    [SerializeField] private float maxRotationSpeed = 180f;
    [SerializeField] private float speedUpTime = 0.3f;
    [SerializeField] private float slowDownTime = 1.5f;
    [SerializeField] private float interactTimeout = 0.1f;

    [Header("Effects")]
    [SerializeField] private AudioSource[] rotationSounds;
    [SerializeField] private ParticleSystem rotationParticles;

    private float currentSpeed;
    private float lastInteractTime = -999f;
    private bool isInteracting;
    private bool isSoundPlaying;
    private float[] originalVolumes;

    private void Awake()
    {
        originalVolumes = new float[rotationSounds.Length];

        for (int i = 0; i < rotationSounds.Length; i++)
        {
            if (rotationSounds[i] != null)
                originalVolumes[i] = rotationSounds[i].volume;
        }
    }

    public bool Interact()
    {
        lastInteractTime = Time.time;
        return false;
    }

    private void Update()
    {
        bool interactingNow = Time.time - lastInteractTime <= interactTimeout;

        if (interactingNow && !isInteracting)
            OnInteractionStarted();
        else if (!interactingNow && isInteracting)
            OnInteractionStopped();

        isInteracting = interactingNow;

        UpdateSpeed();

        if (currentSpeed > 0f)
            target.Rotate(0f, currentSpeed * Time.deltaTime, 0f, Space.World);

        if (isInteracting && rotationParticles != null )
            rotationParticles.Play();

        UpdateSound();
    }

    private void UpdateSpeed()
    {
        float targetSpeed = isInteracting ? maxRotationSpeed : 0f;
        float changeTime = isInteracting ? speedUpTime : slowDownTime;
        float changeRate = maxRotationSpeed / Mathf.Max(changeTime, 0.01f);

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, changeRate * Time.deltaTime);
    }

    private void OnInteractionStarted()
    {
        if (isSoundPlaying)
            return;

        for (int i = 0; i < rotationSounds.Length; i++)
        {
            if (rotationSounds[i] == null)
                continue;

            rotationSounds[i].volume = 0f;
            rotationSounds[i].Play();
        }

        isSoundPlaying = true;
    }

    private void OnInteractionStopped()
    {
        if (rotationParticles == null)
            return;

        rotationParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        rotationParticles.Play();
    }

    private void UpdateSound()
    {
        if (!isSoundPlaying)
            return;

        float speedPercent = currentSpeed / maxRotationSpeed;

        for (int i = 0; i < rotationSounds.Length; i++)
        {
            if (rotationSounds[i] != null)
                rotationSounds[i].volume = originalVolumes[i] * speedPercent;
        }

        if (!isInteracting && currentSpeed <= 0f)
        {
            for (int i = 0; i < rotationSounds.Length; i++)
            {
                if (rotationSounds[i] == null)
                    continue;

                rotationSounds[i].Stop();
                rotationSounds[i].volume = originalVolumes[i];
            }

            isSoundPlaying = false;
        }
    }
}