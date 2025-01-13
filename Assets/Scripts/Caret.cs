using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Caret : MonoBehaviour
{
    public Renderer render;
    public TextMeshPro display;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        float elapsed = 0;
        while(true){
            if(display.text.Length < 1){
                transform.localPosition = new Vector3(-3.482394f,1.577121f,0f);
            } else {
                TMP_TextInfo textInfo = display.textInfo;
                TMP_CharacterInfo charInfo;
                try{
                    charInfo = textInfo.characterInfo[display.text.Length - 1];
                }
                catch {
                    charInfo = textInfo.characterInfo[display.text.Length - 2];
                }
                transform.localPosition = charInfo.bottomRight + new Vector3(0.15f, 0.26f, 0);
            }
            if(elapsed % 1 < 0.5){
                render.enabled = false;
            } else {
                render.enabled = true;
            }
            elapsed += Time.deltaTime;
            yield return 0;
        }
    }
}
