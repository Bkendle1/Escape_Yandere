using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointPaths : MonoBehaviour
{
    [SerializeField] private WaypointMarkers[] m_waypointPath;
    
    public Transform GetWaypoint(int index)
    {
        //return the position of the indexed waypoint
        return m_waypointPath[index].transform;
    }

    public int GetWaypointCount()
    {
        //return number of elements in waypointPath array
        return m_waypointPath.Length;
    }
    
    
    #if UNITY_EDITOR

    public void OnDrawGizmosSelected()
    {
        //if we dont have waypoints or there's less than 2 don't do anything
        if (m_waypointPath == null || m_waypointPath.Length < 2)
        {
            return;
        }
        
        //if we have data in at index 0 of array...
        if (m_waypointPath[0] != null)
        {
            for (int i = 1; i < m_waypointPath.Length; i++)
            {
                //draw a line between the current index and the index before the current if they both exist
                if (m_waypointPath[i - 1] != null && m_waypointPath[i] != null)
                {
                    Gizmos.DrawLine(m_waypointPath[i-1].transform.position, m_waypointPath[i].transform.position);
                }
            }
        }
    }

    #endif
}
 