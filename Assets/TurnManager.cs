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
    [SerializeField] private DisplayHandler _displayHandler;
    public GameObject playerGO;
    public UnityEvent OnTurnEnd;
    public List<Player> players = new List<Player>();
    public int turn;
    public List<TileScript> tilesForRound = new List<TileScript>();
    public TileScript[,] boardForRound = new TileScript[15, 15];
    //public List<List<TileMove>> recordedPositions = new List<List<TileMove>>();
    private int currentRound = 1;
    //private int roundsLeft = 3;
    public float turnTime { get; private set; } = 20f; // Property with default value

    //public float turnTime = 60f; // Duration of a turn in seconds
    public float timeRemaining;
    //private float timeRemaining = turnTime; // Time left in the current turn

    private void Awake() {
        players.Add(Instantiate(playerGO).GetComponent<Player>());
        players.Add(Instantiate(playerGO).GetComponent<Player>());
        players[0].playerName = "SuperMe";
        players[1].playerName = "Generic Opponent";
        timeRemaining = turnTime;
    }
    void Update()
    {   if (_boardScript.NewGameSet)
        {
            UpdateTimer(); // Update timer every frame
        }
    }
    public void ResetTurnManager() {
        turn = 0;
        currentRound = 1;
        boardForRound = new TileScript[15, 15];
        tilesForRound = new List<TileScript>();
        //timeRemaining = turnTime; // Reset timer for the first turn
    }

    public void EndTurn() {
        turn++;
        _tileBag.RetrieveAllTiles();

        
        if (currentRound < GetRoundNumber()) {
            int winningPlayer;

            if (players[1].playerRoundPoints > players[0].playerRoundPoints)
            {
                winningPlayer = 1;
                foreach (var tile in players[winningPlayer].recordedPositions)
                {
                    tilesForRound.Remove(tile.GetComponent<TileScript>());
                }
                RefillHandTiles(players[winningPlayer].recordedPositions.Count);
                currentRound = GetRoundNumber();
                players[0].recordedPositions.Clear();
                players[1].recordedPositions.Clear();
                boardForRound = (TileScript[,])players[winningPlayer].lastPlayerBoard.Clone();
                players[winningPlayer].CalculateNewPoints();
            }
            else if (players[1].playerRoundPoints < players[0].playerRoundPoints)
            {
                winningPlayer = 0;
                foreach (var tile in players[winningPlayer].recordedPositions)
                {
                    tilesForRound.Remove(tile.GetComponent<TileScript>());
                }
                RefillHandTiles(players[winningPlayer].recordedPositions.Count);
                currentRound = GetRoundNumber();
                players[0].recordedPositions.Clear();
                players[1].recordedPositions.Clear();
                boardForRound = (TileScript[,])players[winningPlayer].lastPlayerBoard.Clone();
                players[winningPlayer].CalculateNewPoints();
            }
            else
            {
                // Both players have the same points, consider them both winners
                winningPlayer = -1; // Use a sentinel value to indicate both winner
                // use the first player to have those points                
                foreach (var tile in players[0].recordedPositions)
                {
                    tilesForRound.Remove(tile.GetComponent<TileScript>());
                }
                RefillHandTiles(players[0].recordedPositions.Count);
                currentRound = GetRoundNumber();
                players[0].recordedPositions.Clear();
                players[1].recordedPositions.Clear();
                boardForRound = (TileScript[,])players[0].lastPlayerBoard.Clone();
                players[0].CalculateNewPoints();
                players[1].CalculateNewPoints();
            }
            
        }

        _displayHandler.score_0.text = players[0].playerTotalPoints.ToString();
        _displayHandler.score_1.text = players[1].playerTotalPoints.ToString();
        _displayHandler.roundsLeft.text = currentRound.ToString();
        _boardScript.placedTilePositions = (TileScript[,])boardForRound.Clone();
        _boardScript.valTiles = (TileScript[,])boardForRound.Clone();
        _boardScript.SetPlayerHandTiles(GetTilesForRound());
        _generator.RegenerateBoard(boardForRound);
        OnTurnEnd?.Invoke();

        // Start the timer for the next turn
        timeRemaining = turnTime;
        UpdateTimer(); // Now call this to start the countdown

    }

    public void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        // Display remaining time on UI (replace with your UI element)
        _displayHandler.timerText.text = timeRemaining.ToString("0");

        // If time runs out, end the turn
        if (timeRemaining <= 0) {
            EndTurn(); // Forcefully end the turn
        }
    }

    public void SetPlayersTurn(List<TileMove> moves) {
        players[turn % players.Count].recordedPositions.AddRange(moves);
    }
    
    public void SetPlayersBoard(TileScript[,] board) {
        players[turn % players.Count].lastPlayerBoard = (TileScript[,] ) board.Clone();
    }

    public int GetRoundNumber() {
        // Calculate total rounds based on players
        //int totalRounds = Mathf.CeilToInt((float)players.Count);

        // Calculate rounds left based on total rounds and current round
        //roundsLeft = roundsLeft - currentRound;

        //return currentRound; // Return actual round number


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
