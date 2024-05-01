using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Gestures;
using UnityEngine.XR.Hands.Samples.Gestures.DebugTools;
using TMPro;

public class KeyboardHandler : MonoBehaviour
{
    public float averageCurlLeft;
    public float averageCurlRight;
    public CurlDetection leftDetect;
    public CurlDetection rightDetect;
    //these six vars are all similar data.  Might be able to ditch one with some clever destructive uses
    public List<float> tipCurls;
    public string inputs;
    public string lastChord;
    public TextMeshPro display;
    public string inputBuffer;
    public Dictionary<string, string> chords = new Dictionary<string, string>{
        { "0", "a" },
        { "1", "s" },
        { "01", "w" },
        { "2", "e" },
        { "02", "x" },
        { "12", "d" },
        { "3", "t" },
        { "03", "f" },
        { "13", "c" },
        { "23", "r" },
        { "4", "n" },
        { "04", "q" },
        { "14", "j" },
        { "24", "y" },
        { "34", "b" },
        { "5", "i" },
        { "05", "z" },
        { "15", "k" },
        { "25", "," },
        { "35", "v" },
        { "45", "h" },
        { "6", "o" },
        { "06", "(" },
        { "16", "." },
        { "26", "-" },
        { "36", "g" },
        { "46", "u" },
        { "56", "l" },
        { "7", "p" },
        { "07", "?" },
        { "17", ")" },
        { "37", "←" },
        { "47", "m" },
        { "57", "!" },
        { "67", ";" },
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputs = "";
        for(int i = 0; i < leftDetect.tipCurls.Count; i++) {
            tipCurls[i] = leftDetect.tipCurls[i];
            tipCurls[i+5] = rightDetect.tipCurls[4-i];
            averageCurlLeft += tipCurls[i];
            averageCurlRight += tipCurls[i+5];
        }
        averageCurlLeft /= 5;
        averageCurlRight /= 5;
        for(int i = 0; i < 5; i++) {
            if(tipCurls[i] > averageCurlLeft + 0.15f){
                inputs += i;
            }
            if(tipCurls[i+5] > averageCurlRight + 0.15f){
                inputs += i+5;
            }
        }
        if(inputs== ""){
            if(inputBuffer != "" && !lastChord.Contains(inputBuffer)){
                Chord(inputBuffer);
            }
            inputBuffer = "";
            lastChord = "";
        } else {
            if(!lastChord.Contains(inputs) && inputs.Length > 1){
                Chord(inputs);
            }
        }
        if(inputs.Length == 1){
            inputBuffer = inputs;
        }
        // leftMax = Mathf.Max(leftTipCurls.ToArray());
        // if(leftMax > Average(leftTipCurls) + 0.2f){
        //     leftMaxIndex = leftTipCurls.IndexOf(leftMax);
        // } else {
        //     leftMaxIndex = -1;
        //     lastChordLeft = -1;
        // }
        // rightMax = Mathf.Max(rightTipCurls.ToArray());
        // if(rightMax > Average(rightTipCurls) + 0.2f){
        //     rightMaxIndex = rightTipCurls.IndexOf(rightMax);
        // } else {
        //     rightMaxIndex = -1;
        //     lastChordRight = -1;
        // }
        // if(leftMaxIndex > -1 && rightMaxIndex > -1 && (leftMaxIndex != lastChordLeft || rightMaxIndex != lastChordRight)){
        //     Chord(leftMaxIndex, rightMaxIndex);
        // }
    }

    void Chord(string chordInputs)
    {
        //had some pointer nonsense with this
        lastChord = "";
        lastChord += chordInputs;
        inputBuffer = "";
        Debug.Log("Striking Chord");
        display.text += "\nChord: ";
        display.text += chords[chordInputs];
        display.text += " (";
        display.text += chordInputs;
        display.text += ")";
    }
}
