using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    // singleton
    public static SoundFXManager instance;

    [SerializeField] private AudioSource[] jumpSounds;
    [SerializeField] private AudioSource soundFXObject;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable() => MovementHandler.OnJump += playJumpSound;

    private void OnDisable() => MovementHandler.OnJump -= playJumpSound;

    // function to play clips
    private void playJumpSound()
    {
        AudioSource jumpSound = jumpSounds[Random.Range(0, jumpSounds.Length)];
        float rand = Random.Range(.5f, .6f);
        playAudio(jumpSound.clip, this.transform, rand);
    }

    private void playAudio(AudioClip clip, Transform spawnTransform, float volume)
    {
        AudioSource source = Instantiate(soundFXObject, spawnTransform);
        source.clip = clip;
        Debug.Log("Playing sound: " + source.name);
        source.volume = 1f;
        source.Play();
        float clipLength = source.clip.length;
        Destroy(source, clipLength);
    }
}
