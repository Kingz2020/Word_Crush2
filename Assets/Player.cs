using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public string playerName;
    public int playerTotalPoints;
    public int playerRoundPoints;
    public TileScript[,] lastPlayerBoard = new TileScript[15, 15];
    public List<TileMove> recordedPositions = new List<TileMove>();

    public void ResetPlayer()
    {
        playerRoundPoints = 0;
        playerTotalPoints = 0;
        lastPlayerBoard = new TileScript[15, 15];
        recordedPositions = new List<TileMove>();
    }

    public void CalculateNewPoints()
    {
        playerTotalPoints += playerRoundPoints;
    }

}
