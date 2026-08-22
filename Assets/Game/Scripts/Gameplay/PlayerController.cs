using UnityEngine;
using UnityEngine.InputSystem;

namespace ArdJam2026.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        private const float DEADZONE = 0.3f;

        private GameState gameState;
        private InputAction moveAction;
        private InputAction interactAction;
        private InputAction possessAction;

        private bool movePressed = false;

        public void Initialize(GameState gameState)
        {
            this.gameState = gameState;
            moveAction = InputSystem.actions.FindAction("Move");
            interactAction = InputSystem.actions.FindAction("Interact");
            interactAction.performed += InteractAction_performed;
            possessAction = InputSystem.actions.FindAction("Possess");
            possessAction.performed += PossessAction_performed;
        }

        private void Update()
        {
            Vector2 input = moveAction.ReadValue<Vector2>();
            if (input.magnitude > DEADZONE)
            {
                if (movePressed)
                    return;

                movePressed = true;

                // Dunno if this is stupid
                Vector2Int direction = Vector2Int.RoundToInt((input * 10).normalized);
                gameState.DoMoveAction(direction);
            }
            else
            {
                movePressed = false;
            }
        }

        private void OnDestroy()
        {
            if (interactAction != null)
                interactAction.performed -= InteractAction_performed;
            if (possessAction != null)
                possessAction.performed -= PossessAction_performed;
        }

        private void InteractAction_performed(InputAction.CallbackContext context)
        {
            gameState.DoInteractAction();
        }

        private void PossessAction_performed(InputAction.CallbackContext context)
        {
            gameState.DoPossessAction();
        }
    }
}