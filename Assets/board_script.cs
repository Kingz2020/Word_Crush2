using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static PrintWords;
using static UnityEngine.RuleTile.TilingRuleOutput;



public class BoardScript : MonoBehaviour
{
    public TextMeshProUGUI textScore;
    //private ValTiles valTiles;

    TileBag2 tileBag2;
    TileScript2 tileScript2;
    public UnityEvent hidePointTiles;

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    //private List<Vector2Int> recordedPositions = new List<Vector2Int>();
    public List<TileMove> recordedPositions = new List<TileMove>();
    public string[,] valTiles = new string[15, 15];
    public int currentScore;

    public void HideAllPointTiles()
    {
         hidePointTiles?.Invoke();
    }
    private void Start()
    {
        tileBag2 = GameObject.Find("Handtiles").GetComponent<TileBag2>();
        //tileScript2 = GameObject.Find("New Basic Tile").GetComponent<TileScript2>();
    }
    
public void RecordTilePosition(TileMove tileMove)
    {
        // Record the position of the placed tile.
        recordedPositions.Add(tileMove);
    }

    

    public List<TileMove> GetRecordedPositions()
    {
        return recordedPositions;
    }

    public void DisplayRecordedPositions()
    {
        foreach (var tileMove in recordedPositions)
        {
            Debug.Log("Recorded Position: " + tileMove.X + ", " + tileMove.Y);
            Debug.Log("Letter: " + tileMove.letter);
        }
    }

    public GameObject GetTileAtPosition(Vector2Int position)
    {
        GameObject[] tilesOnBoard = GameObject.FindGameObjectsWithTag("New Basic Tile");

        foreach (GameObject tile in tilesOnBoard)
        {
            TileScript2 tileScript = tile.GetComponent<TileScript2>();

            // Retrieve the recorded local positions of the tiles.
            List<Vector3> localPositions = tileScript.GetLocalPositions();

            foreach (var localPos in localPositions)
            {
                Vector3 boardPosition = transform.position; // Adjust this to match the board's position or relative positioning.

                // Check if the calculated local position matches the provided position.
                if (Vector2Int.FloorToInt(boardPosition) == position)
                {
                    return tile;
                }
            }
        }

        return null; // If no tile is found at the given position.
    }
    /*public void RemoveTileFromBoard(GameObject tile)
    {
        // Disable the tile's visual representation
        //tile.SetActive(false);

        // Retrieve the tile's position from the recorded positions
        //List<TileMove> recordedPositions = GetRecordedPositions();
        //Vector2Int tilePosition = GetTilePositionFromGameObject(tile);

        // Remove the tile from the recorded positions
        //recordedPositions.RemoveAll(tileMove => tileMove.X == tilePosition.x && tileMove.Y == tilePosition.y );


    }*/

    public TilePlacement AllTilesInSameLine()
    {
        //string[,] valTiles = valTiles;
        List<TileMove> thisTurn = GetRecordedPositions();
        if (thisTurn.Count == 0) return TilePlacement.NoTilePlaced;
        if (thisTurn.Count == 1) return TilePlacement.SingleTile;
        TileMove checkTile = thisTurn[0];
        bool vertical = true;
        bool horizontal = true;
        foreach (var pos in thisTurn)
        {
            if (pos.X != checkTile.X) horizontal = false;
            if (pos.Y != checkTile.Y) vertical = false;
        }
        if (vertical) return TilePlacement.Vertical;
        if (horizontal) return TilePlacement.Horizontal;
        return TilePlacement.WrongTilePlacement;
    }

    private Vector2Int GetTilePositionFromGameObject(GameObject tile)
    {
        string tileName = tile.name;

        // Extract the X and Y coordinates from the tile's name
        int xPosition = int.Parse(tileName.Substring(4, 1));
        int yPosition = int.Parse(tileName.Substring(6, 1));

        // Return the tile's position as a Vector2Int
        return new Vector2Int(xPosition, yPosition);
    }

