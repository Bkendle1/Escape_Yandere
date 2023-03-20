using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //store the keys the player has collected
    [HideInInspector] public List<Colorkeys> m_keyStorage = new List<Colorkeys>();

    public void AddKey(Colorkeys key)
    {
        //check whether or not the player has the key
        if (m_keyStorage.Contains(key))
        {
            Debug.Log("you already have this key");
            return;
        }
        else
        {
            Debug.Log("key obtained");
            m_keyStorage.Add(key);
        }
    }
    
    //check whether we have the key to unlock the door
    public bool DoWeHaveTheKey(Colorkeys key)
    {
        return m_keyStorage.Contains(key);
    }
}
