using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] Transform GroundCheck;
    [SerializeField] float GroundCheckRadius = 0.1f;
    [SerializeField] float RotationSpeed;
    [SerializeField] LayerMask GroundLayerMask;
    InputActions inputActions;
    Vector2 MoveInput;
    Vector3 Velocity;
    CharacterController characterController;
    float Gravity= -9.8f;

    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
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
        if(IsOnGround())
        {
            Velocity.y = -0.2f;
        }
        characterController = GetComponent<CharacterController>();
        inputActions.Gameplay.Move.performed += MoveInputUpdated;
        inputActions.Gameplay.Move.canceled += MoveInputUpdated;

    }

    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"player move input is: {MoveInput}");
        Velocity.x = GetPlayerDesiredMoveDirection().x * WalkingSpeed;
        Velocity.y += Gravity * Time.deltaTime;
        Velocity.z = GetPlayerDesiredMoveDirection().z * WalkingSpeed;
        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();


    }
    Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }

    void UpdateRotation()
    {
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDirection();
        if(PlayerDesiredDir.magnitude ==0)
        {
            PlayerDesiredDir = transform.forward;
        }
        Quaternion DesiredRotaion = Quaternion.LookRotation(PlayerDesiredDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotaion, Time.deltaTime*RotationSpeed);

    }
}
