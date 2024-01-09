using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardScript;

public class ValTiles : MonoBehaviour
{
    //public string[,] valTiles = new string[15, 15];
    //private ValTiles valTiles;
    

    BoardScript boardScript;

    //string[,] valTiles = boardScript.valTiles;
    private void Start()
    {
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();
    }
    public void PrintValidationResult()
    {
        Debug.Log(IsValid());
    }

    public bool IsValid()
    {
        List<TileMove> recordedPositions = boardScript.GetRecordedPositions();

        bool isValid = false;

        if (recordedPositions.Count == 1)
        { // Check if there's only one tile
            isValid = true;
        }
        else if (CheckSameLine(recordedPositions, boardScript.valTiles))
        { // Check if tiles share the same line
            isValid = true;
        }

        Debug.Log("Tile placement validity: " + isValid); // Debug log of the validity result

        return isValid; // Return the validity status
    }


    private bool CheckSameLine(List<TileMove> tileMove, TileScript2[,] valTiles)
    {
        // Check if all tiles share the same line (either horizontal or vertical)
        bool sameLine = true;

        int firstXCoordinate = tileMove[0].X;
        int firstYCoordinate = tileMove[0].Y;

        for (int i = 1; i < tileMove.Count; i++)
        {
            if (tileMove[i].X != firstXCoordinate && tileMove[i].Y != firstYCoordinate)
            {
                sameLine = false;
                break;
            }
        }

        if (!sameLine)
        {
            return false; // Not a valid line
        }

        // Check for gaps and filled gaps
        bool gapFoundWithValidTile = false; // Flag to track filled gaps
        bool gapFoundWithoutValidTile = false; // Flag to track unfilled gaps

        if (tileMove[0].X == firstXCoordinate)
        {
            // Horizontal line, check gaps along the y-axis
            for (int i = 1; i < tileMove.Count; i++)
            {
                if (tileMove[i].X - tileMove[i - 1].Y > 1)
                {
                    gapFoundWithValidTile = gapFoundWithValidTile || CheckGap(tileMove[i - 1], tileMove[i], valTiles);
                    gapFoundWithoutValidTile = !gapFoundWithValidTile;
                }
            }
        }
        else
        {
            // Vertical line, check gaps along the x-axis
            for (int i = 1; i < tileMove.Count; i++)
            {
                if (tileMove[i].X - tileMove[i - 1].X > 1)
                {
                    gapFoundWithValidTile = gapFoundWithValidTile || CheckGap(tileMove[i - 1], tileMove[i], valTiles);
                    gapFoundWithoutValidTile = !gapFoundWithValidTile;
                }
            }
        }

        if (gapFoundWithoutValidTile)
        {
            return false; // Unfilled gaps exist
        }

        return true; // Valid line, either no gaps or all gaps are filled with validated tiles
    }

    private bool CheckGap(TileMove tileMove1, TileMove tileMove2, TileScript2[,] valTiles)
    {
        if (tileMove2.Y - tileMove1.Y > 1)
        {
            // Horizontal gap
            for (int j = tileMove1.Y + 1; j < tileMove2.Y; j++)
            {
                if (valTiles[tileMove1.X, j] != null)
                {
                    return true; // Gap filled with a validated tile
                }
            }
        }
        else
        {
            // Vertical gap
            for (int j = tileMove1.X + 1; j < tileMove2.X; j++)
            {
                if (valTiles[j, tileMove1.Y] != null)
                {
                    return true; // Gap filled with a validated tile
                }
            }
        }

        return false; // Gap is unfilled
    }
}