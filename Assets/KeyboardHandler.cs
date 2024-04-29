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
    public List<int> inputs;
    public List<int> lastChord;
    public TextMeshPro display;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        //Indexes are reversed from the OpenXR standard so that pinkies are 0 and thumbs are 4
        for(int i = 0; i < leftDetect.tipCurls.Count; i++) {
            tipCurls[i] = leftDetect.tipCurls[4-i];
            tipCurls[i+5] = rightDetect.tipCurls[4-i];
            averageCurlLeft += tipCurls[i];
            averageCurlRight += tipCurls[i+5];
        }
        averageCurlLeft /= 5;
        averageCurlRight /= 5;
        inputs.Clear();
        for(int i = 0; i < 5; i++) {
            if(tipCurls[i] > averageCurlLeft + 0.15f){
                inputs.Add(i);
            }
            if(tipCurls[i+5] > averageCurlRight + 0.15f){
                inputs.Add(i+5);
            }
        }
        if(inputs.Count == 0){
            lastChord.Clear();
        } else {
            if(!CheckEqual(inputs,lastChord)){
                Chord(inputs);
            }
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

    void Chord(List<int> chordInputs)
    {
        lastChord.Clear();
        foreach(int input in chordInputs){
            lastChord.Add(input);
        }
        Debug.Log("Striking Chord");
        display.text += "\nChord";
        foreach(int input in chordInputs){
            display.text += input.ToString();
        }
    }
    public bool CheckEqual(List<int> list1, List<int> list2)
    {
        if(list1.Count == list2.Count){
            for(int i = 0; i < list1.Count; i++) {
                if(list1[i] != list2[i]){
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
