using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Score : MonoBehaviour
{
    BoardScript boardScript;
    TileBag tileBag;

    public TextMeshProUGUI textScore;
    List<string> words = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();
        tileBag = GameObject.Find("TileBag2").GetComponent<TileBag>();
        
    }
    
}
