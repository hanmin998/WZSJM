using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public int rowIndex;
    public int columnIndex;
    public GameController game;

    public float xOff = -2.25f;
    public float yOff = -3f;

    public bool movable = true;
    public void UpdatePosition() {
        Vector3 v = new Vector3(0f, 0f, 0f);
        v.x = 0.5f * columnIndex + xOff;
        if (rowIndex % 2 == 0) {
            v.x = 0.5f * columnIndex + xOff + 0.25f;
        }
        v.y = 0.5f * rowIndex + yOff;
        transform.position = v;
    }
    public void Goto(int rowIndex, int columnIndex) {
        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
        UpdatePosition();
    }
    public void OnMouseDown() {
        game.Select(this);
    }
}
