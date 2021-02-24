using UnityEngine;

public class LimitRoll : MonoBehaviour
{
    #region Inspector-Assigned Fields
    [SerializeField]
    private float maxRollAngle = 30f;
    #endregion

    #region Private & Protected

    #endregion

    private void Awake()
    {
        
    }

    float rollAngle;

    public void FixedUpdate()
    {
        rollAngle = transform.localRotation.eulerAngles.z;

        if ( rollAngle < 180 && Mathf.Abs(rollAngle) > maxRollAngle)
        {
            float nudge = Mathf.Lerp(transform.localEulerAngles.z, 0, Time.deltaTime);
            
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, nudge);
        }
    }

}
