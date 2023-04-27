using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AiSensor : MonoBehaviour
{
    public float visionAngle = 45;
    public float maxVisionDistance = 20f;

    public Color visionColor;
    public LayerMask mask;

    [Serializable]
    public class OnPlayerDetectClass : UnityEvent { }

    [FormerlySerializedAs("OnDetectPlayer")]
    [SerializeField]
    public OnPlayerDetectClass m_OnDetectPlayer = new OnPlayerDetectClass();

    [Serializable]
    public class OnPlayerLostClass : UnityEvent { }

    [FormerlySerializedAs("OnPlayerLost")]
    [SerializeField]
    public OnPlayerLostClass m_OnLostPlayer = new OnPlayerLostClass();


    private void Update()
    {
        Vector3 targetDirection = AiManager.instance.player.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);

        if (angle < visionAngle)
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, targetDirection, out hit, maxVisionDistance, mask))
            {
                if (hit.collider != null)
                {
                    if(hit.collider.transform == AiManager.instance.player)
                    {
                        Debug.DrawRay(transform.position, targetDirection, Color.red);
                        m_OnDetectPlayer?.Invoke();
                    }
                    else
                    {
                        m_OnLostPlayer?.Invoke();
                    }
                }
            }
        }
        else
        {
            m_OnLostPlayer?.Invoke();
        }
    }
}

#if UNITY_EDITOR
[ExecuteAlways]
[CustomEditor(typeof(AiSensor))] 
public class EnemyVisionSensor: Editor
{
    public void OnSceneGUI()
    {
        var ai = target as AiSensor;

        Vector3 startPoint = Mathf.Cos(-ai.visionAngle * Mathf.Deg2Rad) * ai.transform.forward + 
                                        Mathf.Sin(ai.visionAngle * Mathf.Deg2Rad) * -ai.transform.right;

        Handles.color = ai.visionColor;
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.visionAngle * 2f, ai.maxVisionDistance);
    }
}
#endif
