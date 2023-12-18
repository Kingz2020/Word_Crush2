using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static EndTurnBtn;
//using static BoardScript;

public class EndTurnBtn : MonoBehaviour
{
    public TileBag2 tileBag; // Reference to your TileBag2 script

    //private List<ValidatedTiles> valTiles = new List<ValidatedTiles>();
    //public string[,] valTiles = new string[15, 15];
    

    BoardScript boardScript;
    PrintWords printWords;

    private void Start()
    {
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();
        printWords = GameObject.Find("Button_print_words").GetComponent<PrintWords>();
    }
    public class ValidatedTiles
    {
        public string letter;
        public Vector2Int currentPosition;
        

        public ValidatedTiles(string letter, Vector2Int currentPosition)
        {
            this.letter = letter;
            this.currentPosition = currentPosition;
        }
    }
    public void EndTurnButtonClicked()
    {
        // Clear the validated tiles list for a new turn
        // Validate the board for valid placement
        // Validate that all tiles form correct words
        string[,] valTiles = boardScript.valTiles;

        // Populate the validated tiles list with recorded positions
        //List<TileMove> recordedPositions = boardScript.GetRecordedPositions();

        foreach (string word in boardScript.CollectAllWords(boardScript.AllTilesInSameLine()))
        {
            if (!printWords.IsInDict(word))
            {
                Debug.Log("this is not a valid word :" + word);
                boardScript.RetrieveTilesFromBoard();
                
                return;
            }

        }
        Debug.Log("all words are valid");

        foreach (TileMove tileMove in boardScript.GetRecordedPositions())
        {
            //valTiles.Add(new ValidatedTiles(tileMove.letter, tileMove.recordedPosition));
            valTiles[tileMove.X, tileMove.Y] = tileMove.letter;
        }

        Debug.Log("all letters are stored");

        // Refill player's hand
        int currentTileCount = tileBag.GetCurrentTileCount();
        tileBag.RefillHandTiles(currentTileCount);


        //recordedPositions.Clear();

        boardScript.recordedPositions.Clear();
            

            // Update points
            // ... Implement logic to update player's points

            // Transfer moves to board array and/or history, chat, and clear the list
            // ... Implement logic to transfer moves to relevant data structures and clear the list

            // Pass turn to other player
            // ... Implement logic to pass turn to the next player
        }
    }
