using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TileScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI textLetter;
    public TextMeshProUGUI textPoints;

    private string letter;
    private int points;
    private Vector3 initialPosition; // Store the initial position of the tile.
    private Vector2 dropPosition;
    private GameObject board; // Declare the board GameObject.


    private void Awake()
    {
        board = GameObject.Find("Board");

        if (board == null)
        {
            Debug.LogError("Board GameObject not found. Ensure it has the name 'Board'.");
        }
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsDroppedOnBoard(eventData.position))
        {
            // Tile was dropped on the board.
            Debug.Log("Dropped on the Board");

            // Store the drop position.
            dropPosition = transform.position;

            RectTransform boardRect = board.GetComponent<RectTransform>();

            PlaceholderGenerator gen = board.GetComponent<PlaceholderGenerator>();
            Vector2 cellSize = gen.cellsize;

            Debug.Log("Cell Size: " + cellSize);

            Vector3 localPosition = boardRect.InverseTransformPoint(dropPosition);

            localPosition.x = Mathf.Clamp((int)(localPosition.x - 50) / (int)cellSize.x * (int)cellSize.x + 50, 50, 960);
            localPosition.y = Mathf.Clamp((int)(localPosition.y + 50) / (int)cellSize.y * (int)cellSize.y - 50, -960, -50);




            transform.SetParent(board.transform);

            transform.localPosition = localPosition;

            GetComponent<RectTransform>().sizeDelta = cellSize;

            Debug.Log("Local Position:"  + localPosition);
        }
        else
        {
            Debug.Log("Dropped outside the Board");

            transform.position = initialPosition;
        }
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
}