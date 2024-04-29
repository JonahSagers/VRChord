using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Gestures;
using UnityEngine.XR.Hands.Samples.Gestures.DebugTools;

public class KeyboardHandler : MonoBehaviour
{
    public List<float> leftTipCurls;
    public List<float> rightTipCurls;
    public CurlDetection leftDetect;
    public CurlDetection rightDetect;
    //these six vars are all similar data.  Might be able to ditch one with some clever destructive uses
    public float leftMax;
    public float rightMax;
    public int leftMaxIndex;
    public int rightMaxIndex;
    public int lastChordLeft;
    public int lastChordRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        //Indexes are reversed from the OpenXR standard so that pinkies are 0 and thumbs are 4
        for(int i = 0; i < leftDetect.tipCurls.Count; i++) {
            leftTipCurls[i] = leftDetect.tipCurls[4-i];
        }
        for(int i = 0; i < rightDetect.tipCurls.Count; i++) {
            rightTipCurls[i] = rightDetect.tipCurls[4-i];
        }
        leftMax = Mathf.Max(leftTipCurls.ToArray());
        if(leftMax > Average(leftTipCurls) + 0.2f){
            leftMaxIndex = leftTipCurls.IndexOf(leftMax);
        } else {
            leftMaxIndex = -1;
            lastChordLeft = -1;
        }
        rightMax = Mathf.Max(rightTipCurls.ToArray());
        if(rightMax > Average(rightTipCurls) + 0.2f){
            rightMaxIndex = rightTipCurls.IndexOf(rightMax);
        } else {
            rightMaxIndex = -1;
            lastChordRight = -1;
        }
        if(leftMaxIndex > -1 && rightMaxIndex > -1 && (leftMaxIndex != lastChordLeft || rightMaxIndex != lastChordRight)){
            Chord(leftMaxIndex, rightMaxIndex);
        }
    }
    void Chord(int leftInput, int rightInput)
    {
        lastChordLeft = leftInput;
        lastChordRight = rightInput;
        Debug.Log("Striking chord at:" + leftInput + rightInput);
    }
    public float Average(List<float> list)
    {
        float value = 0;
        foreach(float item in list){
            value += item;
        }
        value /= list.Count;
        return value;
    }
}
