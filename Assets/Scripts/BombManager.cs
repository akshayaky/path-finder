using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BombManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Sprite path;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float timer;
    [SerializeField] private Player player;
    private bool bombSpawned = false;
    private float timeToExplode = 3;
    private int x, y;
    private GameObject bomb;

    private PlayerMovement _playerMovement;
    private EnemyManager _enemyManager;
    private List<GameObject> enemies;
    private Tile currentTile;
    
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _enemyManager = FindObjectOfType<EnemyManager>();
        enemies = _enemyManager.enemies;
        timer = timeToExplode;
    }

    
    void Update()
    {
        if (!bombSpawned)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SpawnBomb();
            }
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                bombSpawned = false;
                timer = timeToExplode;
                Explode();
            }
        }

        

    }

    void SpawnBomb()
    {
        bombSpawned = true;
        Vector2 pos = _playerMovement.GetPosition();
        x = (int)Math.Round(pos.x, 0);
        y = (int)Math.Round(pos.y, 0);
        bomb = Instantiate(bombPrefab, pos, Quaternion.identity);
        currentTile = grid.GetTile(new Vector2(x, y));
        if(currentTile != null)
        {
            currentTile.SetState(Tile.States.occupied);
        }
    }

    void Explode()
    {
        for (int k = 0; k < enemies.Count; k++)
        {
            int posX = (int)Math.Round(enemies[k].transform.position.x, 0);
            int posY = (int)Math.Round(enemies[k].transform.position.y, 0);
            if (new List<int> { x, x - 1, x + 1 }.Contains(posX) && new List<int> { y, y - 1, y + 1 }.Contains(posY))
            {
                _enemyManager.Destroy(k);
            }
        }

        Destroy(bomb);
        Vector2 pos = transform.position;
        pos.x = (int)Math.Round(pos.x, 0);
        pos.y = (int)Math.Round(pos.y, 0);
        for (int i = x-1; i <= x+1; i++)
        {
            for (int j = y-1; j <= y+1; j++)
            {
                Tile t = grid.GetTile(new Vector2(i, j));
                if(t != null)
                {
                    if (t.GetState() == Tile.States.destructible)
                    {
                        grid.SetGrid(t, Tile.States.free, Color.white);
                    }
                }
                if(pos.x == i && pos.y == j)
                {
                    player.Lose();
                }
                
                
            }
        }
        currentTile.SetState(Tile.States.free);
    }
}
