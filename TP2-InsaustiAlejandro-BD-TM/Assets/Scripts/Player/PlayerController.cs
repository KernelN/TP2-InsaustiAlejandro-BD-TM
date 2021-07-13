using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] PlayerData data;
    Vector2 move;

    private void FixedUpdate()
    {
        transform.Translate(move * speed);
    }
    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
}