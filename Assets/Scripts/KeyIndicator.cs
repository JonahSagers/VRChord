using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyIndicator : MonoBehaviour
{
    private KeyboardHandler keyboard;
    private TextMeshPro display;
    //this should be refactored later to not hardcode each display
    //a list of display values would be almost required for language support
    public string primaryInputs;
    public string primaryDisplay;
    public string secondaryInputs;
    public string secondaryDisplay;
    public string altPrimaryInputs;
    public string altPrimaryDisplay;
    public string altSecondaryInputs;
    public string altSecondaryDisplay;

    void Start()
    {
        keyboard = GameObject.Find("Gesture Tracker").GetComponent<KeyboardHandler>();
        display = GetComponent<TextMeshPro>();
    }

    void Update()
    {
            if(keyboard.altKeyboard && altPrimaryInputs.Contains(keyboard.inputs) && keyboard.chordingEnabled){
                display.text = altPrimaryDisplay;
            } else if(keyboard.altKeyboard && altSecondaryInputs.Contains(keyboard.inputs) && keyboard.chordingEnabled){
                display.text = altSecondaryDisplay;
            } else if(!keyboard.altKeyboard && primaryInputs.Contains(keyboard.inputs) && keyboard.chordingEnabled){
                display.text = primaryDisplay;
            } else if(!keyboard.altKeyboard && secondaryInputs.Contains(keyboard.inputs) && keyboard.chordingEnabled){
                display.text = secondaryDisplay;
            } else {
                display.text = "";
            }
    }
}
