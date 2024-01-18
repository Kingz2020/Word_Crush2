using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager: MonoBehaviour {

    [SerializeField] private TileBag _tileBag;
    [SerializeField] private BoardScript _boardScript;
    [SerializeField] private PlaceholderGenerator _generator;
    public UnityEvent OnTurnEnd;
    public List<string> players = new List<string>();
    public int turn;
    public List<TileScript> tilesForRound = new List<TileScript>();
    public TileScript[,] boardForRound = new TileScript[15, 15];
    public List<List<TileMove>> recordedPositions = new List<List<TileMove>>();
    private int currentRound = 1;

    private void Awake() {
        players.Add("Player 1");
        players.Add("Player 2");
        recordedPositions.Add(new List<TileMove>());
        recordedPositions.Add(new List<TileMove>());
    }

    public void ResetTurnManager() {
        turn = 0;
        currentRound = 1;
        boardForRound = new TileScript[15, 15];
        tilesForRound = new List<TileScript>();
    }

    public void EndTurn() {
        turn++;
        _tileBag.RetrieveAllTiles();
        if (currentRound < GetRoundNumber()) {
            foreach (var tile in recordedPositions[1]) {
                tilesForRound.Remove(tile.GetComponent<TileScript>());
            }
            RefillHandTiles(recordedPositions[1].Count);
            currentRound = GetRoundNumber();
            recordedPositions[0].Clear();
            recordedPositions[1].Clear();
            boardForRound = (TileScript[,]) _boardScript.valTiles.Clone();
        }
        _boardScript.placedTilePositions = (TileScript[,]) boardForRound.Clone();
        _boardScript.valTiles = (TileScript[,]) boardForRound.Clone();
        _boardScript.SetPlayerHandTiles(GetTilesForRound());
        _generator.RegenerateBoard(boardForRound);
        OnTurnEnd?.Invoke();
    }

    public void SetPlayersTurn(List<TileMove> moves) {
        recordedPositions[turn % players.Count].AddRange(moves);
    }

    public int GetRoundNumber() {
        return turn / players.Count + 1;
    }

    public List<TileScript> GetTilesForRound() {
        return tilesForRound;
    }
    
    public void RefillHandTiles(int tilesToDraw) {
        for (int amount = 1; amount <= tilesToDraw; amount++) {
            tilesForRound.Add(_tileBag.GetRandomLetterFromBag());
        }
    }

    public String GetActivePlayerName() {
        return players[turn % players.Count];
    }
}
