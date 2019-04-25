using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource audioSource;

    private float musicVolume = 1f;

    private bool musicMute = true;

    public Sound[] sounds;

    // Use this for initialization
    public static AudioManager soundinstance;

    private bool check = true;

    void Awake()
    {
        AudioListener.volume = 1;

        if (PlayerPrefs.HasKey("VolumeMute"))
        {
            if (PlayerPrefs.GetInt("VolumeMute") == 0)
            {
                AudioListener.pause = true;
            }
            else AudioListener.pause = false;
        }
        else
            AudioListener.pause = false;




        if (soundinstance == null)
        {
            soundinstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Play("TileMusic");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        s.source.Play();
    }
    public void Stop(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        s.source.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    
    public void SetMute(bool mute)
    {
        int value;

        if (mute)
        {
            AudioListener.pause = true;    
            value = 1;
        }
        else
        {
            AudioListener.pause = false;
            value = 0;
        }

        PlayerPrefs.SetInt("VolumeMute", value);
    }
}
