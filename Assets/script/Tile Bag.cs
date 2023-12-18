using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBag : MonoBehaviour
{

    public GameObject basicTile;
    public GameObject handTileHolder;

    private void Start()
    {
        for (int i = 0; i <= 5; i++)
        {
            GameObject tempTile = Instantiate(basicTile, handTileHolder.transform);
            var letter = (char)Random.Range('A', 'Z');
            tempTile.GetComponent<TileScript>().InitTile(letter.ToString(), Random.Range(1, 8));
        }
    }
}
