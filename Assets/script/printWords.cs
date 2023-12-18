using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PrintWords : MonoBehaviour
{

    private BoardScript boardScript;
    private EndTurnBtn endTurnBtn;
    private ValTiles valTiles;
    private const int boardSize = 15;

    [SerializeField] private TextAsset scrabbleWordsList;
    private List<string> scrabbleWords;

    public void Start()
    {
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>(); // Initialize reference to BoardScript
        endTurnBtn = GameObject.Find("Button_end_turn").GetComponent<EndTurnBtn>();
        scrabbleWords = new List<string>(scrabbleWordsList.text.Split(new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None));
    }

    public bool IsInDict(string word)
    {
        if (!scrabbleWords.Contains(word))
            return false;
        else
            return true;
    }

    
    //TilePlacement is the enum that tells you the direction
    public enum TilePlacement
    {
        Horizontal, Vertical, SingleTile, NoTilePlaced, WrongTilePlacement
    }
    //PlacedTile is a combination of location of the tile in x/y and information about the tile itself(letter/ points)


    public void  DebugPrintMeth()
    {

        Debug.Log(boardScript.AllTilesInSameLine());
    }
    private bool IsAttached(List<TileMove> recordedPositions, string[,] valTiles)
    {
        // Check if any of the recorded positions are adjacent to a valTile
        foreach (TileMove position in recordedPositions)
        {
            // Check around the current position in all directions
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    // Skip the current position itself
                    if (dx == 0 && dy == 0) continue;

                    // Check if the adjacent position is within the board boundaries
                    int x = position.X + dx;
                    int y = position.Y + dy;
                    if (x < 0 || x >= valTiles.GetLength(0) || y < 0 || y >= valTiles.GetLength(1)) continue;

                    // Check if the adjacent position is a valTile
                    if (valTiles[x, y] != null) return true;
                }
            }
        }
        // If no adjacent valTile is found, return false
        return false;
    }
    public void printwordz()
    {
        StringBuilder wordList = new StringBuilder();
        bool isAttached = IsAttached(boardScript.recordedPositions, boardScript.valTiles);
        bool isEmptyBoard = boardScript.CheckEmptyBoard(boardScript.valTiles);

        if (isAttached || (!isAttached && isEmptyBoard))
        {
            foreach (var word in boardScript.CollectAllWords(boardScript.AllTilesInSameLine()))
            {
                wordList.Append(word + ", ");
                IsInDict(word);

            }
            Debug.Log(wordList.ToString());

        }
        else 
        {
            Debug.Log("not attached");
        }

    }

    

    

    
}
