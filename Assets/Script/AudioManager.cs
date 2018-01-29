using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    AudioSource sfx2DSource; 
    AudioSource musicSources; 

    int activeMusicSourceIndex; 

    public static AudioManager instance; 

    SoundLibrary library; 

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        library = GetComponent<SoundLibrary>();
        sfx2DSource = GetComponent<AudioSource>();
    } 

    public void PlaySound(AudioClip clip, Vector3 pos) //function to play sfx
    {
        if (clip != null)//if is playing a clip
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound2D(string soundName) 
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName));
    }

    public void PlaySound(string soundName, Vector3 pos) 
    {
        PlaySound(library.GetClipFromName(soundName), pos); 
    }
}