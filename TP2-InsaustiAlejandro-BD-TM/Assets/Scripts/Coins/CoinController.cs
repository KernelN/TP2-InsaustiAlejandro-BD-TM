using System;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public enum Type { YELLOW, RED };
    public Type type { get; private set; }
    public Action<CoinController> coinPickedUp;

    private void Start()
    {
        UpdateSprite();
    }

    public void ChangeType()
    {
        if (type == Type.YELLOW)
        {
            type = Type.RED;
        }
        else
        {
            type = Type.YELLOW;
        }
        UpdateSprite();
    }
    void UpdateSprite()
    {
        switch (type)
        {
            case Type.YELLOW:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case Type.RED:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            default:
                break;
        }
    }
    public void PickUp()
    {
        coinPickedUp?.Invoke(this);
        gameObject.SetActive(false);
    }
}