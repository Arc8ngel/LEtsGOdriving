using System.Collections;
using System.Collections.Generic;
using Unity.LEGO.Behaviours.Actions;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCount : MonoBehaviour
{
    [SerializeField]
    Text[] collectedCountText;

    [SerializeField]
    Animator collectionImageAnimator;

    [SerializeField]
    string animator_CollectTrigger = "Collect";

    private int collectedCount;
    private int totalToCollect;

    List<PickupAction> pickupList;

    public delegate void EventHandler();
    public event EventHandler AllPickupsCollected;

    private void Awake()
    {
        pickupList = new List<PickupAction>();
        pickupList.AddRange(FindObjectsOfType<PickupAction>());

        if( pickupList.Count > 0 )
        {
            totalToCollect = pickupList.Count;
            pickupList?.ForEach(pickup => RegisterCollectionItem(pickup));
        }

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

            if (collectionImageAnimator != null )
            {
                collectionImageAnimator.SetTrigger(animator_CollectTrigger);
            }

            UpdateText();

            if (collectedCount >= totalToCollect )
            {
                AllPickupsCollected?.Invoke();
                Debug.Log("YOU WIN!");
            }
        }
    }
}
