using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Game;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    [Range(0f, 1f)]
    private float sfxVolume;

    [SerializeField]
    [Range(0f, 1f)]
    private float musicVolume;

    [SerializeField]
    private Slider musicVolumeSlider;
    
    private Dictionary<AudioSource, float> originalVolumeDict;

    private AudioSource _musicSource;

    public float GetMusicVolumeMultiplier() => musicVolume;

    private bool _musicIsPlaying = false;

    private void Awake()
    {
        Instance = this;

        originalVolumeDict = new Dictionary<AudioSource, float>();

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicVolumeSlider.onValueChanged.AddListener(val => musicVolume = val);

        EventManager.AddListener<OptionsMenuEvent>(OnOptionsMenuToggled);
    }

    private void OnApplicationFocus(bool focus)
    {
        if( !focus )
        {
            PauseMusic();
        }
        else if(_musicIsPlaying)
        {
            PlayMusic();
        }
    }

    public void PlaySoundEffect(AudioSource audioSource)
    {
        CheckAndRegisterAudioSource(audioSource);

        audioSource.volume = originalVolumeDict[audioSource] * musicVolume;

        audioSource?.Play();
    }

    private void OnOptionsMenuToggled(OptionsMenuEvent evt)
    {
        if( evt.Active )
        {
            PauseMusic();
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayMusic();
        }
    }

    public void PauseMusic()
    {
        _musicSource?.Pause();
    }

    public void PlayMusic(AudioSource audioSource = null)
    {
        if( audioSource != null )
        {
            _musicSource = audioSource;
        }

        if (_musicSource == null )
        {
            Debug.LogError("PlayMusic -- Music Source is NULL");
            return;
        }

        CheckAndRegisterAudioSource(_musicSource);

        if (originalVolumeDict.ContainsKey(_musicSource))
        {
            _musicSource.volume = originalVolumeDict[_musicSource] * musicVolume;
        }
        else
        {
            Debug.LogError($"{_musicSource} isn't registered. Incoming audio source: {audioSource}");
        }

        _musicIsPlaying = true;

        _musicSource?.Play();
    }

    // Track original volume of each source and use that to multiply with player's setting.
    private void CheckAndRegisterAudioSource(AudioSource source)
    {
        if (source != null && originalVolumeDict.ContainsKey(source) == false)
        {
            originalVolumeDict.Add(source, source.volume);
        }
    }
}
