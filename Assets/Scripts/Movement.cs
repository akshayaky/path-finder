using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Movement : MonoBehaviour
{
    [SerializeField] protected Grid grid;
    [SerializeField] protected bool[] colliderCheck;
    protected bool isMoving;
    protected Vector3 startPos, toMovePos;
    protected int x, y;
    [SerializeField]protected float timeToMove = 0.2f;
    [SerializeField]protected List<Tile.States> moveStates;
    private void Awake()
    {
        colliderCheck = new bool[] { true, true, true, true };
        grid = FindObjectOfType<Grid>();
        /*Tile t = grid.GetTile(new Vector2(x, y));
        if (t != null)
            colliderCheck[index] = t.GetCollider();*/
    }

    protected void SetSurroundCollision()
    {
        x = (int)Math.Round(transform.position.x, 0);
        y = (int)Math.Round(transform.position.y, 0);
        checkForCollider(x, y + 1, 0);
        checkForCollider(x, y - 1, 1);
        checkForCollider(x - 1, y, 2);
        checkForCollider(x + 1, y, 3);
    }

    void checkForCollider(int x, int y, int index)
    {
        Tile t = grid.GetTile(new Vector2(x, y));
        if (t != null)
            colliderCheck[index] = t.GetCollider(moveStates);
        else
            colliderCheck[index] = false;
    }

    protected IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        startPos = new Vector3(x,y,-1);
        toMovePos = startPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, toMovePos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = toMovePos;


        isMoving = false;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(x, y);
    }

}
