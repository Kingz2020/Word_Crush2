using System;
using UnityEngine;

public class DebugFunctions: MonoBehaviour {
    
    [SerializeField] private BoardScript boardScript;
    [SerializeField] private TileBag tileBag;
    
    public void DisplayRecordedPositions() {
        foreach (var tileMove in boardScript.recordedPositions) {
            Debug.Log("Recorded Position: " + tileMove.X + ", " + tileMove.Y);
            Debug.Log("Letter: " + tileMove.gameObject.GetComponent<TileScript>().GetLetter());
        }
    }
    
    public void RetrieveTilesFromBoard() {
        tileBag.RetrieveAllTiles();
    }

    public void DrawTile() {
        //tileBag.AddTileToHand(tileBag.GetRandomLetterFromBag());
    }
    
    public void CheckSameLine() {
        Debug.Log(boardScript.AllTilesInSameLine());
    }
    
    public void PrintHandTiles() {
        Debug.Log("The following tiles are in the hand:");
        foreach (TileScript tile in tileBag.handTiles) {
            Debug.Log("Letter: " + tile.GetLetter() + ", Points: " + tile.GetPoints());
        }
    }
   
    public void NewGame() {
        boardScript.StartNewGame();
    }

    public void PrintWords() {
        foreach (var word in boardScript.CollectAllWords(boardScript.AllTilesInSameLine())) {
            string fullWord = String.Empty;
            foreach (var letter in word) {
                fullWord += letter.GetLetter();
            }
            Debug.Log("The placed word is: " + fullWord);
        }
    }

}
