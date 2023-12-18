using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class del_tiles : MonoBehaviour
{
    public GameObject handTileHolder;


    public void DeleteTile()
    {
        foreach (var tempTile in handTileHolder.GetComponentsInChildren<TileScript>())
            {
            Destroy(tempTile.gameObject);
        }
    }

}
