using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class AdjustBoardScaling : MonoBehaviour
    {
        // Reference to the Grid Layout Group
        private GridLayoutGroup gridLayoutGroup;

        void Start()
        {
            // Get the reference to the Grid Layout Group component on this GameObject
            gridLayoutGroup = GetComponent<GridLayoutGroup>();

            if (gridLayoutGroup == null)
            {
                Debug.LogError("No GridLayoutGroup component found on this GameObject.");
                return;
            }

            // Debug log to check Constraint Count and Cell Size
            Debug.Log("Constraint Count: " + gridLayoutGroup.constraintCount);
            Debug.Log("Cell Size: " + gridLayoutGroup.cellSize);
            Debug.Log("gridWidth: " + gridLayoutGroup.constraintCount);
            Debug.Log("height-> ceil: " + Mathf.CeilToInt(transform.GetComponent<RectTransform>().rect.height));
            Debug.Log("height-> constraintCount: " + (float)gridLayoutGroup.constraintCount);

        // Calculate the new scale based on Constraint Count and Cell Size
        int gridWidth = gridLayoutGroup.constraintCount;
            //int gridHeight = Mathf.CeilToInt(gridLayoutGroup.transform.childCount / (float)gridLayoutGroup.constraintCount);
            int gridHeight = Mathf.CeilToInt(transform.GetComponent<RectTransform>().rect.height / (float)gridLayoutGroup.constraintCount);

        Vector2 cellSize = gridLayoutGroup.cellSize;

            Debug.Log("Grid Width: " + gridWidth);
            Debug.Log("Grid Height: " + gridHeight);
            Debug.Log("Cell Size: " + cellSize);

            float currentWidth = transform.localScale.x;
            float currentHeight = transform.localScale.y;

            float newScaleX = (gridWidth * cellSize.x) / currentWidth;
            float newScaleY = (gridHeight * cellSize.y) / currentHeight;

            Debug.Log("New Scale X: " + newScaleX);
            Debug.Log("New Scale Y: " + newScaleY);

            // Set the new scale for the board
            transform.localScale = new Vector3(newScaleX, newScaleY, transform.localScale.z);
        }
    }
