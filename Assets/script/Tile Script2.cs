using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileScript2: MonoBehaviour {
    
    public TextMeshProUGUI textLetter;
    public TextMeshProUGUI textPoints;
    public TextMeshProUGUI textPointsHolder;
    private string letter;
    private int points;
    public GameObject pointHolder;
    private List<Vector3> localPositions = new List<Vector3>() { };
    
    BoardScript boardScript;
    PrintWords printWords;

    private void Awake() {
        boardScript = FindObjectOfType<BoardScript>();
        boardScript.hidePointTiles.AddListener(HideRoundPoints);
    }
    private void HideRoundPoints() {
        pointHolder.SetActive(false);
    }

    public void InitTile(string letter, int points) {
        this.letter = letter;
        this.points = points;

        textLetter.text = letter;
        textPoints.text = points.ToString();
    }

    public string GetLetter() {
        return letter;
    }

    public int GetPoints() {
        return points;
    }

    public void RemoveTilePosition(string tileIdentifier) {
        // Extract the coordinates from the tile identifier
        string[] parts = tileIdentifier.Split('X');
        int gridX = int.Parse(parts[1]);
        int gridY = int.Parse(parts[2].Replace("Y", ""));

        // Remove the specific coordinates from the recordedPositions list
        //int index = boardScript.recordedPositions.FindIndex(x => x.recordedPosition == new Vector2Int(gridX, gridY));
        int index = boardScript.recordedPositions.FindIndex(x => x.X == gridX && x.Y == gridY);
        if (index != -1)
        {
            boardScript.recordedPositions.RemoveAt(index);
        }
    }

    public List<Vector3> GetLocalPositions() {
        return localPositions;
    }

    public void SetScore(int score) {
        boardScript.HideAllPointTiles();
        textPointsHolder.text = score.ToString();
    }


}

