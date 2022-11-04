using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tile : MonoBehaviour
{
    public Color _color;

    [SerializeField] private int H = 0;

    [SerializeField] private int G = 0;


    [SerializeField] private Color total;

    [SerializeField] private TMP_Text H_text;
    [SerializeField] private TMP_Text G_text;
    [SerializeField] private TMP_Text Total_text;

    public Vector2 prev;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private List<string> occuppyingTags;

    public int x, y;

    public enum States
    {
        free,
        occupied,
        destructible,
        indestructible,
        border

    }

    [SerializeField] private States currentState;
    public void Awake()
    {
        occuppyingTags = new List<string>() {"Bomb"};
        currentState = States.free;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        H_text.text = H.ToString();
        G_text.text = G.ToString();
        Total_text.text = (H+G).ToString();
    }

    public void SetSprite(Sprite _sprite)
    {
        _spriteRenderer.sprite = _sprite;
    }

    public void SetColor(Color c)
    {
        _spriteRenderer.color = c;
        _color = c;
    }

    public void SetCollider(bool set)
    {
        _boxCollider2D.isTrigger = !set;
    }

    public void SetState(States _state)
    {
        currentState = _state;
    }

    public States GetState()
    {
        return currentState;
    }

    public bool GetCollider(List<States> moveStates)
    {
        return !moveStates.Contains(currentState);
    }

    public void SetGH(int g, int h)
    {
        G = g;
        H = h;
        H_text.text = H.ToString();
        G_text.text = G.ToString();
        Total_text.text = (H+G).ToString();
    }

    public int GetG()
    {
        return G;
    }

    public int GetH()
    {
        return H;
    }
}
