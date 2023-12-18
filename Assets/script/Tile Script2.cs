using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class TileScript2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI textLetter;
    public TextMeshProUGUI textPoints;
    public TextMeshProUGUI textPointsHolder;
    //public TextMeshProUGUI textScore;

    private string letter;
    private int points;
    public GameObject pointHolder;
    private Vector3 initialPosition; // Store the initial position of the tile.
    private Vector2 dropPosition;
    private GameObject board; // Declare the board GameObject.

    private bool[,] placedTilePositions;
    //private List<Vector3> localPositions = new List<Vector3>();
    private List<Vector3> localPositions = new List<Vector3>() { };
    GameObject Button_get_tiles_placed;
    BoardScript boardScript;
    TileBag2 tileBag2Script;
    PrintWords printWords;

    int gridSizeX = 15; // Set your grid size X dimension here (e.g., 15).
    int gridSizeY = 15; // Set your grid size Y dimension here (e.g., 15).
    int score =0;
    

    private void Awake()
    {
        Debug.Log(gameObject.name);

        tileBag2Script = GameObject.FindObjectOfType<TileBag2>();

        board = GameObject.Find("Board");



        //printWords

        placedTilePositions = new bool[gridSizeX, gridSizeY]; // Initialize the grid array.

        //tileRecorder = board.GetComponent<board_script.RecordTilePosition>();
        boardScript = board.GetComponent<BoardScript>();

        boardScript.hidePointTiles.AddListener(HideRoundPoints);

        if (board == null)
        {
            Debug.LogError("Board GameObject not found. Ensure it has the name 'Board'.");
        }
    }
    private void HideRoundPoints()
    {
        pointHolder.SetActive(false);
    }


    public void InitTile(string letter, int points)
    {
        this.letter = letter;
        this.points = points;

        textLetter.text = letter;
        textPoints.text = points.ToString();
    }

    public string GetLetter()
    {
        return letter;
    }

    public int GetPoints()
    {
        return points;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = transform.position;
    }

    public void OnDragStart(PointerEventData eventData)
    {
        // Check if the tile is currently placed on the board
        if (IsDroppedOnBoard(eventData.position))
        {
            // Get the tile's unique identifier
            string tileIdentifier = gameObject.name;

            // Remove the tile's coordinates from the recordedPositions list .name
            //boardScript.recordedPositions = null;
        }
    }

    public void RemoveTilePosition(string tileIdentifier)
    {
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsDroppedOnBoard(eventData.position))
        {
            dropPosition = transform.position;
            RectTransform boardRect = board.GetComponent<RectTransform>();
            PlaceholderGenerator gen = board.GetComponent<PlaceholderGenerator>();
            Vector2 cellSize = gen.cellsize;

            Vector3 localPosition = boardRect.InverseTransformPoint(dropPosition);

            int gridSizeX = 15; // Define the grid size in the X-axis (assuming it's 15).
            int gridSizeY = 15; // Define the grid size in the Y-axis (assuming it's 15).

            int gridX =  Mathf.Clamp((int)(localPosition.x - 50) / (int)cellSize.x, 0, gridSizeX - 1);
            int gridY = 14 - Mathf.Clamp((int)((localPosition.y + 960) / cellSize.y), 0, gridSizeY - 1);

            Debug.Log("Grid X: " + gridX);
            Debug.Log("Grid Y: " + gridY);
            Debug.Log("Local Position: " + localPosition);

            if (CheckAndStoreAvailability(gridX, gridY))
            {
                localPosition.x = Mathf.Clamp((int)(localPosition.x - 50) / (int)cellSize.x * (int)cellSize.x + 50, 50, 960);
                localPosition.y = Mathf.Clamp((int)(localPosition.y + 960) / (int)cellSize.y * cellSize.y - 960, -960, -50);

                localPositions.Add(localPosition);


                transform.SetParent(board.transform);
                transform.localPosition = localPosition;
                GetComponent<RectTransform>().sizeDelta = cellSize;
                
                // Rename the tile's GameObject with its grid position coordinates
                gameObject.name = "Tile" + gridY + "X" + gridX + "Y";


                //boardScript.RecordTilePosition(new Vector2Int(gridX, gridY));
                // Notify the centralized TilePositionRecorder about the new position
                boardScript.RecordTilePosition(new TileMove(new Vector2Int(gridY, gridX), textLetter.text));

                //calculate the points here
                int score = boardScript.CalculateScore(boardScript.CollectAllWords(boardScript.AllTilesInSameLine()));

                SetScore(score);

                pointHolder.SetActive(true);

                TileBag2 tileBag = GameObject.FindObjectOfType<TileBag2>();
                if (tileBag != null)
                {
                    // Remove the tile from the hand when it's placed on the board
                    tileBag.handTiles.Remove(gameObject); // The gameObject represents the tile being dragged
                    //tileBag.RefillHandTiles(tileBag.GetCurrentTileCount());
                }

            }
            else
            {
                transform.position = initialPosition;
            }
        }
        else
        {
            transform.position = initialPosition;
        }
    }

    public List<Vector3> GetLocalPositions()
    {
        return localPositions;
    }
    private bool IsDroppedOnBoard(Vector2 dropPosition)
    {
        if (board != null)
        {
            RectTransform boardRect = board.GetComponent<RectTransform>();
            if (boardRect != null)
            {
                return RectTransformUtility.RectangleContainsScreenPoint(boardRect, dropPosition);
            }
        }

        return false;
    }

    private bool CheckAndStoreAvailability(int x, int y)
    {
        // Check if the grid cell is available (you need to implement this).
        if (IsGridCellAvailable(x, y))
        {
            // Store the grid position for this tile.
            //placedTilePositions[x, y] = transform;
            //Debug.Log("placedTilePositions: " + placedTilePositions[x, y]);
            placedTilePositions[x, y] = true;
            Debug.Log("placedTilePositions: " + x + ", " + y);
            return true; // Cell is available
        }

        return false; // Cell is occupied
    }

    private bool IsGridCellAvailable(int x, int y)
    {
        // Implement your logic to check if the grid cell at coordinates (x, y) is available.
        // You can use the placedTilePositions array or any other data structure you prefer to track occupied cells.

        // Example: Check if the grid cell is occupied based on the placedTilePositions array.
        // Assuming placedTilePositions is a two-dimensional array of bools.

        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            return !placedTilePositions[x, y];
        }

        return false;
    }

    public Vector2Int GetGridPosition()
    {
        // Get the cell size of your grid
        PlaceholderGenerator gen = board.GetComponent<PlaceholderGenerator>();
        Vector2 cellSize = gen.cellsize;

        RectTransform boardRect = board.GetComponent<RectTransform>();
        Vector3 localPosition = boardRect.InverseTransformPoint(transform.position);

        // Calculate the grid positions using the local position and cell size
        int gridX = Mathf.Clamp((int)(localPosition.x - 50) / (int)cellSize.x, 0, gridSizeX - 1);
        int gridY = Mathf.Clamp((int)(localPosition.y + 960) / (int)cellSize.y, 0, gridSizeY - 1);

        return new Vector2Int(gridX, gridY);
    }

    public void SetScore(int score)
    {
        //textScore.text = score.ToString();
        boardScript.HideAllPointTiles();

        textPointsHolder.text = score.ToString();

        
    }


}

