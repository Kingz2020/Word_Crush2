using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager: MonoBehaviour {

    [SerializeField] private TileBag _tileBag;
    [SerializeField] private BoardScript _boardScript;
    public UnityEvent OnTurnEnd;
    public List<string> players = new List<string>();
    public int turn;
    public List<TileScript> tilesForRound = new List<TileScript>();
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
    }

    public void EndTurn() {
        turn++;
        if (currentRound < GetRoundNumber()) {
            RefillHandTiles(recordedPositions[0].Count);
            currentRound = GetRoundNumber();
            recordedPositions[0].Clear();
            recordedPositions[1].Clear();
        }
        OnTurnEnd.Invoke();
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
