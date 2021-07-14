using UnityEngine;

public class TileController : MonoBehaviour
{
    public enum State { SOLID, DISAPPEARING, INTANGIBLE };
    public State currentState { get; private set; }

    public void OnStateUpdate()
    {
        switch (currentState)
        {
            case State.DISAPPEARING:
                currentState = State.INTANGIBLE;
                break;
            case State.INTANGIBLE:
                currentState = State.SOLID;
                break;
            default:
                break;
        }
        SetSprite();
    }
    public void StartDisappear()
    {
        currentState = State.DISAPPEARING;
        SetSprite();
    }
    void SetSprite()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color newColor = sprite.color;
        switch (currentState)
        {
            case State.SOLID:
                newColor.a = 1;
                newColor.g = 128;
                break;
            case State.DISAPPEARING:
                newColor.a = 0.5f;
                newColor.g = 128 * newColor.a;
                break;
            case State.INTANGIBLE:
                newColor.a = 0.1f;
                newColor.g = 128 * newColor.a;
                break;
            default:
                break;
        }
        sprite.color = newColor;
    }
}
