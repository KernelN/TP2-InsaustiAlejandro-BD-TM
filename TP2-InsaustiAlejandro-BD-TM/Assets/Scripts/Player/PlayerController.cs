using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Action playerDied;
    public Action scoreChanged;
    [SerializeField] TileManager tileManager;
    [SerializeField] int maxHealth;
    [SerializeField] float speed;
    public PlayerData data;
    Vector2 move;

    private void Start()
    {
        data.health = maxHealth;
    }
    private void FixedUpdate()
    {
        Vector2 mapBorders = tileManager.GetMapBorders();
        bool xPosIsInsideMap = Mathf.Abs(transform.localPosition.x + move.x * speed) < mapBorders.x;
        bool yPosIsInsideMap = Mathf.Abs(transform.localPosition.y + move.y * speed) < mapBorders.y;
        if (xPosIsInsideMap && yPosIsInsideMap)
        {
            transform.Translate(move * speed);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collisionGO = collision.gameObject;
        if (collisionGO.layer == LayerMask.NameToLayer("Map"))
        {
            if (collisionGO.GetComponent<TileController>().currentState == TileController.State.INTANGIBLE)
            {
                Die(true);
            }
        }
        else if (collisionGO.tag == "Coin")
        {
            collisionGO.GetComponent<CoinController>().PickUp();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void Die(bool playerFell)
    {
        data.health--;
        if (data.health <= 0)
        {
            data.health = maxHealth;
            data.deaths++;
            data.score /= 2;
            scoreChanged?.Invoke();
            if (!playerFell)
            {
                do
                {
                    transform.localPosition = tileManager.GetRandomPosInsideMap();
                } while (Physics2D.Raycast(transform.localPosition, Vector2.right, 1, LayerMask.GetMask("Map")));
                GetComponent<BoxCollider2D>().enabled = false;
                Invoke("ReactivateCollider", 0.5f);
            }
        }
        if (playerFell)
        {
            transform.localPosition = new Vector3(0, 0, 0);
            GetComponent<BoxCollider2D>().enabled = false;
            Invoke("ReactivateCollider", 0.5f);
        }
        playerDied?.Invoke();
    }
    void ReactivateCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void IncreaseScore(int scoreIncrease)
    {
        data.score += scoreIncrease;
        scoreChanged?.Invoke();
    }
}