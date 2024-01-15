using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static PrintWords;

public class BoardScript : MonoBehaviour {
    
    public TextMeshProUGUI textScore;
    public GameObject handTileHolder;
    TileBag _tileBag;
    TileScript _tileScript;
    [SerializeField] private PrintWords printWords;
    [SerializeField] private TurnManager _turnManager;
    public UnityEvent hidePointTiles;
    public List<TileMove> recordedPositions = new List<TileMove>();
    public TileScript[,] valTiles = new TileScript[15, 15];
    public TileScript[,] placedTilePositions = new TileScript[15, 15];
    public int currentScore;
    public int gridSizeX = 15; // Set your grid size X dimension here (e.g., 15).
    public int gridSizeY = 15; // Set your grid size Y dimension here (e.g., 15).
    
    void OnDrawGizmos() {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    public void EndTurn() {
        // Clear the validated tiles list for a new turn
        // Validate the board for valid placement
        // Validate that all tiles form correct words

        // Populate the validated tiles list with recorded positions
        //List<TileMove> recordedPositions = boardScript.GetRecordedPositions();

        foreach (List<TileScript> word in CollectAllWords(AllTilesInSameLine())) {
            string assembledWord = String.Empty;
            foreach (var letter in word) {
                assembledWord += letter.GetLetter();
            }
            
            if (!printWords.IsInDict(assembledWord)) {
                Debug.Log("this is not a valid word :" + assembledWord);
                RetrieveTilesFromBoard();
                return;
            }

        }
        Debug.Log("all words are valid");

        foreach (TileMove tileMove in GetRecordedPositions()) {
            valTiles[tileMove.X, tileMove.Y] = tileMove.GetComponent<TileScript>();
        }

        Debug.Log("all letters are stored");
        
        SetPlayerHandTiles(_turnManager.GetTilesForRound());
        _turnManager.SetPlayersTurn(recordedPositions);
        recordedPositions.Clear();
            

        // Update points
        // ... Implement logic to update player's points

        // Transfer moves to board array and/or history, chat, and clear the list
        // ... Implement logic to transfer moves to relevant data structures and clear the list

        // Pass turn to other player
        // ... Implement logic to pass turn to the next player
    }

    public void HideAllPointTiles() {
         hidePointTiles?.Invoke();
    }
    private void Start() {
        SetPlayerHandTiles(_turnManager.GetTilesForRound());
        //tileScript2 = GameObject.Find("New Basic Tile").GetComponent<TileScript2>();
    }

    private void Awake() {
        _tileBag = GameObject.Find("TileBag").GetComponent<TileBag>();
    }

    public void RecordTilePosition(TileMove tileMove) {
        // Record the position of the placed tile.
        recordedPositions.Add(tileMove);
    }

    public List<TileMove> GetRecordedPositions() {
        return recordedPositions;
    }
    
    public void SetPlayerHandTiles(List<TileScript> tiles) {
        foreach (TileScript tile in _turnManager.GetTilesForRound()) {
           AddTileToHand(tile);
        }
    }

    private void AddTileToHand(TileScript tile) {
        tile.gameObject.SetActive(true);
        tile.transform.SetParent(handTileHolder.transform);
    }

    public TilePlacement AllTilesInSameLine() {
        //string[,] valTiles = valTiles;
        List<TileMove> thisTurn = GetRecordedPositions();
        if (thisTurn.Count == 0) return TilePlacement.NoTilePlaced;
        if (thisTurn.Count == 1) return TilePlacement.SingleTile;
        TileMove checkTile = thisTurn[0];
        bool vertical = true;
        bool horizontal = true;
        foreach (var pos in thisTurn) {
            if (pos.X != checkTile.X) horizontal = false;
            if (pos.Y != checkTile.Y) vertical = false;
        }
        if (vertical) return TilePlacement.Vertical;
        if (horizontal) return TilePlacement.Horizontal;
        return TilePlacement.WrongTilePlacement;
    }

    public void RetrieveTilesFromBoard() {
        foreach (TileMove position in recordedPositions) {
            position.onBoard = false;
            AddTileToHand(position.GetComponent<TileScript>());
        }
        recordedPositions.Clear();
        placedTilePositions = new TileScript[15, 15];
    }
    
    public List<List<TileScript>> CollectAllWords(TilePlacement orientation) {
        List<List<TileScript>> wordList = new List<List<TileScript>>();
        if (orientation == TilePlacement.NoTilePlaced) return wordList;
        TileScript[,] tempBoard = (TileScript[,]) valTiles.Clone();

        foreach (var tempTile in GetRecordedPositions()) {
            tempBoard[tempTile.X, tempTile.Y] = tempTile.GetComponent<TileScript>();
        }

        if (orientation == TilePlacement.SingleTile) {
            var singleTile = GetRecordedPositions()[0];
            if( tempBoard[singleTile.X - 1, singleTile.Y] == null
                && tempBoard[singleTile.X, singleTile.Y - 1] == null
                && tempBoard[singleTile.X + 1, singleTile.Y] == null
                && tempBoard[singleTile.X, singleTile.Y + 1] == null) {
                List<TileScript> tempList = new List<TileScript>();
                tempList.Add(singleTile.GetComponent<TileScript>());
                wordList.Add(tempList);
                return wordList;
            }
        }

        foreach (var tempTile in GetRecordedPositions()) {
            if (orientation == TilePlacement.Horizontal || orientation == TilePlacement.SingleTile) {
                if (tempBoard[tempTile.X - 1, tempTile.Y] != null) {
                    wordList.Add(GetWordFromBoard(TilePlacement.Horizontal, tempBoard, GetFirstLetterIndex(TilePlacement.Horizontal, tempBoard, tempTile.X, tempTile.Y), tempTile.Y));
                } else if (tempBoard[tempTile.X + 1, tempTile.Y] != null) {
                    wordList.Add(GetWordFromBoard(TilePlacement.Horizontal, tempBoard, tempTile.X, tempTile.Y));
                }
            }
            if (orientation == TilePlacement.Vertical || orientation == TilePlacement.SingleTile) {
                if (tempBoard[tempTile.X, tempTile.Y - 1] != null) {
                    wordList.Add(GetWordFromBoard(TilePlacement.Vertical, tempBoard, tempTile.X, GetFirstLetterIndex(TilePlacement.Vertical, tempBoard, tempTile.X, tempTile.Y)));
                } else if (tempBoard[tempTile.X, tempTile.Y + 1] != null) {
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

    public int GetFirstLetterIndex(TilePlacement orientation, TileScript[,] board, int row, int col) {
        do {
            if (orientation == TilePlacement.Horizontal) row--;
            if (orientation == TilePlacement.Vertical) col--;
        } while (board[row, col] != null);
        return orientation == TilePlacement.Horizontal ? ++row : ++col;
    }

    public List<TileScript> GetWordFromBoard(TilePlacement orientation, TileScript[,] board, int row, int col) {
        List<TileScript> newWord = new List<TileScript>();
        while (board[row, col] != null) {
            newWord.Add(board[row, col]);
            if (orientation == TilePlacement.Horizontal) row++;
            if (orientation == TilePlacement.Vertical) col++;
        }
        return newWord;
    }

    public bool CheckEmptyBoard(TileScript[,] valTiles) {
        for (int row = 0; row < valTiles.GetLength(0); row++) {
            for (int col = 0; col < valTiles.GetLength(1); col++) {
                if (valTiles[row, col] != null) {
                    return false;
                }
            }
        }
        return true;
    }
    
    public int CalculateScore(List<List<TileScript>> words) {
        int totalScore = 0;
        foreach (List<TileScript> word in words) {
            foreach (var tile in word) {
                totalScore += tile.GetPoints();    
            }
        }
        return totalScore;
    }
}

