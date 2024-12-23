using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Gestures;
using UnityEngine.XR.Hands.Samples.Gestures.DebugTools;
using TMPro;

public class KeyboardHandler : MonoBehaviour
{
    public GameObject selected;
    public TextMeshPro selectedText;
    public Renderer render;
    public Material inertMaterial;
    public Material activeMaterial;
    public float averageCurlLeft;
    public float averageCurlRight;
    public CurlDetection leftDetect;
    public CurlDetection rightDetect;
    public bool leftFist;
    public bool rightFist;
    public bool leftPoint;
    public bool rightPoint;
    public LineRenderer leftLine;
    public LineRenderer rightLine;
    public Transform leftPos;
    public Transform rightPos;
    public bool gestureLatch;
    public bool chordingEnabled;
    //this list is 10 items long to allow for thumb inputs, but for this build I handle them separately
    public List<float> tipCurls;
    public Coroutine delKey;
    public bool enterKey;
    public bool spaceKey;
    public string inputs;
    public string lastChord;
    public TextMeshPro display;
    public TextMeshPro debugDisplay;
    public string inputBuffer;
    public bool altKeyboard;
    public Dictionary<string, string> chords = new Dictionary<string, string>{
        { "1", "a" },
        { "2", "s" },
        { "12", "w" },
        { "3", "e" },
        { "13", "x" },
        { "23", "d" },
        { "4", "t" },
        { "14", "f" },
        { "24", "c" },
        { "34", "r" },
        { "5", "n" },
        { "15", "q" },
        { "25", "j" },
        { "35", "y" },
        { "45", "b" },
        { "6", "i" },
        { "16", "z" },
        { "26", "k" },
        { "36", "," },
        { "46", "v" },
        { "56", "h" },
        { "7", "o" },
        { "17", "(" },
        { "27", "." },
        { "37", "-" },
        { "47", "g" },
        { "57", "u" },
        { "67", "l" },
        { "8", "p" },
        { "18", "?" },
        { "28", ")" },
        { "38", "'" },
        //Normally, the apostrophe goes here as key 3,8 but it wasn't on the asetniop chart for some reason
        //I added it manually, but note this in case it causes discrepancies
        //{ "48", "←" }, default asetniop backspace
        { "58", "m" },
        { "68", "!" },
        { "78", ";" },
        { "9", " " },
        { "0", "←" }, //added backspace
    };
    public Dictionary<string, string> altChords = new Dictionary<string, string>{
        { "1", "1" },
        { "2", "2" },
        { "3", "3" },
        { "4", "4" },
        { "12", "5" },
        { "23", "6" },
        { "34", "7" },
        { "14", "8" },
        { "13", "9" },
        { "24", "0" }
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(leftFist && rightFist){
            float handDist = Vector3.Distance(leftPos.position,rightPos.position);
            if(handDist < 0.12f && !gestureLatch){
                if(chordingEnabled){
                    Disable();
                } else {
                    Enable();
                }
                gestureLatch = true;
            } else if(handDist > 0.14f){
                gestureLatch = false;
            }
        }
        if(chordingEnabled){
            inputs = "";
            averageCurlLeft = 0;
            averageCurlRight = 0;
            //get curl values for each finger, thumbs are done separately
            for(int i = 1; i < leftDetect.tipCurls.Count; i++){
                tipCurls[i] = leftDetect.tipCurls[i];
                tipCurls[9-i] = rightDetect.tipCurls[i];
                averageCurlLeft += tipCurls[i];
                averageCurlRight += tipCurls[9-i];
            }
            averageCurlLeft /= 4;
            averageCurlRight /= 4;
            if(leftDetect.tipCurls[0] > 0.7f){
                altKeyboard = true;
            } else if(leftDetect.tipCurls[0] < 0.5f){
                altKeyboard = false;
            }
            if(altKeyboard && rightDetect.tipCurls[0] > 0.7f && delKey == null){
                delKey = StartCoroutine(Backspace());
            } else if((!altKeyboard || rightDetect.tipCurls[0] < 0.5f) && delKey != null){
                StopCoroutine(delKey);
                delKey = null;
            }
            if(!altKeyboard && rightDetect.tipCurls[0] > 0.7f && !spaceKey){
                display.text += " ";
                spaceKey = true;
            } else if(altKeyboard || rightDetect.tipCurls[0] < 0.5f && spaceKey){
                spaceKey = false;
            }
            if(averageCurlLeft > 0.9f && averageCurlRight > 0.9f && !enterKey){
                display.text += "\n";
                enterKey = true;
            } else if(averageCurlLeft < 0.8f || averageCurlRight < 0.8f && enterKey){
                enterKey = false;
            }
            for(int i = 0; i < 5; i++){
                if(tipCurls[i] > averageCurlLeft + 0.18f){
                    inputs += i;
                }
                if(tipCurls[i+5] > averageCurlRight + 0.18f){
                    inputs += i+5;
                }
            }
            if(inputs== ""){
                if(inputBuffer != "" && !lastChord.Contains(inputBuffer) && (chords.ContainsKey(inputBuffer) || altChords.ContainsKey(inputBuffer))){
                    Chord(inputBuffer);
                }
                inputBuffer = "";
                lastChord = "";
            } else {
                if(!lastChord.Contains(inputs) && inputs.Length > 1 && (chords.ContainsKey(inputs) || altChords.ContainsKey(inputs))){
                    Chord(inputs);
                }
            }
            if(inputs.Length == 1){
                inputBuffer = inputs;
                //allow for repeating a chord by only feathering one finger
                if(lastChord != ""){
                    lastChord = inputs;
                }
            }
        }
    }

