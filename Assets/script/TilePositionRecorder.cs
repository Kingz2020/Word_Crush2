using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePositionRecorder : MonoBehaviour
{
    private List<Vector2Int> recordedPositions = new List<Vector2Int>();

    public void RecordTilePosition(Vector2Int position)
    {
        // Record the position of the placed tile.
        recordedPositions.Add(position);
    }

    public List<Vector2Int> GetRecordedPositions()
    {
        return recordedPositions;
    }

    public void DisplayRecordedPositions()
    {
        // Access the recorded positions and display them.
        foreach (var position in recordedPositions)
        {
            Debug.Log("Recorded Position: " + position.x + ", " + position.y);
        }
    }
}