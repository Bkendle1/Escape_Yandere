using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TMP_Text _text;

    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    //convert parameter into text form to update UI
    public void UpdateUI(int number)
    {
        _text.text = number.ToString();
    }

    public void UpdateUI(float number)
    {
        _text.text = number.ToString();
    }

    //passes a copy of the parameter modifying the copy
    public void UpdateUI(string text)
    {
        _text.text = text;
    }
    
    //passes the memory address of the parameter and alters its original
    public void UpdateUI(ref string text)
    {
        _text.text = text;
    }
}