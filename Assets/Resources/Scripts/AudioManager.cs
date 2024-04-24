using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource audio_Source;

    public AudioClip buttonClickSound;
    public AudioClip bucketSound;
    public AudioClip stampSound;


    public void PlayButtonClickSound()
    {
        audio_Source.PlayOneShot(buttonClickSound, 0.4f);
    }
    public void PlayBucketSound()
    {
        audio_Source.PlayOneShot(bucketSound, 0.4f);
    }
    public void PlayStampSound()
    {
        audio_Source.PlayOneShot(stampSound, 0.4f);
    }
}
