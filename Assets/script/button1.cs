using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class button1 : MonoBehaviour
{

    public GameObject handTileHolder;


    public void PrintHandTiles()
    {
        foreach (TileScript tile in handTileHolder.GetComponentsInChildren<TileScript>())
        {
            Debug.Log("Letter: " + tile.GetLetter() + ", Points: " + tile.GetPoints());
        }
    }
}

