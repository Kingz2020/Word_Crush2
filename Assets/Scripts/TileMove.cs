using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMove: MonoBehaviour {
    public int X { get; set; }
    public int Y { get; set; }
    public bool onBoard = false;
    

    // Start is called before the first frame update

    public void SetTileMove(Vector2Int recordedPosition) {
        X = recordedPosition.x;
        Y = recordedPosition.y;
        onBoard = true;
    }


}



