using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    //data
    [SerializeField] private UnityDictionary<string, Sound> sounds;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            InitializeSounds();
        }
    }

    private void InitializeSounds()
    {
        foreach (Sound s in sounds.dict.Values) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            //settings
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    //----------------play sound fx------------------
    public void Play(string name)
    {
        if (sounds.dict.ContainsKey(name)) {
            sounds.dict[name].source.Play();
        }
        //debug info
        else { Debug.LogError(transform.name + " contains no sound for " + name); }
    }
}
