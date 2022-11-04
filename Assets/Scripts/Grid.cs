using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Dictionary<Vector2, Tile> _tiles { get; private set; }
    public int width;
    public int height;
    [SerializeField] private Tile _tile;
    [SerializeField] private Transform parentGrid;
    [SerializeField] private Transform _cam;
    [SerializeField] private Sprite outerWall, destructibleWall, indestructibleWall, path;
    private void Awake()
    {
        _tiles = new Dictionary<Vector2, Tile> ();
        GridGeneration();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GridGeneration(true);
        }
    }
    
    public void SetGrid(Tile t, Tile.States _state, Color _color)
    {
        t.SetState(_state);
        //t.SetSprite(_sprite);
        t.SetColor(_color);
    }

    void GridGeneration(bool spawned = false)
    {
        System.Random rnd = new System.Random(DateTime.Now.Millisecond);
        Tile t = null;
        for(int x = 0; x < width; x++)
        {

            for(int y = 0; y < height; y++)
            {
                
                if(!spawned)
                {
                    var tileSpawned = Instantiate(_tile, new Vector3(x, y), Quaternion.identity, parentGrid);
                    tileSpawned.name = $"Tile {x} {y}";
                    tileSpawned.x = x;
                    tileSpawned.y = y;

                    t = tileSpawned.GetComponent<Tile>();
                    _tiles[new Vector2(x, y)] = t;
                }
                else
                {
                    t = GetTile(new Vector2(x, y));
                }
                
                if (x == width-1 || y == height-1 || x == 0 || y == 0)
                {
                    SetGrid(t ,Tile.States.border, Color.black);
                }
                else
                {
                    //indistructible
                    if ((x%2 == 0 && y%2==0) && !(x == width - 2 || y == height - 2 || x == 1 || y == 1))
                    {
                        SetGrid(t, Tile.States.occupied, Color.black);
                    }
                    else
                    {
                        SetGrid(t, Tile.States.free, Color.white);
                        int x_ = rnd.Next(3, width-1);
                        int y_ = rnd.Next(3, height-1);

                        if(rnd.Next(0,2)==0)
                        {
                            var t_ = GetTile(new Vector2(x_, y_));
                            if (t_ != null)
                                SetGrid(t_, Tile.States.occupied, Color.black);
                        }
                        else
                        {
                            var t_ = GetTile(new Vector2(x_, y_));
                            if (t_ != null)
                                SetGrid(t_, Tile.States.occupied, Color.black);
                        }
                        

                    }

                }
            }
        }
        _cam.position = new Vector3(width/2 -0.5f,height/2 -0.5f, -10);
    }

    public Tile GetTile(Vector2 v)
    {
        if (_tiles == null)
            return null;
        if(_tiles.TryGetValue(v, out var tile))
        {
            return tile;
        }
        return null;
    }

}
