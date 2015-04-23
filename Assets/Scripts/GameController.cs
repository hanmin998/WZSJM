using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject pot1;
    public GameObject pot2;
    public GameObject cat;
    

    public int columnNum = 9;
    public int rowNum = 9;

    private ArrayList potArr;
    private ArrayList pot2Arr;

    private bool started = false;
    private bool gameOver = false;

    public GameObject startScreen;
    public GameObject victory;
    public GameObject failed;
    public GameObject replay;
	void Start () {
        //set game controller ref
        startScreen.GetComponent<StartGame>().controller = this;
        replay.GetComponent<StartGame>().controller = this;
        //init pot2Arr
        pot2Arr = new ArrayList();
        potArr = new ArrayList();
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++) {
            ArrayList tmp = new ArrayList();
            for (int columnIndex = 0; columnIndex < columnNum; columnIndex++)
            {
                Item item = CreatePot(pot1, rowIndex, columnIndex);
                tmp.Add(item);
            }
            potArr.Add(tmp);
        }


            
	}

    public void StartGame() {
        //reset start state
        started = true;
        startScreen.SetActive(false);
        //reset game over state
        gameOver = false;
        failed.SetActive(false);
        victory.SetActive(false);
        replay.SetActive(false);
        //reset cat position
        cat.SetActive(true);
        //random cat position
        MoveCat(Random.Range(3, rowNum - 3), Random.Range(3, columnNum - 3));
        //set pot1 movable true
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < columnNum; columnIndex++)
            {
                Item item = GetPot(rowIndex, columnIndex);
                item.movable = true;
            }
        }
        //remove pot2
        for (int i = 0; i < pot2Arr.Count; i++) {
            Item pot2 = pot2Arr[i] as Item;
            Destroy(pot2.gameObject);
        }
        //clear pot2Arr
        pot2Arr = new ArrayList();
    }

    //get pot from potArr
    Item GetPot(int rowIndex, int columnIndex) {
        if (rowIndex < 0 || rowIndex > rowNum - 1 || columnIndex < 0 || columnIndex > columnNum - 1) {
            return null;
        }
        ArrayList tmp = potArr[rowIndex] as ArrayList;
        Item item = tmp[columnIndex] as Item;
        return item;
    }
    public void MoveCat(int rowIndex, int columnIndex) {
        Item item = cat.GetComponent<Item>();
        item.Goto(rowIndex, columnIndex);
    }

    public void Select(Item item) {
        if (!started || gameOver) return;
        if (item.movable == true) {
            Item pot = CreatePot(pot2, item.rowIndex, item.columnIndex);
            pot2Arr.Add(pot);
            item.movable = false;
            ArrayList steps = FindSteps();
            Debug.Log(steps.Count);
            if (steps.Count > 0)
            {
                int index = Random.Range(0, steps.Count);
                Vector2 v = (Vector2)steps[index];
                MoveCat((int)v.y, (int)v.x);
                if (Escaped()) {
                    gameOver = true;
                    failed.SetActive(true);
                    replay.SetActive(true);
                }
            }
            else {
                gameOver = true;
                victory.SetActive(true);
                replay.SetActive(true);
            }
        }
    }

    bool Movable(Vector2 v) {
        Item item = GetPot((int)v.y, (int)v.x);
        if (item == null) {
            return false;
        }
        return item.movable;
    }

    ArrayList FindSteps() {
        Item item = cat.GetComponent<Item>();
        int rowIndex = item.rowIndex;
        int columnIndex = item.columnIndex;

        ArrayList steps = new ArrayList();
        Vector2 v = new Vector2();
        //left
        v.y = rowIndex;
        v.x = columnIndex - 1;
        if (Movable(v)) {
            steps.Add(v);
        }
        //right
        v.y = rowIndex;
        v.x = columnIndex + 1;
        if (Movable(v))
        {
            steps.Add(v);
        }
        //top
        v.y = rowIndex + 1;
        v.x = columnIndex;
        if (Movable(v))
        {
            steps.Add(v);
        }
        //buttom
        v.y = rowIndex - 1;
        v.x = columnIndex;
        if (Movable(v))
        {
            steps.Add(v);
        }
        //奇数行 topLeft 偶数行 topRight
        v.y = rowIndex + 1;
        if (rowIndex % 2 == 1) {
            v.x = columnIndex - 1;
        }else 
            v.x = columnIndex + 1;
        if (Movable(v))
        {
            steps.Add(v);
        }
        //奇数行 buttomLeft 偶数行 buttomRight
        v.y = rowIndex - 1;
        if (rowIndex % 2 == 1)
        {
            v.x = columnIndex - 1;
        }
        else
            v.x = columnIndex + 1;
        if (Movable(v))
        {
            steps.Add(v);
        }
        return steps;
    }
    Item CreatePot(GameObject pot, int rowIndex, int columnIndex) {
        GameObject o = Instantiate(pot) as GameObject;
        o.transform.parent = this.transform;
        Item item = o.GetComponent<Item>();
        item.Goto(rowIndex, columnIndex);
        item.game = this;
        return item;
    }
    bool Escaped() {
        Item item = cat.GetComponent<Item>();
        int rowIndex = item.rowIndex;
        int columnIndex = item.columnIndex;
        if (rowIndex == 0 || rowIndex == rowNum - 1 || columnIndex == 0 || columnIndex == columnNum - 1)
        {
            return true;
        }
        return false;
    }
}
