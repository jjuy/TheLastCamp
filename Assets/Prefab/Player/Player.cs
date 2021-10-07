using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    [SerializeField] Transform PicupSocketTransform;
    InputActions inputActions;

    MovementComponent movementComponent;
    LadderClimbingComponent climbingComp;

   
    public Transform GetPickUpSocketTransfom()
    {
        return PicupSocketTransform;
    }
 
    private void Awake()
    {
        inputActions = new InputActions();
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        movementComponent = GetComponent<MovementComponent>();
        climbingComp = GetComponent<LadderClimbingComponent>();
        climbingComp.SetInput(inputActions);

        inputActions.Gameplay.Move.performed += MoveInputUpdated;
        inputActions.Gameplay.Move.canceled += MoveInputUpdated;
        inputActions.Gameplay.Interact.performed += Interact;

    }
    void Interact(InputAction.CallbackContext ctx)
    {
        InteractComponent interactComp = GetComponentInChildren<InteractComponent>();
        if(interactComp!= null)
        {
            interactComp.Interact();
        }
    }
    

    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        movementComponent.SetMovementInput(ctx.ReadValue<Vector2>());
    }

    
}
