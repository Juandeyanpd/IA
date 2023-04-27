using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AiSensor : MonoBehaviour
{
    public float visionAngle = 45;
    public float maxVisionDistance = 20f;

    public Color visionColor;

    private void Update()
    {
        Vector3 targetDirection = AiManager.instance.player.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);

        if (angle < visionAngle)
        {
            Debug.Log("Lo está viendo");
        }
    }
}

#if UNITY_EDITOR
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
