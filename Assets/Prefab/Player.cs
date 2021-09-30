using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] Transform GroundCheck;
    [SerializeField] float GroundCheckRadius = 0.1f;
    [SerializeField] float RotationSpeed = 20f;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] float EdgeCheckTracingDistance = 0.8f;
    [SerializeField] float EdgeCheckTracingDepth = 1f;
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    [SerializeField] Transform PicupSocketTransform;
    InputActions inputActions;
    Vector2 MoveInput;
    Vector3 Velocity;
    CharacterController characterController;
    float Gravity = -9.8f;
    Ladder CurrentClimbingLadder;


    List<Ladder> LaddersNearby = new List<Ladder>();

    public Transform GetPickUpSocketTransfom()
    {
        return PicupSocketTransform;
    }
    public void NotifyLadderNearby(Ladder ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }
    public void NotifyLadderExit(Ladder ladderExit)
    {
        if (ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            Velocity.y = 0;
        }
        LaddersNearby.Remove(ladderExit);

    }

    Ladder FindPlayerClimingLadder()
    {
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDirection();
        Ladder ChosenLadder = null;
        float CloestAngle = 180.0f;

        foreach (Ladder ladder in LaddersNearby)
        {
            Vector3 LadderDir = ladder.transform.position - transform.position;
            LadderDir.y = 0;
            LadderDir.Normalize();
            float Dot = Vector3.Dot(PlayerDesiredMoveDir, LadderDir);
            float AngleDgrees = Mathf.Acos(Dot) * Mathf.Rad2Deg;
            if (AngleDgrees < LadderClimbCommitAngleDegrees && AngleDgrees < CloestAngle)
            {
                ChosenLadder = ladder;
                CloestAngle = AngleDgrees;
            }
        }
        return ChosenLadder;
    }

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
        characterController = GetComponent<CharacterController>();
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
        MoveInput = ctx.ReadValue<Vector2>();
    }

    void HopOnLadder(Ladder ToHopOn)
    {
        if (ToHopOn == null) return;
        if (ToHopOn != CurrentClimbingLadder)
        {
            Transform snapToTransform = ToHopOn.GetClosestSnapTransform(transform.position);
            characterController.Move(snapToTransform.position - transform.position);
            transform.rotation = snapToTransform.rotation;
            CurrentClimbingLadder = ToHopOn;
            Debug.Log("Snap On Ladder");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentClimbingLadder == null)
        {
            HopOnLadder(FindPlayerClimingLadder());
        }
        if(CurrentClimbingLadder)
        {
            CalculateClimbingVelocity();
        }
        else
        {
            CalculateWlkingVelocity();
        }
        //Debug.Log($"player move input is: {MoveInput}");
        

        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();



    }

    void CalculateClimbingVelocity()
    {
        if(MoveInput.magnitude == 0)
        {
            Velocity = Vector3.zero;
        }
        Vector3 LadderDir = CurrentClimbingLadder.transform.forward;
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDirection();

        float Dot = Vector3.Dot(LadderDir, PlayerDesiredMoveDir);
        if(Dot<0)
        {
            Velocity = GetPlayerDesiredMoveDirection() * WalkingSpeed;
            Velocity.y = WalkingSpeed;
        }
        else
        {
            if(IsOnGround())
            {
                Velocity = GetPlayerDesiredMoveDirection() * WalkingSpeed;

            }
            Velocity.y = -WalkingSpeed;
        }
    }

    private void CalculateWlkingVelocity()
    {
        if (IsOnGround())
        {
            Velocity.y = -0.2f;
        }

        Velocity.x = GetPlayerDesiredMoveDirection().x * WalkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDirection().z * WalkingSpeed;
        Velocity.y += Gravity * Time.deltaTime;

        Vector3 PosXTracePos = transform.position + new Vector3(EdgeCheckTracingDistance, 0.5f, 0f);
        Vector3 NegXTracePos = transform.position + new Vector3(-EdgeCheckTracingDistance, 0.5f, 0f);
        Vector3 PosZTracePos = transform.position + new Vector3(0f, 0.5f, EdgeCheckTracingDistance);
        Vector3 NegZTracePos = transform.position + new Vector3(0f, 0.5f, -EdgeCheckTracingDistance);

        bool CanGoPosX = Physics.Raycast(PosXTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoNegX = Physics.Raycast(NegXTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoPosZ = Physics.Raycast(PosZTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoNegZ = Physics.Raycast(NegZTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);

        float xMin = CanGoNegX ? float.MinValue : 0f;
        float xMax = CanGoPosX ? float.MaxValue : 0f;
        float zMin = CanGoNegZ ? float.MinValue : 0f;
        float zMax = CanGoPosZ ? float.MaxValue : 0f;


        Velocity.x = Mathf.Clamp(Velocity.x, xMin, xMax);
        Velocity.z = Mathf.Clamp(Velocity.z, zMin, zMax);


    }

    public Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }

    void UpdateRotation()
    {
        if (CurrentClimbingLadder != null)
        {
            return;
        }
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDirection();
        if (PlayerDesiredDir.magnitude == 0)
        {
            PlayerDesiredDir = transform.forward;
        }
        
        Quaternion DesiredRotaion = Quaternion.LookRotation(PlayerDesiredDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotaion, Time.deltaTime * RotationSpeed);

    }
}
