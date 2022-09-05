using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    //data
    [SerializeField] private UnityDictionary<string, Sound> tracks;

    [Header("Settings")]
    [Tooltip("Time it takes for the last track to be muted")]
    [SerializeField] private float muteTime = 0.5f;
    [Tooltip("Time it takes for the new track to reach normal volume")]
    [SerializeField] private float transitionTime = 1f;

    //vars
    private bool switching;
    //external components
    private AudioSource source;

    //---------------------------setup--------------------------
    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            SetupNonDestroy();
            SetupAudioSource();
        }
    }
    private void SetupNonDestroy()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    private void SetupAudioSource()
    {
        source = GetComponent<AudioSource>();
        source.loop = true;
    }

    //--------------------setup track--------------------
    private void SetupTrack(Sound sound)
    {
        source.clip = sound.clip;
        //settings
        source.pitch = sound.pitch;
        //play
        source.Play();
    }

    //-------------------------manage music track--------------------
    public void SwitchTrack(string name)
    {
        if (!switching) {
            if (tracks.dict.ContainsKey(name)) {
                StartCoroutine(SwitchCo(tracks.dict[name]));
            }
            else { Debug.LogError(transform.name + " contains no sound for " + name); }
        }
    }

    //------------------------switch music track--------------------
    private IEnumerator SwitchCo(Sound nextSound)
    {
        switching = true;
        yield return MuteLastTrack();
        SetupTrack(nextSound);
        yield return UnmuteNextTrack(nextSound);
        source.volume = nextSound.volume;
        switching = false;
    }

    private IEnumerator MuteLastTrack()
    {
        float timer = 0f;
        float startVolume = source.volume;
        while (source.volume > 0f) {
            timer += Time.deltaTime;
            source.volume = startVolume - (timer / (startVolume * muteTime));
            yield return null;
        }
    }

    private IEnumerator UnmuteNextTrack(Sound nextSound)
    {
        float timer = 0f;
        while (source.volume < nextSound.volume) {
            timer += Time.deltaTime;
            source.volume = nextSound.volume * (timer / transitionTime);
            yield return null;
        }
    }
}
