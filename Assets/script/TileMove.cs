using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMove
{
    public int X { get; set; }
    public int Y { get; set; }

    public string letter;

    // Start is called before the first frame update

    public TileMove(Vector2Int recordedPosition, string letter)
    {
        this.X = recordedPosition.x;
        this.Y = recordedPosition.y;
        this.letter = letter;
    }


}