    void Chord(string chordInputs)
    {
        lastChord = "";
        lastChord += chordInputs;
        inputBuffer = "";
        Debug.Log("Striking Chord");
        debugDisplay.text = "\nChord: ";
        debugDisplay.text += (altKeyboard) ? altChords[chordInputs] : chords[chordInputs];
        debugDisplay.text += " (";
        debugDisplay.text += chordInputs;
        debugDisplay.text += ")";
        display.text += (altKeyboard) ? altChords[chordInputs] : chords[chordInputs];
        if(selectedText != null){
            selectedText.text += (altKeyboard) ? altChords[chordInputs] : chords[chordInputs];
        }
    }

    public IEnumerator Backspace()
    {
        if(display.text.Length > 0){
            display.text = display.text.Substring(0,display.text.Length - 1);
        }
        if(selectedText != null && selectedText.text.Length > 0){
            selectedText.text = selectedText.text.Substring(0,selectedText.text.Length - 1);
        }
        yield return new WaitForSeconds(0.4f);
        while(display.text.Length > 0 || (selectedText != null && selectedText.text.Length > 0)){
            yield return new WaitForSeconds(0.05f);
            if(display.text.Length > 0){
                display.text = display.text.Substring(0,display.text.Length - 1);
            }
            if(selectedText.text.Length > 0){
                selectedText.text = selectedText.text.Substring(0,selectedText.text.Length - 1);
            }
        }
    }
    public void Enable()
    {
        chordingEnabled = true;
        debugDisplay.text = "Enabled";
        render = GameObject.Find("LeftHand").GetComponent<Renderer>();
        render.material = activeMaterial;
        render = GameObject.Find("RightHand").GetComponent<Renderer>();
        render.material = activeMaterial;
    }
    public void Disable()
    {
        selected.GetComponent<Outline>().enabled = false;
        selected = null;
        selectedText = null;
        chordingEnabled = false;
        debugDisplay.text = "Disabled";
        render = GameObject.Find("LeftHand").GetComponent<Renderer>();
        render.material = inertMaterial;
        render = GameObject.Find("RightHand").GetComponent<Renderer>();
        render.material = inertMaterial;
        inputBuffer = "";
        inputs = "";
    }
    public void LeftFist()
    {
        leftFist = true;
    }
    public void LeftFistEnd()
    {
        leftFist = false;
    }
    public void RightFist()
    {
        rightFist = true;
    }
    public void RightFistEnd()
    {
        rightFist = false;
    }
    public void LeftPoint()
    {
        leftPoint = true;
        if(!rightPoint){
            leftLine.enabled = true;
        }
    }
    public void LeftPointEnd()
    {
        leftPoint = false;
        if(leftLine.enabled){
            leftLine.enabled = false;
            Enable();
        }
        
    }
    public void RightPoint()
    {
        rightPoint = true;
        if(!leftPoint){
            rightLine.enabled = true;
        }
    }
    public void RightPointEnd()
    {
        rightPoint = false;
        if(rightLine.enabled){
            rightLine.enabled = false;
            Enable();
        }
    }
}