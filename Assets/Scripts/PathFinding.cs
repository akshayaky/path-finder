using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ESarkis;
using System;
public class PathFinding : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Movement playerMovement;
    [SerializeField] private PriorityQueue<Tile> openList;
    [SerializeField] private List<Tile> closeList;
    
    [SerializeField]private List<Tile> pathList;
    
    [SerializeField] private Tile startTile;
    [SerializeField] private Tile destTile;
    
    void Awake()
    {
        pathList = new List<Tile>();
    }

    void Start()
    {
        if(destTile == null)
            destTile = grid.GetTile(new Vector2(10, 1));
        Vector2 pos = playerMovement.GetPosition();
        int x = (int)Math.Round(pos.x, 0);
        int y = (int)Math.Round(pos.y, 0);
        var temp = grid.GetTile(new Vector2(x, y));
        if (temp != null)
        {
            startTile = temp;
            openList = new PriorityQueue<Tile>();
            openList.Enqueue(startTile, startTile.GetG() + startTile.GetH());
            closeList = new List<Tile>() { startTile };
        }
        
        startTile.SetGH(0, getDist(destTile));
        //path = StartCoroutine(pathFind());
        pathFind();
        //var posChange = StartCoroutine(checkChangePosition(path));
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

            if (cubeHit)
            {
                Debug.Log("We hit " + cubeHit.collider.name);
                var temp = cubeHit.transform.GetComponent<Tile>();
                if(temp != null)
                {
                    destTile = temp;
                    pathList.Clear();
                    Start();
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

            if (cubeHit)
            {
                Debug.Log("We hit " + cubeHit.collider.name);
                var temp = cubeHit.transform.GetComponent<Tile>();
                if (temp != null)
                {
                    if (temp.GetState() == Tile.States.free)
                        grid.SetGrid(temp, Tile.States.occupied, Color.black);
                    pathList.Clear();
                    resetNonPaths();
                    Start();
                }
            }
        }

        if (Input.GetMouseButton(2))
        {
            Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

            if (cubeHit)
            {
                Debug.Log("We hit " + cubeHit.collider.name);
                var temp = cubeHit.transform.GetComponent<Tile>();
                if (temp != null)
                {
                    if (temp.GetState() == Tile.States.occupied)
                    {
                        grid.SetGrid(temp, Tile.States.free, Color.white);
                    }
                    pathList.Clear();
                    Start();
                }
            }
        }

        if (Input.anyKey)
        {
            //StopCoroutine(path);
            pathList.Clear();
            Start();
            //StartCoroutine(pathFind());
        }
    }
    IEnumerator checkChangePosition(Coroutine path)
    {
        while(true)
        {
            var oldPos = new Vector2(startTile.x, startTile.y);
            var newPos = playerMovement.GetPosition();
            if (oldPos != newPos)
            {
                startTile.x = (int)Math.Round(newPos.x, 0);
                startTile.y = (int)Math.Round(newPos.x, 0);
                /*StopCoroutine(path);
                path = StartCoroutine(pathFind());
                Debug.Log("_______________________________");*/
            }
            yield return new WaitForSeconds(2.0f);
        }
        
    }

    private Tile getTile(int x, int y)
    {
        var temp = grid.GetTile(new Vector2(x, y));
        if (temp == null)
            return null;
        return temp;
    }

    bool checkifDest(Tile t1)
    {
        return (t1.x == destTile.x) && (t1.y == destTile.y);
    }

    int getDist(Tile t1)
    {
        return Mathf.Abs(t1.x - destTile.x) + Mathf.Abs(t1.y - destTile.y);
    }

    void pathFind()
    {
        List<Tile> neighbours = new List<Tile>();
        bool destReached = false;
        while (openList.Count != 0 && !destReached)
        {
            var temp = openList.Dequeue();
            neighbours.Add(getTile(temp.x + 1, temp.y));
            neighbours.Add(getTile(temp.x - 1, temp.y));
            neighbours.Add(getTile(temp.x, temp.y + 1));
            neighbours.Add(getTile(temp.x, temp.y - 1));

            for(int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i] != null && !closeList.Contains(neighbours[i]) && neighbours[i].GetState() == Tile.States.free)
                {
                    //if (neighbours[i].GetG() >= temp.GetG() + 1)
                    {
                        neighbours[i].prev = new Vector2(temp.x, temp.y);
                        neighbours[i].SetGH(temp.GetG() + 1, getDist(neighbours[i]));
                        neighbours[i].SetColor(Color.yellow);
                    }
                    if (checkifDest(neighbours[i]))
                    {
                        neighbours[i].SetColor(Color.green);
                        destReached = true;
                        break;
                    }
                    if (!openList.Contains(neighbours[i], neighbours[i].GetG() + neighbours[i].GetH()))
                    {
                        openList.Enqueue(neighbours[i], neighbours[i].GetG() + neighbours[i].GetH());
                        //yield return waitFunc;
                    }
                }

            }
            if(destReached)
            {
                tracePath();
                resetNonPaths();
            }
            neighbours.Clear();
            closeList.Add(temp);
        }
    }    

    void tracePath()
    {
        Tile current = destTile;
        pathList.Add(destTile);
        while(current != startTile)
        {
            var temp = grid.GetTile(current.prev);
            temp.SetColor(Color.green);
            current = temp;
            pathList.Add(temp);
        }
    }

    void resetNonPaths()
    {
        for (int x = 0; x < grid.width; x++)
        {

            for (int y = 0; y < grid.height; y++)
            {
                var temp = grid.GetTile(new Vector2(x, y));
                if (temp != null && temp._color == Color.green && !pathList.Contains(temp))
                {
                    temp.SetColor(Color.yellow);
                }
            }
        }
    }
}
