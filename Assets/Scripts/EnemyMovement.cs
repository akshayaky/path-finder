using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyMovement : Movement
{
    private System.Random rnd;
    private List<int> possiblePathIndex;
    private int prevDir;
    private Tile currentTile;
    private bool escapeFromPlayer;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform player;
    public int minCount = 2;

    void Start()
    {
        possiblePathIndex = new List<int>();
        prevDir = 0;
        grid = FindObjectOfType<Grid>();
        enemyManager = FindObjectOfType<EnemyManager>();
        player = FindObjectOfType<Player>().GetComponent<Transform>();
        moveStates.Add(Tile.States.free);
    }

    void Update()
    {
        int count = 0;
        SetSurroundCollision();

        possiblePathIndex.Clear();
        for (int i = 0; i < 4; i++)
        {
            if (!colliderCheck[i])
            {
                possiblePathIndex.Add(i);
                count++;
            }
                
        }

        if(enemyManager.GetAliveEnemies() <= minCount)
        {
            var playerPos = player.position;
            Vector2 changedPos = transform.position;
            if(playerPos.y - transform.position.y > 0 && possiblePathIndex.Count > 1)
            {
                possiblePathIndex.Remove(0);
                count--;
                Debug.Log("Removed up");
            }
            if(playerPos.y - transform.position.y < 0 && possiblePathIndex.Count > 1)
            {
                possiblePathIndex.Remove(1);
                count--;
                Debug.Log("Removed down");
            }
            if(playerPos.x - transform.position.x > 0 && possiblePathIndex.Count > 1)
            {
                possiblePathIndex.Remove(3);
                count--;
                Debug.Log("Removed right");
            }
            if(playerPos.x - transform.position.x < 0 && possiblePathIndex.Count > 1)
            {
                possiblePathIndex.Remove(2);
                count--;
                Debug.Log("Removed left");
            }
            
        }

        string name = gameObject.name;
        rnd = new System.Random(DateTime.Now.Millisecond + (int)name[name.Length-1]);
        int index_ = rnd.Next(count);
        int index = possiblePathIndex[index_];
        prevDir = index;
        
        if(!isMoving)
        {
            if (index == 0)
            {
                StartCoroutine(Move(Vector3.up));
            }
            if (index == 2)
            {
                StartCoroutine(Move(Vector3.left));
            }
            if (index == 1)
            {
                StartCoroutine(Move(Vector3.down));
            }
            if (index == 3)
            {
                StartCoroutine(Move(Vector3.right));
            }
        }
    }
}
