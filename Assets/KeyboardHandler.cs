using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Gestures;
using UnityEngine.XR.Hands.Samples.Gestures.DebugTools;
using TMPro;

public class KeyboardHandler : MonoBehaviour
{
    public Renderer render;
    public Material inertMaterial;
    public Material activeMaterial;
    public float averageCurlLeft;
    public float averageCurlRight;
    public CurlDetection leftDetect;
    public CurlDetection rightDetect;
    public bool leftFist;
    public bool rightFist;
    public Transform leftPos;
    public Transform rightPos;
    public bool gestureLatch;
    public bool enabled;
    public List<float> tipCurls;
    public string inputs;
    public string lastChord;
    public TextMeshPro display;
    public TextMeshPro debugDisplay;
    public string inputBuffer;
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
        //{ "48", "←" }, default asetniop backspace
        { "58", "m" },
        { "68", "!" },
        { "78", ";" },
        { "9", " " },
        { "0", "←" }, //added backspace
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(leftFist && rightFist){
            if(Vector3.Distance(leftPos.position,rightPos.position) < 0.15f && !gestureLatch){
                if(enabled){
                    Disable();
                } else {
                    Enable();
                }
                gestureLatch = true;
            //can avoid checking position twice if you weren't allergic to placeholder variables
            } else if(Vector3.Distance(leftPos.position,rightPos.position) > 0.17f){
                gestureLatch = false;
            }
        }
        if(enabled){
            inputs = "";
            for(int i = 0; i < leftDetect.tipCurls.Count; i++){
                tipCurls[i] = leftDetect.tipCurls[i];
                tipCurls[i+5] = rightDetect.tipCurls[4-i];
                averageCurlLeft += tipCurls[i];
                averageCurlRight += tipCurls[i+5];
            }
            averageCurlLeft /= 5;
            averageCurlRight /= 5;
            //two for loops is necessary to keep inputs in order
            //can be done with one if you sort the inputs after, but that's way messier
            //thought: could reformat the dictionary to have the inputs in a different order, but it's not even that big a deal to have two for loops
            for(int i = 0; i < 5; i++){
                if(tipCurls[i] > averageCurlLeft + 0.15f){
                    inputs += i;
                }
            }
            for(int i = 0; i < 5; i++){
                if(tipCurls[i+5] > averageCurlRight + 0.15f){
                    inputs += i+5;
                }
            }
            if(inputs== ""){
                if(inputBuffer != "" && !lastChord.Contains(inputBuffer) && chords.ContainsKey(inputBuffer)){
                    Chord(inputBuffer);
                }
                inputBuffer = "";
                lastChord = "";
            } else {
                if(!lastChord.Contains(inputs) && inputs.Length > 1 && chords.ContainsKey(inputs)){
                    Chord(inputs);
                }
            }
            if(inputs.Length == 1){
                inputBuffer = inputs;
            }
        }
    }

    void Chord(string chordInputs)
    {
        //had some pointer nonsense with this
        //does C# even have pointers?
        lastChord = "";
        lastChord += chordInputs;
        inputBuffer = "";
        Debug.Log("Striking Chord");
        debugDisplay.text += "\nChord: ";
        debugDisplay.text += chords[chordInputs];
        debugDisplay.text += " (";
        debugDisplay.text += chordInputs;
        debugDisplay.text += ")";
        if(chords[chordInputs] == "←"){
            display.text = display.text.Substring(0,display.text.Length - 1);
        } else {
            display.text += chords[chordInputs];
        }
    }
    public void Enable()
    {
        enabled = true;
        debugDisplay.text = "Enabled";
        render = GameObject.Find("LeftHand").GetComponent<Renderer>();
        render.material = activeMaterial;
        render = GameObject.Find("RightHand").GetComponent<Renderer>();
        render.material = activeMaterial;
    }
    public void Disable()
    {
        enabled = false;
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
}
