using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class direction : MonoBehaviour
{
    // Start is called before the first frame update
    PrintWords printWords;
    readonly BoardScript boardScript;

    public void Start()
    {
        printWords = GameObject.Find("Button_print_words").GetComponent<PrintWords>(); // Initialize reference to BoardScript

    }

    public void CheckSameLine()
    {
        Debug.Log(boardScript.AllTilesInSameLine());
    }
   
}
