using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] float EdgeCheckTracingDistance = 0.8f;
    [SerializeField] float EdgeCheckTracingDepth = 1f;
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] float RotationSpeed = 20f;

    [Header("Ground Check")]
    [SerializeField] Transform GroundCheck;
    [SerializeField] float GroundCheckRadius = 0.1f;
    [SerializeField] LayerMask GroundLayerMask;

    Player player;
    bool isClimbing;
    Vector3 LadderDir;
    Vector2 MoveInput;
    Vector3 Velocity;
    float Gravity = -9.8f;
    CharacterController characterController;

    Transform currentFloor;
    Vector3 PreviousWorldPos;
    Vector3 PreviousFloorLocalPos;
    Quaternion PreviousWorldRot;
    Quaternion PreviousFloorLocalRot;
    public void SetMovementInput(Vector2 inputVal)
    {
        MoveInput = inputVal;
    }

    public void ClearVerticalVelocity()
    {
        Velocity.y = 0;
    }

    public void SetClimbingInfo(Vector3 ladderDir,bool climbing)
    {
        LadderDir = ladderDir;
        isClimbing = climbing;
    }
   
    void CheckFloor()
    {
        Collider[] cols = Physics.OverlapSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
        if (cols.Length != 0)
        {
            if (currentFloor != cols[0].transform)
            {
                currentFloor = cols[0].transform;
                SnapShotPositionAndRotation();
                //Debug.Log($"I am now standing on {currentFloor}");
            }
        }

    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        player = GetComponentInParent<Player>();
    }


    void SnapShotPositionAndRotation()
    {
        PreviousWorldPos = transform.position;
        PreviousWorldRot = transform.rotation;
        if (currentFloor != null)
        {
            PreviousFloorLocalPos = currentFloor.InverseTransformPoint(transform.position);
            PreviousFloorLocalRot = Quaternion.Inverse(currentFloor.rotation) * transform.rotation;


        }
    }


    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
    }
    public IEnumerator MoveToTransform(Transform Destination, float transformTime)
    {
        Vector3 StartPOs = transform.position;
        Vector3 EndPos = Destination.position;
        Quaternion StartRot = transform.rotation;
        Quaternion EndRot = Destination.rotation;

        float timmer = 0f;
        while(timmer<transformTime)
        {
            timmer += Time.deltaTime;
            Vector3 DeltaMove = Vector3.Lerp(StartPOs,EndPos,timmer);
            characterController.Move(DeltaMove);
            transform.rotation = Quaternion.Lerp(StartRot,EndRot,timmer);
            yield return new WaitForEndOfFrame();
        }
    }

    
    void FollowFloor()
    {
        if (currentFloor)
        {
            Vector3 DeltaMove = currentFloor.TransformPoint(PreviousFloorLocalPos) - PreviousWorldPos;
            Velocity += DeltaMove / Time.deltaTime;

            Quaternion DestinationRot = currentFloor.rotation * PreviousFloorLocalRot;
            Quaternion DeltaRot = Quaternion.Inverse(PreviousWorldRot) * DestinationRot;
            transform.rotation = transform.rotation * DeltaRot;
        }
    }
    private void Update()
    {
        if (isClimbing)
        {
            CalculateClimbingVelocity();
        }
        else
        {
            CalculateWlkingVelocity();
        }
        //Debug.Log($"player move input is: {MoveInput}");

        CheckFloor();
        FollowFloor();
        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();


        SnapShotPositionAndRotation();
    }
    public Vector3 GetPlayerDesiredMoveDirection()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }

    void CalculateClimbingVelocity()
    {
        if (MoveInput.magnitude == 0)
        {
            Velocity = Vector3.zero;
        }
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDirection();

        float Dot = Vector3.Dot(LadderDir, PlayerDesiredMoveDir);
        if (Dot < 0)
        {
            Velocity = GetPlayerDesiredMoveDirection() * WalkingSpeed;
            Velocity.y = WalkingSpeed;
        }
        else
        {
            if (IsOnGround())
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

    void UpdateRotation()
    {
        if (isClimbing)
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
