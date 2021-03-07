using System.Collections;
using System.Collections.Generic;
using Unity.LEGO.Behaviours.Actions;
using Unity.LEGO.Game;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCount : MonoBehaviour
{
    [SerializeField]
    Text[] collectedCountText;

    [SerializeField]
    Animator animator;

    [SerializeField]
    string animator_CollectTrigger = "Collect";

    [SerializeField]
    string animator_AllCollected = "AllCollected";

    private int collectedCount;
    private int totalToCollect;

    List<PickupAction> pickupList;

    private void Awake()
    {
        pickupList = new List<PickupAction>();
        pickupList.AddRange(FindObjectsOfType<PickupAction>());

        if( pickupList.Count > 0 )
        {
            totalToCollect = pickupList.Count;
            pickupList?.ForEach(pickup => RegisterCollectionItem(pickup));
        }

        //animator.SetBool(animator_AllCollected, false);

        UpdateText();
    }

    public void UpdateText()
    {
        if (collectedCountText != null )
        {
            foreach(var txt in collectedCountText )
            {
                if( txt != null )
                {
                    txt.text = $"{collectedCount}/{totalToCollect}";
                }
            }
        }
    }

    public void ResetCollectedCount()
    {
        collectedCount = 0;
    }


    public void RegisterCollectionItem(PickupAction item)
    {
        if (item != null )
        {
            item.OnCollected += OnObjectCollected;
        }
    }

    public void UnRegisterCollectionItem(PickupAction item)
    {
        if (item != null )
        {
            item.OnCollected -= OnObjectCollected;
        }
    }

    public void OnObjectCollected(PickupAction pickup)
    {
        if (pickup != null )
        {
            UnRegisterCollectionItem(pickup);
            collectedCount++;

            if (animator != null )
            {
                animator.SetTrigger(animator_CollectTrigger);
            }

            UpdateText();

            if (collectedCount >= totalToCollect )
            {
                if (animator != null )
                {
                    animator.SetBool(animator_AllCollected, true);
                }

                float laptime = 0;

                TimerDisplay timerDisp = FindObjectOfType<TimerDisplay>();
                if( timerDisp != null )
                {
                    laptime = timerDisp.GetTimeElapsed();
                }

                EventManager.Broadcast(new TimeStopEvent(laptime));
                Debug.Log("YOU WIN!");

                StartCoroutine(StopGame(3.5f));
            }
        }
    }

    private IEnumerator StopGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventManager.Broadcast(new GameOverEvent());
    }
}
