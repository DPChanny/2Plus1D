using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slab : MonoBehaviour
{
    public bool isPowered;

    public bool nextFrameExit = false;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        SetIsPowered(isPowered);
    }

    private void Update()
    {
        if (nextFrameExit)
        {
            SetIsPowered(false);
            nextFrameExit = false;
        }
    }

    private void SetIsPowered(bool _isPowered)
    {
        isPowered = _isPowered;
        if(isPowered)
        {
            spriteRenderer.color = Color.blue;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.PLAYER))
        {
            SetIsPowered(true);
        }
        if (collision.CompareTag(Tags.OBJECT))
        {
            SetIsPowered(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.PLAYER))
        {
            nextFrameExit = true;
        }
        if (collision.CompareTag(Tags.OBJECT))
        {
            nextFrameExit = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.PLAYER))
        {
            SetIsPowered(true);
        }
        if (collision.CompareTag(Tags.OBJECT))
        {
            SetIsPowered(true);
        }
    }
}
