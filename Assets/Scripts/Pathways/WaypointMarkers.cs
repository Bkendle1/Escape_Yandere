using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class WaypointMarkers : MonoBehaviour
{
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.cyan;
        Gizmos.color = Color.red;
        Handles.Label(transform.position, transform.gameObject.name, style);
    }
    
    #endif
}