    public void RetrieveTilesFromBoard()
    {
        //List<TileMove> recordedPositions = boardScript.GetRecordedPositions();

        foreach (TileMove position in recordedPositions)
        {
            string tileName = "Tile" + position.X + "X" + position.Y + "Y";
            GameObject tile = GameObject.Find(tileName);

            if (tile != null)
            {
                tileBag2.AddTileToHand(tile);
                // Adjust position/size if needed

                //RemoveTileFromBoard(tile);
            }
        }
        recordedPositions.Clear();
    }
    public List<String> CollectAllWords(TilePlacement orientation)
    {
        List<String> wordList = new List<String>();
        //string[,] tempBoard = (string[, ]) ValTiles.Clone();
        string[,] tempBoard = (string[,])valTiles.Clone();

        foreach (var tempTile in GetRecordedPositions())
        {
            tempBoard[tempTile.X, tempTile.Y] = tempTile.letter;
        }



        //foreach (var tempTile in GetRecordedPositions())
        //{



        if (orientation == TilePlacement.SingleTile)
        {   var singleTile = GetRecordedPositions()[0];
             if( tempBoard[singleTile.X - 1, singleTile.Y] == null
                && tempBoard[singleTile.X, singleTile.Y - 1] == null
                && tempBoard[singleTile.X + 1, singleTile.Y] == null
                && tempBoard[singleTile.X, singleTile.Y + 1] == null)
            {
                wordList.Add(singleTile.letter);
                return wordList;

            }
        }
        //(//)}

        foreach (var tempTile in GetRecordedPositions())
        { 

            if (orientation == TilePlacement.Horizontal || orientation == TilePlacement.SingleTile)
            {
                if (tempBoard[tempTile.X - 1, tempTile.Y] != null)
                {
                    wordList.Add(GetWordFromBoard(TilePlacement.Horizontal, tempBoard, GetFirstLetterIndex(TilePlacement.Horizontal, tempBoard, tempTile.X, tempTile.Y), tempTile.Y));
                }
                else if (tempBoard[tempTile.X + 1, tempTile.Y] != null)
                {
                    wordList.Add(GetWordFromBoard(TilePlacement.Horizontal, tempBoard, tempTile.X, tempTile.Y));
                }
            }
            if (orientation == TilePlacement.Vertical || orientation == TilePlacement.SingleTile)
            {
                if (tempBoard[tempTile.X, tempTile.Y - 1] != null)
                {
                    wordList.Add(GetWordFromBoard(TilePlacement.Vertical, tempBoard, tempTile.X, GetFirstLetterIndex(TilePlacement.Vertical, tempBoard, tempTile.X, tempTile.Y)));
                }
                else if (tempBoard[tempTile.X, tempTile.Y + 1] != null)
                {
                    wordList.Add(GetWordFromBoard(TilePlacement.Vertical, tempBoard, tempTile.X, tempTile.Y));
                }
            }
        }

        TileMove placedTile = GetRecordedPositions()[0];
        if (orientation == TilePlacement.Horizontal)
        {
            if (tempBoard[placedTile.X, placedTile.Y - 1] != null)
            {
                wordList.Add(GetWordFromBoard(TilePlacement.Vertical, tempBoard, placedTile.X, GetFirstLetterIndex(TilePlacement.Vertical, tempBoard, placedTile.X, placedTile.Y)));
            }
            else if (tempBoard[placedTile.X, placedTile.Y + 1] != null)
            {
                wordList.Add(GetWordFromBoard(TilePlacement.Vertical, tempBoard, placedTile.X, placedTile.Y));
            }
        }
        if (orientation == TilePlacement.Vertical)
        {
            if (tempBoard[placedTile.X - 1, placedTile.Y] != null)
            {
                wordList.Add(GetWordFromBoard(TilePlacement.Horizontal, tempBoard, GetFirstLetterIndex(TilePlacement.Horizontal, tempBoard, placedTile.X, placedTile.Y), placedTile.Y));
            }
            else if (tempBoard[placedTile.X + 1, placedTile.Y] != null)
            {
                wordList.Add(GetWordFromBoard(TilePlacement.Horizontal, tempBoard, placedTile.X, placedTile.Y));
            }
        }

        return wordList;
    }

    public int GetFirstLetterIndex(TilePlacement orientation, string[,] board, int row, int col)
    {
        do
        {
            if (orientation == TilePlacement.Horizontal) row--;
            if (orientation == TilePlacement.Vertical) col--;
        } while (board[row, col] != null);
        return orientation == TilePlacement.Horizontal ? ++row : ++col;
    }

    public string GetWordFromBoard(TilePlacement orientation, string[,] board, int row, int col)
    {
        string newWord = string.Empty;
        while (board[row, col] != null)
        {
            newWord += (board[row, col]);
            if (orientation == TilePlacement.Horizontal) row++;
            if (orientation == TilePlacement.Vertical) col++;
        }
        return newWord;
    }

    public bool CheckEmptyBoard(string[,] valTiles)
    {
        for (int row = 0; row < valTiles.GetLength(0); row++)
        {
            for (int col = 0; col < valTiles.GetLength(1); col++)
            {
                if (valTiles[row, col] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
    /*
    public int CalculateScore(List<string> words)
    {
        if (words.Count == 0)
        {
            return 0;
        }

        int totalScore = 0;
        foreach (string word in words)
        {
            int wordScore = 0;
            foreach (char c in word)
            {
                wordScore += tileBag2.GetLetterValue(c);
            }
            totalScore += wordScore;
        }

        return totalScore;
    }*/

    public int CalculateScore(List<string> words)
    {
        if (words.Count == 0)
        {
            int score = 0;

            if (recordedPositions != null && recordedPositions.Count > 0)
            {
                
                foreach (string c in recordedPositions.Select(m => m.letter).ToList())
                {
                    score += TileBag2.GetLetterValue(c[0]);
                }

                return score;
            }
        }

            int totalScore = 0;
        foreach (string word in words)
        {
            /*if (word.EndsWith(","))
            {
                string currentWord = word.Substring(0, word.Length - 1); // Create a local variable

                int wordScore = 0;
                foreach (char c in currentWord.ToCharArray())
                {
                    wordScore += TileBag2.GetLetterValue(c);
                }

                totalScore += wordScore;
            }
            else
            {*/
                string currentWord = word; // Create a local variable

                int wordScore = 0;
                foreach (char c in currentWord.ToCharArray())
                {
                    wordScore += TileBag2.GetLetterValue(c);
                }

                totalScore += wordScore;

            //}
        }

        return totalScore;
    }


    
}

