using KBCore.Refs;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;
    // i can easily add more here later, like attackClip, footstepClip, etc.

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHitSound()
    {
        if (hitClip != null)
        {
            audioSource.PlayOneShot(hitClip);
        }
    }

    public void PlayDeathSound()
    {
        if (deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }
    }
}