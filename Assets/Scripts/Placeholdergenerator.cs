using UnityEngine;
using UnityEngine.UI;

public class PlaceholderGenerator : MonoBehaviour
{
    public GameObject blankoPrefab; // The prefab for the placeholders.
    public GameObject board; // Reference to the board GameObject.
    public int rows = 15; // Number of rows in the grid.
    public int columns = 15; // Number of columns in the grid;
    public Vector2 offset = new Vector2(50, -50); // Offset for the frame or margin.
    public Vector2 cellsize;

    void Start()
    {
        // Calculate the size of each cell in the grid.
        RectTransform boardRect = board.GetComponent<RectTransform>();
        cellsize = new Vector2(
            (boardRect.rect.width - offset.x * 2) / columns,
            (boardRect.rect.height - offset.y * 2) / rows
        );

        // Generate the placeholders.
        GeneratePlaceholders();
    }

    void GeneratePlaceholders() {
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                // Calculate the position of each placeholder relative to the board, including the offset.
                Vector3 position = new Vector3(
                     offset.x + (col * cellsize.x),
                    - offset.y - (row * cellsize.y ),
                    0);

                // Instantiate a placeholder using the "Blanko" prefab at the calculated position.
                //GameObject placeholder = Instantiate(blankoPrefab, position, Quaternion.identity);
                GameObject placeholder = Instantiate(blankoPrefab, transform);

                // Parent the placeholder to the board.
                //placeholder.transform.SetParent(board.transform);
                placeholder.transform.localPosition = position;
                placeholder.transform.localEulerAngles = new Vector3();
                // Check for the special case of [7, 7] and change the color of the blankoPrefab.
                if (row == 7 && col == 7)
                {
                    placeholder.GetComponentInChildren<Image>().color = Color.cyan;
                }
                placeholder.GetComponent<RectTransform>().sizeDelta = cellsize;
                
               
            }
        }
    }

    public void RegenerateBoard(TileScript[,] boardForRound) {
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < columns; col++) {
                // Calculate the position of each placeholder relative to the board, including the offset.
                Vector3 position = new Vector3(
                    offset.x + (col * cellsize.x),
                    -offset.y - (row * cellsize.y),
                    0);
                if (boardForRound[row, col] != null) {
                    var temp = boardForRound[row, col];
                    temp.gameObject.transform.SetParent(board.transform);
                    temp.gameObject.transform.localPosition = position;
                    temp.gameObject.SetActive(true);
                }
            }
        }
    }
}