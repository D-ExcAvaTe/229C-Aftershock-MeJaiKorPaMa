using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource[] sfx;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public void PlaySFX(int sfxIndex)
    {
        sfxSource.PlayOneShot(sfx[sfxIndex].clip);
    }
}
