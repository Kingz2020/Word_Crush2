using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileDragLogic: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    
    private Vector2 initialPosition;
    private Vector2 dropPosition;
    private BoardScript boardScript;

    private void Start() {
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        TileMove move = GetComponent<TileMove>(); 
        if (move.onBoard) {
            boardScript.placedTilePositions[move.X, move.Y] = null;
        }
        initialPosition = transform.position;
    }
    
    private bool IsDroppedOnBoard(Vector2 dropPosition) {
        if (boardScript != null) {
            RectTransform boardRect = boardScript.GetComponent<RectTransform>();
            if (boardRect != null) {
                return RectTransformUtility.RectangleContainsScreenPoint(boardRect, dropPosition);
            }
        }
        return false;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (IsDroppedOnBoard(eventData.position)) {
            dropPosition = transform.position;
            RectTransform boardRect = boardScript.GetComponent<RectTransform>();
            PlaceholderGenerator gen = boardScript.GetComponent<PlaceholderGenerator>();
            Vector2 cellSize = gen.cellsize;

            Vector2 localPosition = boardRect.InverseTransformPoint(dropPosition);

            int gridSizeX = 15; // Define the grid size in the X-axis (assuming it's 15).
            int gridSizeY = 15; // Define the grid size in the Y-axis (assuming it's 15).

            int gridX =  Mathf.Clamp((int)(localPosition.x - 50) / (int) cellSize.x, 0, gridSizeX - 1);
            int gridY =  Mathf.Clamp((int)(localPosition.y * - 1 - 50 ) / (int) cellSize.y, 0, gridSizeY - 1);

            Debug.Log("Grid X: " + gridX);
            Debug.Log("Grid Y: " + gridY);
            Debug.Log("Local Position: " + localPosition);

            if (CheckAndStoreAvailability(gridX, gridY)) {
                localPosition.x = 50 + gridX * cellSize.x;
                localPosition.y = -50 - gridY * cellSize.y;

                transform.SetParent(boardScript.transform);
                transform.localPosition = localPosition;
                GetComponent<RectTransform>().sizeDelta = cellSize;
                
                // Rename the tile's GameObject with its grid position coordinates
                gameObject.name = "Tile" + gridY + "X" + gridX + "Y";


                //boardScript.RecordTilePosition(new Vector2Int(gridX, gridY));
                // Notify the centralized TilePositionRecorder about the new position

                TileMove move = GetComponent<TileMove>();
                move.SetTileMove(new Vector2Int(gridY, gridX));
                boardScript.RecordTilePosition(move);

                //calculate the points here
                int score = boardScript.CalculateScore(boardScript.CollectAllWords(boardScript.AllTilesInSameLine()));

                GetComponent<TileScript>().SetScore(score);
                GetComponent<TileScript>().pointHolder.SetActive(true);

                TileBag tileBag = FindObjectOfType<TileBag>();
                if (tileBag != null) {
                    // Remove the tile from the hand when it's placed on the board
                    tileBag.handTiles.Remove(GetComponent<TileScript>()); 
                }
            } else {
                transform.position = initialPosition;
            }
        } else {
            transform.position = initialPosition;
        }
    }
    
    public Vector2Int GetGridPosition()
    {
        // Get the cell size of your grid
        PlaceholderGenerator gen = boardScript.GetComponent<PlaceholderGenerator>();
        Vector2 cellSize = gen.cellsize;

        RectTransform boardRect = boardScript.GetComponent<RectTransform>();
        Vector3 localPosition = boardRect.InverseTransformPoint(transform.position);

        // Calculate the grid positions using the local position and cell size
        int gridX = Mathf.Clamp((int)(localPosition.x - 50) / (int)cellSize.x, 0, boardScript.gridSizeX - 1);
        int gridY = Mathf.Clamp((int)(localPosition.y + 960) / (int)cellSize.y, 0, boardScript.gridSizeY - 1);

        return new Vector2Int(gridX, gridY);
    }
    
    private bool CheckAndStoreAvailability(int x, int y) {
        // Check if the grid cell is available (you need to implement this).
        if (IsGridCellAvailable(x, y))
        {
            // Store the grid position for this tile.
            boardScript.placedTilePositions[x, y] = GetComponent<TileScript>();
            Debug.Log("placedTilePositions: " + x + ", " + y);
            return true; // Cell is available
        }

        return false; // Cell is occupied
    }
    
    private bool IsGridCellAvailable(int x, int y) {
        // Implement your logic to check if the grid cell at coordinates (x, y) is available.
        // You can use the placedTilePositions array or any other data structure you prefer to track occupied cells.

        // Example: Check if the grid cell is occupied based on the placedTilePositions array.
        // Assuming placedTilePositions is a two-dimensional array of bools.

        if (x >= 0 && x < boardScript.gridSizeX && y >= 0 && y < boardScript.gridSizeY)
        {
            return boardScript.placedTilePositions[x, y] == null;
        }

        return false;
    }
    
}
