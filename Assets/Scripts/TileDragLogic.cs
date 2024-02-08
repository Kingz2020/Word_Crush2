using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileDragLogic: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    
    private Vector2 initialPosition;
    private TileMove initialMove = null;
    private Vector2 dropPosition;
    private BoardScript boardScript;
    //private HandTiles handTilesScript;

    private void Start() {
        boardScript = GameObject.Find("Board").GetComponent<BoardScript>();
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        TileMove move = GetComponent<TileMove>();
        initialMove = null;
        if (move.onBoard) {
            initialMove = move;
            boardScript.placedTilePositions[move.X, move.Y] = null;
            boardScript.recordedPositions.Remove(move);
        } 
        initialPosition = transform.position;
    }

    private void RevertMove()
    {
        if (initialMove != null) { 
        transform.position = initialPosition;
        boardScript.placedTilePositions[initialMove.X, initialMove.Y] = initialMove.GetComponent<TileScript>();
        boardScript.RecordTilePosition(initialMove);
    }
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

    public bool IsDroppedOnHandTiles(Vector2 dropPosition)
    {
        // Check if a handTileHolder object exists
        if (boardScript.handTileHolder != null)
        {
            // Get the RectTransform component of the handTileHolder
            RectTransform handTilesRect = boardScript.handTileHolder.GetComponent<RectTransform>();

            // Ensure the RectTransform exists
            if (handTilesRect != null)
            {
                // Convert the screen point to a point relative to the handTileHolder RectTransform
                Vector2 dropPositionInHandTiles = handTilesRect.InverseTransformPoint(dropPosition);

                // Check if the point is within the bounds of the handTileHolder
                return RectTransformUtility.RectangleContainsScreenPoint(handTilesRect, dropPositionInHandTiles);
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
                gameObject.name = "Tile" + gridX + "X" + gridY + "Y";


                TileMove move = GetComponent<TileMove>();
                move.SetTileMove(new Vector2Int(gridX, gridY));
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
            } else if (IsDroppedOnHandTiles(eventData.position))
            {
                // Replace existing swapping logic with revised approach:

                TileScript thisTile = GetComponent<TileScript>();
                Vector2 dropPositionInHandTiles = boardScript.handTileHolder.GetComponent<RectTransform>().InverseTransformPoint(dropPosition);

                TileScript otherTile = FindClosestTile(dropPositionInHandTiles);

                if (otherTile != null && thisTile != otherTile)
                {
                    // Use a temporary variable to hold the first tile's parent
                    Transform thisTileParent = thisTile.transform.parent;

                    // Update positions visually (already implemented in your code)
                    transform.SetParent(otherTile.transform.parent);
                    otherTile.transform.SetParent(thisTileParent);

                    // Iterate through handTileHolder children to find indices
                    int thisTileIndex = -1;
                    int otherTileIndex = -1;
                    for (int i = 0; i < boardScript.handTileHolder.transform.childCount; i++)
                    {
                        Transform child = boardScript.handTileHolder.transform.GetChild(i);
                        TileScript tile = child.GetComponent<TileScript>();

                        if (tile == thisTile)
                        {
                            thisTileIndex = i;
                        }
                        else if (tile == otherTile)
                        {
                            otherTileIndex = i;
                        }

                        if (thisTileIndex >= 0 && otherTileIndex >= 0)
                        {
                            break;
                        }
                    }
                    if (thisTileIndex >= 0 && otherTileIndex >= 0)
                    {
                        // Swap positions within handTileHolder based on stored indices
                        boardScript.handTileHolder.transform.GetChild(thisTileIndex).SetSiblingIndex(otherTileIndex);
                        boardScript.handTileHolder.transform.GetChild(otherTileIndex).SetSiblingIndex(thisTileIndex);                 
                    }
                }
            }
        }
        else {
                RevertMove();
            }

    }

    // Helper function to find the closest tile to a given position
    private TileScript FindClosestTile(Vector2 position)
    {
        float closestDistance = float.MaxValue;
        TileScript closestTile = null;

        // Iterate through children of handTileHolder
        foreach (Transform child in boardScript.handTileHolder.transform)
        {
            TileScript tile = child.GetComponent<TileScript>();
            if (tile != null && tile != this) // Exclude current tile
            {
                Vector2 tilePosition = child.localPosition;
                float distance = Vector2.Distance(position, tilePosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTile = tile;
                }
            }
        }

        return closestTile;
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
