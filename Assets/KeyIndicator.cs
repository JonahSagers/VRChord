using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyIndicator : MonoBehaviour
{
    private KeyboardHandler keyboard;
    private TextMeshPro text;
    public string targetInputs;

    void Start()
    {
        keyboard = GameObject.Find("Gesture Tracker").GetComponent<KeyboardHandler>();
        text = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        text.enabled = targetInputs.Contains(keyboard.inputBuffer) && keyboard.chordingEnabled;
    }
}
