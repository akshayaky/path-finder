using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private System.Random rnd;
    private UIController uiController;

    public List<GameObject> enemies;
    public int numEnemies = 5;
    public float waitSeconds;

    void Awake()
    {
        rnd = new System.Random(DateTime.Now.Millisecond);
        enemies = new List<GameObject>();
        uiController = FindObjectOfType<UIController>();

        StartCoroutine(CreateEnemies());
    }


    IEnumerator CreateEnemies()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            int x = 0, y = 0;
            if (rnd.Next(0, 2) == 1)
            {
                x = 1;
                y = rnd.Next(1, 7);
            }
            else
            {
                x = rnd.Next(1, 10);
                y = 1;
            }
            enemies.Add(Instantiate(enemyPrefab, new Vector3(x, y, -1), Quaternion.identity));
            enemies[i].name = $"Enemy{i}";
            yield return new WaitForSeconds(waitSeconds);
        }
    }

    public void Destroy(int k)
    {
        Destroy(enemies[k]);
        enemies.RemoveAt(k);

        if(enemies.Count == 0)
        {
            uiController.Win();
        }
    }

    public int GetAliveEnemies()
    {
        return enemies.Count;
    }

}
