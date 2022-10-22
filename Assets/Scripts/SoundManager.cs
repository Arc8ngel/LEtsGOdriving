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

    private bool _isInitialized = false;

    private void Awake()
    {
        Instance = this;

        originalVolumeDict = new Dictionary<AudioSource, float>();

        if( !_isInitialized )
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        }

        musicVolumeSlider.onValueChanged.AddListener(UpdateVolume);

        EventManager.AddListener<OptionsMenuEvent>(OnOptionsMenuToggled);

        _isInitialized = true;
    }

    private void UpdateVolume(float volume)
    {
        if( _musicSource != null )
        {
            if( originalVolumeDict != null && originalVolumeDict.ContainsKey(_musicSource) )
            {
                musicVolume = volume;
                _musicSource.volume = originalVolumeDict[_musicSource] * musicVolume;
            }
        }
    }

    private void OnDestroy()
    {
        musicVolumeSlider.onValueChanged.RemoveListener(UpdateVolume);
    }

    /*private void OnApplicationFocus(bool focus)
    {
        if( !Application.isPlaying )
            return;
        
        if( !focus )
        {
            PauseMusic(true);
        }
        else if(_musicIsPlaying)
        {
            PauseMusic(false);
        }
    }*/

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
            //PauseMusic();
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PauseMusic(false);
        }
    }

    public void PauseMusic(bool pause)
    {
        if( pause )
        {
            _musicSource?.Pause();
        }
        else
        {
            _musicSource?.UnPause();
        }
    }

    public void PlayMusic(AudioSource audioSource = null)
    {
        if( _musicSource == null && audioSource != null )
        {
            _musicSource = audioSource;
        }

        if (_musicSource == null )
        {
            Debug.LogError("PlayMusic -- Music Source is NULL");
            return;
        }

        CheckAndRegisterAudioSource(_musicSource);

        if( _musicIsPlaying )
        {
            _musicSource?.UnPause();
        }
        else
        {
            _musicIsPlaying = true;
            _musicSource?.Play();
        }
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
