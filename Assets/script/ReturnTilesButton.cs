using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardScript;

public class ReturnTilesButton : MonoBehaviour
{
    public GameObject handTileHolder; // Reference to the hand tiles GameObject

    BoardScript boardScript; // Reference to the BoardScript
    TileBag2 tileBag2Script;

    private void Start()
    {
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();
        tileBag2Script= GameObject.Find("Handtiles").GetComponent<TileBag2>();
    }
    public void RetrieveTilesFromBoard()
    {
        boardScript.RetrieveTilesFromBoard();
       /* List<TileMove> recordedPositions = boardScript.GetRecordedPositions();

        foreach (TileMove position in recordedPositions)
        {
            string tileName = "Tile" + position.X + "X" + position.Y + "Y";
            GameObject tile = GameObject.Find(tileName);

            if (tile != null)
            {
                tileBag2Script.AddTileToHand(tile);
                // Adjust position/size if needed

                boardScript.RemoveTileFromBoard(tile);
            }
        }*/
    }
    
}