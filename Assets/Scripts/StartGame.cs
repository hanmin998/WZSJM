using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    public GameController controller;
    void OnMouseDown() {
        controller.StartGame();
    }
}
