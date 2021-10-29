using UnityEngine;

public class SwitchSlab : MonoBehaviour
{
    public bool isPowered;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        SetIsPowered(isPowered);
    }

    public void SetIsPowered(bool _isPowered)
    {
        isPowered = _isPowered;
        if (isPowered)
        {
            spriteRenderer.color = Color.blue;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }
}
