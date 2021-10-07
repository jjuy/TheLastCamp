using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderClimbingComponent : MonoBehaviour
{
    [SerializeField] public float LadderClimbCommitAngleDegrees = 20f;
    [SerializeField] float LadderHopOnTime = 0.2f;

    Ladder CurrentClimbingLadder;
    List<Ladder> LaddersNearby = new List<Ladder>();
    MovementComponent movementComponent;
    IInputActionCollection InputAction;
    public void SetInput(IInputActionCollection inputAction)
    {
        InputAction = inputAction;
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
            movementComponent.SetClimbingInfo(Vector3.zero, false);
            movementComponent.ClearVerticalVelocity();
        }
        LaddersNearby.Remove(ladderExit);

    }
    public Ladder FindPlayerClimingLadder()
    {
        Vector3 PlayerDesiredMoveDir = movementComponent.GetPlayerDesiredMoveDirection();
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

    public void HopOnLadder(Ladder ladderToHopOn)
    {

        if (ladderToHopOn == null) return;
        if (ladderToHopOn != CurrentClimbingLadder)
        {

            Transform snapToTransform = ladderToHopOn.GetClosestSnapTransform(transform.position);
            CurrentClimbingLadder = ladderToHopOn;
            movementComponent.SetClimbingInfo(ladderToHopOn.transform.right, true);
            DisableInput();
            //transform.position = snapToTransform.position;
            //transform.rotation = snapToTransform.rotation;
            StartCoroutine(movementComponent.MoveToTransform(snapToTransform, LadderHopOnTime));
            Invoke("EnableInput", LadderHopOnTime);

            //Debug.Log("Snap On Ladder");
        }

    }
     void EnableInput()
    {
        InputAction.Enable();
    }
     void DisableInput()
    {
        InputAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        movementComponent = GetComponent<MovementComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentClimbingLadder == null)
        {
            HopOnLadder(FindPlayerClimingLadder());
        }
    }
}
