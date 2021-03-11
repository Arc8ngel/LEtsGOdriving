using System;
using System.Collections;
using System.Collections.Generic;
using Unity.LEGO.Game;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField]
    private KartGame.KartSystems.BaseInput inputSystem;

    [SerializeField]
    private Text[] timerDisplay_text;

    [SerializeField]
    private float m_IntroLockoutTime = 3.0f;

    [SerializeField]
    private AudioSource bgmMusicSource;

    [SerializeField]
    private AudioSource driverAudio;

    [SerializeField]
    private float bgmMusicStartDelay = 2f;

    private float m_timeElapsed;
    private bool m_isTimeCounting = false;

    private bool m_gameIsOver = false;

    private void Awake()
    {
        if( inputSystem == null )
        {
            inputSystem = FindObjectOfType<KartGame.KartSystems.BaseInput>();
        }

        EventManager.AddListener<GameStartEvent>(OnGameStart);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<TimeStopEvent>(OnTimeStop);
        EventManager.AddListener<OptionsMenuEvent>(OnMenuToggled);
    }

    private void OnDestroy()
    {
        if (bgmMusicSource != null )
        {
            bgmMusicSource.Stop();
        }

        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<TimeStopEvent>(OnTimeStop);
        EventManager.RemoveListener<OptionsMenuEvent>(OnMenuToggled);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inputSystem != null )
        {
            inputSystem.InputAllowed = false;
        }
    }

    

    public float GetTimeElapsed() => m_timeElapsed;

    private IEnumerator BGMAudioInit()
    {
        yield return new WaitForSeconds(bgmMusicStartDelay);

        if( bgmMusicSource != null )
        {
            SoundManager.Instance.PlayMusic(bgmMusicSource);
        }
    }

    private IEnumerator AllowInput(bool allow, float delay = 0f)
    {
        if( delay > 0 )
        {
            yield return new WaitForSeconds(delay);
        }

        if (inputSystem != null )
        {
            inputSystem.InputAllowed = allow;

            if( allow && driverAudio != null)
            {
                driverAudio.Play();
            }
        }

        SetTimerActive(allow);
    }

    private void SetTimerActive(bool active)
    {
        m_isTimeCounting = active;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( !m_gameIsOver && m_isTimeCounting )
        {
            m_timeElapsed += Time.deltaTime;
            UpdateTimerText();
        }

        if( m_gameIsOver && bgmMusicSource != null && bgmMusicSource.isPlaying )
        {
            // Fade out
            bgmMusicSource.volume = Mathf.Floor(bgmMusicSource.volume * 0.95f) * SoundManager.Instance.GetMusicVolumeMultiplier();
        }
    }

    private bool _timerRunningOnApplicationLoseFocus;
    private void OnApplicationFocus(bool focus)
    {
        if (m_isTimeCounting && !focus )
        {
            _timerRunningOnApplicationLoseFocus = true;
            SetTimerActive(false);
        }
        else if (focus && _timerRunningOnApplicationLoseFocus )
        {
            SetTimerActive(true);
        }
    }

    private void UpdateTimerText()
    {
        foreach (var txt in timerDisplay_text )
        {
            if( txt != null )
            {
                TimeSpan timeElapsed = TimeSpan.FromSeconds(m_timeElapsed);
                txt.text = timeElapsed.ToString("m\\:ss\\.fff");
            }
        }
    }

    private void OnGameStart(GameStartEvent evt)
    {
        m_gameIsOver = false;
        StartCoroutine(BGMAudioInit());
        StartCoroutine(AllowInput(true, m_IntroLockoutTime));
    }

    private void OnGameOver(GameOverEvent evt)
    {
        m_gameIsOver = true;

        StartCoroutine(AllowInput(false, 1f));
    }

    private void OnTimeStop(TimeStopEvent evt)
    {
        SetTimerActive(false);
    }

    private void OnMenuToggled(OptionsMenuEvent evt)
    {
        if (inputSystem != null )
        {
            inputSystem.InputAllowed = !evt.Active;
        }
    }
}
