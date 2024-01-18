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
    public GameObject playerGO;
    public UnityEvent OnTurnEnd;
    public List<Player> players = new List<Player>();
    public int turn;
    public List<TileScript> tilesForRound = new List<TileScript>();
    public TileScript[,] boardForRound = new TileScript[15, 15];
    //public List<List<TileMove>> recordedPositions = new List<List<TileMove>>();
    private int currentRound = 1;

    private void Awake() {
        players.Add(Instantiate(playerGO).GetComponent<Player>());
        players.Add(Instantiate(playerGO).GetComponent<Player>());
        players[0].playerName = "SuperMe";
        players[1].playerName = "Generic Opponent";
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
            int winningPlayer = 1;
            foreach (var tile in players[winningPlayer].recordedPositions) {
                tilesForRound.Remove(tile.GetComponent<TileScript>());
            }
            RefillHandTiles(players[winningPlayer].recordedPositions.Count);
            currentRound = GetRoundNumber();
            players[0].recordedPositions.Clear();
            players[1].recordedPositions.Clear();
            boardForRound = (TileScript[,]) players[winningPlayer].lastPlayerBoard.Clone();
            players[winningPlayer].CalculateNewPoints();
        }
        _boardScript.placedTilePositions = (TileScript[,]) boardForRound.Clone();
        _boardScript.valTiles = (TileScript[,]) boardForRound.Clone();
        _boardScript.SetPlayerHandTiles(GetTilesForRound());
        _generator.RegenerateBoard(boardForRound);
        OnTurnEnd?.Invoke();
    }

    public void SetPlayersTurn(List<TileMove> moves) {
        players[turn % players.Count].recordedPositions.AddRange(moves);
    }
    
    public void SetPlayersBoard(TileScript[,] board) {
        players[turn % players.Count].lastPlayerBoard = (TileScript[,] ) board.Clone();
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
        return players[turn % players.Count].playerName;
    }

    public void AddPlayerPoints(int points) {
        players[turn % players.Count].playerRoundPoints = points;
    }

    public int GetActivePlayerPoints() {
        return players[turn % players.Count].playerTotalPoints;
    }
}
