using System;
using System.Collections;
using System.Collections.Generic;
using Unity.LEGO.Game;
using UnityEngine;
using UnityEngine.UI;

public class WinCelebrationMessage : MonoBehaviour
{
    [SerializeField]
    Text[] finalMessage_texts;

    List<(float, string)> CompletionStringAndTimes = new List<(float, string)>(){
        ( 36f, "SUCH SPEED"),
        ( 40f, "SO FAST"),
        ( 45f, "ZOOM ZOOM"),
        ( 50f, "WEEEEEEE"),
        ( 60f, "MEH"),
        ( 80f, "EASY RIDER"),
        (2000f, "TRY AGAIN")        
    };

    private void Awake()
    {
        EventManager.AddListener<TimeStopEvent>(OnTimeStop);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<TimeStopEvent>(OnTimeStop);
    }

    private void OnTimeStop(TimeStopEvent evt)
    {
        if (evt != null )
        {
            string dispText = "";
            // Only works if time values are in order
            foreach ( var pair in CompletionStringAndTimes )
            {
                if (evt.LapTime < pair.Item1 )
                {
                    dispText = pair.Item2;
                    break;
                }
            }

            foreach (var txt in finalMessage_texts )
            {
                txt.text = dispText;
                
            }
        }
    }
}
