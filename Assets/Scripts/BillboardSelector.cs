using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BillboardSelector : MonoBehaviour
{
    public LayerMask interactable;
    public KeyboardHandler keyboard;
    public LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(line.enabled == true){
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.green);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, interactable)){
                line.SetPosition(1, new Vector3(0,0,hit.distance));
                keyboard.selected = hit.transform.gameObject;
                keyboard.selectedText = keyboard.selected.transform.GetChild(0).GetComponent<TextMeshPro>();
                keyboard.selected.GetComponent<Outline>().enabled = true;
            } else {
                line.SetPosition(1, new Vector3(0,0,100));
                if(keyboard.selected != null){
                    keyboard.selected.GetComponent<Outline>().enabled = false;
                    keyboard.selected = null;
                    keyboard.selectedText = null;
                }
            }
        }
        
    }
}
