using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyIndicator : MonoBehaviour
{
    private KeyboardHandler keyboard;
    private TextMeshPro display;
    public string primaryInputs;
    public string primaryDisplay;
    public string secondaryInputs;
    public string secondaryDisplay;

    void Start()
    {
        keyboard = GameObject.Find("Gesture Tracker").GetComponent<KeyboardHandler>();
        display = GetComponent<TextMeshPro>();
    }

    void Update()
    {
            if(primaryInputs.Contains(keyboard.inputs) && keyboard.chordingEnabled){
                display.text = primaryDisplay;
            } else if(secondaryInputs.Contains(keyboard.inputs) && keyboard.chordingEnabled){
                display.text = secondaryDisplay;
            } else {
                display.text = "";
            }
    }
}
