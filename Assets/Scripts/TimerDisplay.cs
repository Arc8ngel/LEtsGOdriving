﻿using System;
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
        EventManager.AddListener<TimeStop>(OnTimeStop);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<GameStartEvent>(OnGameStart);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<TimeStop>(OnTimeStop);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inputSystem != null )
        {
            inputSystem.InputAllowed = false;
        }

        StartCoroutine(BGMAudioInit());
    }

    private IEnumerator BGMAudioInit()
    {
        yield return new WaitForSeconds(bgmMusicStartDelay);

        bgmMusicSource?.Play();
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
            bgmMusicSource.volume = Mathf.Floor(bgmMusicSource.volume * 0.95f);
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
        StartCoroutine(AllowInput(true, m_IntroLockoutTime));
    }

    private void OnGameOver(GameOverEvent evt)
    {
        m_gameIsOver = true;

        StartCoroutine(AllowInput(false, 1f));
    }

    private void OnTimeStop(TimeStop evt)
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
