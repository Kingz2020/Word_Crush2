using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class create_tile : MonoBehaviour
{
    // Note the real create tile will be taking a tile from the tile bag which will have all the tiles
    // here were just creating a random tile.

    public GameObject basicTile;
    public GameObject handTileHolder;

    public void make_tile()
    {
        GameObject tempTile = Instantiate(basicTile, handTileHolder.transform);
        var letter = (char)Random.Range('A', 'Z');
        tempTile.GetComponent<TileScript>().InitTile(letter.ToString(), Random.Range(1, 8));
    }
    

}
