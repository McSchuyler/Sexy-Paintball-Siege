using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetsGenerator : MonoBehaviour
{
    [Header("Game Depth")]
    public int row;
    public int column;

    [Header("Game Colors")]
    public List<ColorSelection> colorsList;

    [Header("Layout")]
    public float distance;
    public GameObject location;
    public GameObject button;

    [Header("Score")]
    public int targetDestroy;
    public List<Vector2> moveList;
    public int combo;

    public Target[,] targetsObject;
    public static TargetsGenerator instance;

    public PaintballHolder paintballHolder;
    public Boss boss;
    public Player player;
    public int eachTargetDamage = 10;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        if (colorsList.Count > 0)
        {
            for (int i = 0; i < colorsList.Count; i++)
            {
                colorsList[i].isAllowed = true;
            }
        }

        targetsObject = new Target[row, column];

        GenerateTarget();

        player = FindObjectOfType<Player>();
        boss = FindObjectOfType<Boss>();
        paintballHolder = FindObjectOfType<PaintballHolder>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(GetRandomColor());
        }
	}

    public void Restart()
    {
        foreach (Target item in targetsObject)
        {
            Destroy(item.targetObject);
            item.color = "";
            item.down = 0;
            item.left = 0;
        }

        GenerateTarget();


    }

    //function to generate target
    void GenerateTarget()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                CheckDisableColor(i, j);
                int index = GetRandomColor();
                Target ta = new Target();
                ta.color = colorsList[index].color;

                if (j > 0)
                {
                    if (ta.color == targetsObject[i, j - 1].color)
                    {
                        ta.left = targetsObject[i, j - 1].left + 1;
                    }
                    else
                    {
                        ta.left = 1;
                    }
                }
                else
                {
                    ta.left = 1;
                }

                if (i > 0)
                {
                    if (ta.color == targetsObject[i - 1, j].color)
                    {
                        ta.down = targetsObject[i - 1, j].down + 1;
                    }
                    else
                    {
                        ta.down = 1;
                    }
                }
                else
                {
                    ta.down = 1;
                }

                targetsObject[i, j] = ta;
                Debug.Log("color in " + i + " , " + j + " is " + targetsObject[i, j].color);

                //create object
                GameObject prefab = Instantiate(colorsList[index].prefabs, location.transform);
                prefab.GetComponent<TargetHolder>().x = i;
                prefab.GetComponent<TargetHolder>().y = j;
                RectTransform rect = prefab.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(j * (1f / row), i * (1f / column));
                rect.anchorMax = new Vector2((j + 1) * (1f / row), (i + 1) * (1f / column));
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                targetsObject[i, j].targetObject = prefab;
            }
        }
    }

    public void ChangeColor(int x, int y)
    {
        targetsObject[x, y].color = paintballHolder.GetMainColor();
        int index = 0;

        for (int i = 0; i < colorsList.Count; i++)
        {
            if (colorsList[i].color == targetsObject[x,y].color)
            {
                index = i;
            }
        }
        Destroy(targetsObject[x, y].targetObject);
        targetsObject[x, y].targetObject = Instantiate(colorsList[index].prefabs, location.transform);
        targetsObject[x, y].targetObject.GetComponent<TargetHolder>().x = x;
        targetsObject[x, y].targetObject.GetComponent<TargetHolder>().y = y;
        RectTransform rect = targetsObject[x, y].targetObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(y * (1f / row), x * (1f / column));
        rect.anchorMax = new Vector2((y + 1) * (1f / row), (x + 1) * (1f / column));
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        //exec after shoot function
        player.OnShoot();
        CheckNeighbours(x, y);
    }

    void CheckNeighbours(int x, int y)
    {
        combo = 0;
        targetDestroy = 0;
        int xPointer = x;
        int yPointer = y;

        List<Vector2> vertical = new List<Vector2>();
        List<Vector2> horizontal = new List<Vector2>();

        while (yPointer > 0)
        {
            if (targetsObject[x,y].color == targetsObject[x,--yPointer].color)
            {
                horizontal.Add(new Vector2(x, yPointer));
                Debug.Log("same");
            }
            else
            {
                break;
            }
        }

        yPointer = y;

        while (yPointer < column - 1)
        {
            if (targetsObject[x, y].color == targetsObject[x, ++yPointer].color)
            {
                horizontal.Add(new Vector2(x, yPointer));
                //Debug.Log("same");
            }
            else
            {
                break;
            }
        }

        while (xPointer > 0)
        {
            if (targetsObject[x,y].color == targetsObject[--xPointer, y].color)
            {
                vertical.Add(new Vector2(xPointer, y));
            }
            else
            {
                break;
            }
        }

        xPointer = x;

        while (xPointer < row - 1)
        {
            if (targetsObject[x, y].color == targetsObject[++xPointer, y].color)
            {
                vertical.Add(new Vector2(xPointer, y));
            }
            else
            {
                break;
            }
        }

        if (horizontal.Count >= 3)
        {
            for (int i = 0; i < horizontal.Count; i++)
            {
                boss.ApplyPaintballEffect(eachTargetDamage, targetsObject[(int)horizontal[i].x, (int)horizontal[i].y].color);
                Destroy(targetsObject[(int)horizontal[i].x, (int)horizontal[i].y].targetObject);
                targetsObject[(int)horizontal[i].x, (int)horizontal[i].y] = null;
            }

            targetDestroy += horizontal.Count;
        }

        if (vertical.Count >= 3)
        {
            for (int i = 0; i < vertical.Count; i++)
            {
                boss.ApplyPaintballEffect(eachTargetDamage, targetsObject[(int)vertical[i].x, (int)vertical[i].y].color);
                Destroy(targetsObject[(int)vertical[i].x, (int)vertical[i].y].targetObject);
                targetsObject[(int)vertical[i].x, (int)vertical[i].y] = null;
            }

            targetDestroy += vertical.Count;
        }

        if (horizontal.Count >= 3 || vertical.Count >= 3)
        {
            boss.ApplyPaintballEffect(eachTargetDamage, targetsObject[x, y].color);
            Destroy(targetsObject[x, y].targetObject);
            targetsObject[x, y] = null;
            targetDestroy++;
        }

        moveList = new List<Vector2>();
        int newCreatedTargetAmount = 0;

        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                if (targetsObject[j, i] == null)
                {
                    moveList.Add(new Vector2(j, i));
                    Debug.Log("object missing in " + j + " , " + i);
                    for (int k = j; k < row; k++)
                    {
                        if (targetsObject[k, i] != null)
                        {
                            Debug.Log("found available object " + k + " , " + i);
                            targetsObject[j, i] = targetsObject[k, i];
                            targetsObject[k, i] = null;
                            targetsObject[j, i].targetObject.GetComponent<TargetHolder>().x = j;
                            targetsObject[j, i].targetObject.GetComponent<TargetHolder>().y = i;
                            StartCoroutine(MoveTargetToNewPosition(targetsObject[j, i].targetObject, j, i));
                            
                            break;
                        }
                        else if (k == row - 1 && targetsObject[k, i] == null)
                        {
                            targetsObject[j, i] = GenerateNewTarget(j, i, ++newCreatedTargetAmount);
                            StartCoroutine(MoveTargetToNewPosition(targetsObject[j, i].targetObject, j, i));
                        }
                    }
                }
            }
        }

        if (moveList.Count > 0)
        {
            StartCoroutine(StartComboWithDelay(moveList));
        }
    }

    IEnumerator StartComboWithDelay(List<Vector2> list)
    {
        yield return new WaitForSeconds(2f);
        Combo(list);
    }

    void Combo(List<Vector2> list)
    {
        int count = 0;

        while (list.Count > 0)
        {
            int xPointer = (int)list[count].x;
            int yPointer = (int)list[count].y;

            List<Vector2> vertical = new List<Vector2>();
            List<Vector2> horizontal = new List<Vector2>();

            while (yPointer > 0)
            {
                if (targetsObject[(int)list[count].x, (int)list[count].y].color == targetsObject[(int)list[count].x, --yPointer].color)
                {
                    horizontal.Add(new Vector2((int)list[count].x, yPointer));
                    Debug.Log("same");
                }
                else
                {
                    break;
                }
            }

            yPointer = (int)list[count].y;

            while (yPointer < column - 1)
            {
                if (targetsObject[(int)list[count].x, (int)list[count].y].color == targetsObject[(int)list[count].x, ++yPointer].color)
                {
                    horizontal.Add(new Vector2((int)list[count].x, yPointer));
                    //Debug.Log("same");
                }
                else
                {
                    break;
                }
            }

            while (xPointer > 0)
            {
                if (targetsObject[(int)list[count].x, (int)list[count].y].color == targetsObject[--xPointer, (int)list[count].y].color)
                {
                    vertical.Add(new Vector2(xPointer, (int)list[count].y));
                }
                else
                {
                    break;
                }
            }

            xPointer = (int)list[count].x;

            while (xPointer < row - 1)
            {
                if (targetsObject[(int)list[count].x, (int)list[count].y].color == targetsObject[++xPointer, (int)list[count].y].color)
                {
                    vertical.Add(new Vector2(xPointer, (int)list[count].y));
                }
                else
                {
                    break;
                }
            }

            if (horizontal.Count >= 3)
            {
                for (int i = 0; i < horizontal.Count; i++)
                {
                    boss.ApplyPaintballEffect(eachTargetDamage, targetsObject[(int)horizontal[i].x, (int)horizontal[i].y].color);
                    Destroy(targetsObject[(int)horizontal[i].x, (int)horizontal[i].y].targetObject);
                    targetsObject[(int)horizontal[i].x, (int)horizontal[i].y] = null;
                    list.Remove(horizontal[i]);
                }

                targetDestroy += horizontal.Count;
            }

            if (vertical.Count >= 3)
            {
                for (int i = 0; i < vertical.Count; i++)
                {
                    boss.ApplyPaintballEffect(eachTargetDamage, targetsObject[(int)vertical[i].x, (int)vertical[i].y].color);
                    Destroy(targetsObject[(int)vertical[i].x, (int)vertical[i].y].targetObject);
                    targetsObject[(int)vertical[i].x, (int)vertical[i].y] = null;
                    list.Remove(vertical[i]);
                }

                targetDestroy += vertical.Count;
            }

            moveList = new List<Vector2>();

            if (horizontal.Count >= 3 || vertical.Count >= 3)
            {
                combo++;
                boss.ApplyPaintballEffect(eachTargetDamage, targetsObject[(int)list[count].x, (int)list[count].y].color);
                Destroy(targetsObject[(int)list[count].x, (int)list[count].y].targetObject);
                targetsObject[(int)list[count].x, (int)list[count].y] = null;
                targetDestroy++;
                list.RemoveAt(0);

                int newCreatedTargetAmount = 0;

                for (int i = 0; i < column; i++)
                {
                    for (int j = 0; j < row; j++)
                    {
                        if (targetsObject[j, i] == null)
                        {
                            moveList.Add(new Vector2(j, i));
                            Debug.Log("object missing in " + j + " , " + i);
                            for (int k = j; k < row; k++)
                            {
                                if (targetsObject[k, i] != null)
                                {
                                    Debug.Log("found available object " + k + " , " + i);
                                    targetsObject[j, i] = targetsObject[k, i];
                                    targetsObject[k, i] = null;
                                    targetsObject[j, i].targetObject.GetComponent<TargetHolder>().x = j;
                                    targetsObject[j, i].targetObject.GetComponent<TargetHolder>().y = i;
                                    StartCoroutine(MoveTargetToNewPosition(targetsObject[j, i].targetObject, j, i));

                                    break;
                                }
                                else if (k == row - 1 && targetsObject[k, i] == null)
                                {
                                    targetsObject[j, i] = GenerateNewTarget(j, i, ++newCreatedTargetAmount);
                                    StartCoroutine(MoveTargetToNewPosition(targetsObject[j, i].targetObject, j, i));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                list.RemoveAt(0);
            }

            if (moveList.Count > 0)
            {
                StartCoroutine(StartComboWithDelay(moveList));
            }
        }

    }

    IEnumerator MoveTargetToNewPosition(GameObject target, int x, int y)
    {
        float step = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            step += 0.2f;
            RectTransform rect = target.GetComponent<RectTransform>();
            float originMin = rect.anchorMin.y;
            float originMax = rect.anchorMax.y;
            rect.anchorMin = new Vector2(rect.anchorMin.x, Mathf.Lerp(originMin, x * 1f / row, step));
            rect.anchorMax = new Vector2(rect.anchorMax.x, Mathf.Lerp(originMax, (x + 1f) / row, step));
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;

            if (step > 1)
            {
                break;
            }
        }
    }

    Target GenerateNewTarget(int x, int y, int amount)
    {
        for (int i = 0; i < colorsList.Count; i++)
        {
            colorsList[i].isAllowed = true;
        }

        int index = GetRandomColor();

        Target ta = new Target();
        //Instantiate(colorsList[index].prefabs, new Vector3(i * 1 * distance, j * 1 * distance, 0), transform.rotation);
        ta.targetObject = Instantiate(colorsList[index].prefabs, location.transform);
        ta.targetObject.GetComponent<TargetHolder>().x = x;
        ta.targetObject.GetComponent<TargetHolder>().y = y;
        RectTransform rect = ta.targetObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(y * 1f / column, (row + amount) * 1f / row);
        rect.anchorMax = new Vector2((y + 1) * 1f / column, (row + amount + 1) * 1f / row);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        ta.color = colorsList[index].color;
        
        return ta;
    }

    void CheckDisableColor(int x, int y)
    {
        for (int i = 0; i < colorsList.Count; i++)
        {
            colorsList[i].isAllowed = true;
        }

        if (y > 0)
        {
            if (targetsObject[x, y - 1].left >= 3)
            {
                colorsList[FindColorListIndex(targetsObject[x, y - 1].color)].isAllowed = false;
            }
        }

        if (x > 0)
        {
            if (targetsObject[x - 1, y].down >= 3)
            {
                colorsList[FindColorListIndex(targetsObject[x - 1, y].color)].isAllowed = false;
            }
        }
    }

    int FindColorListIndex(string colorName)
    {
        for (int i = 0; i < colorsList.Count; i++)
        {
            if (colorsList[i].color == colorName)
            {
                return i;
            }
        }

        return -1;
    }

    //function to get random color
    int GetRandomColor()
    {
        int index;

        do
        {
            index = Random.Range(0, 5);
        } while (!colorsList[index].isAllowed);

        return index;
    }

    //color selection object
    [System.Serializable]
    public class ColorSelection
    {
        public string color;
        public bool isAllowed;
        public GameObject prefabs;
    }

    //target spawn object
    [System.Serializable]
    public class Target
    {
        public string color;
        public int left;
        public int down;
        public GameObject targetObject;
    }
    
}
