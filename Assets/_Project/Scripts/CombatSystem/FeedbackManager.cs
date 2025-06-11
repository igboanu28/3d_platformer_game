using UnityEngine;
using System.Collections.Generic;

namespace CombatSystem
{
    public class  FeedbackManager : MonoBehaviour
    {
        public static FeedbackManager Instance { get; private set; }

        [Header("Audio Setting")]
        [Tooltip("Prefab of an AudioSource for playing one-shot sounds. will be instantiated.")]
        [SerializeField] private AudioSource sfxSourcePrefab;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayVFX(GameObject vfxPrefab, Vector3 position, Quaternion rotation = default, Transform parent = null)
        {
            if (vfxPrefab == null)
            {
                Debug.LogWarning("VFX prefab is null. Cannot play VFX.");
                return;
            }
            GameObject vfxInstance = Instantiate(vfxPrefab, position, (rotation == default) ? Quaternion.identity : rotation);
            if (parent != null)
            {
                vfxInstance.transform.SetParent(parent);
            }

            // Auto-destory for particles systems after their duration
            if (vfxInstance.GetComponent<ParticleSystem>() is ParticleSystem ps)
            {
                // Ensure the particle system will auto-destroy itself or handle its lifecycle
                var main = ps.main;
                main.stopAction = ParticleSystemStopAction.Destroy; // Common setup for one-shot effects
                // Destroy(vfxInstance, main.duration + main.startLifetime.constantMax); // Alternative manual destroy
            }
            else
            {
                // Fallback destory for non-particle systems GameObjects
                Destroy(vfxInstance, 5f);
            }
        }

        public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
        {
            if (clip == null)
            {
                Debug.LogWarning("Audio clip is null. Cannot play sound effect.");
                return;
            }
            if (sfxSourcePrefab != null) // Instantiate a dedicated source
            {
                AudioSource sourceInstance = Instantiate(sfxSourcePrefab, position, Quaternion.identity);
                sourceInstance.clip = clip;
                sourceInstance.volume = volume;
                sourceInstance.pitch = pitch;
                sourceInstance.Play();
                Destroy(sourceInstance.gameObject, clip.length / pitch + 0.1f); // Destroy after the clip finishes playing
            }
            else
            {
                AudioSource.PlayClipAtPoint(clip, position, volume);
                // Note: Pitch is not directly settable with PlayClipAtPoint.
            }
        }
    }
}