using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] Transform TopSnapTranform;
    [SerializeField] Transform BottomSnapTranform;
    MovementComponent movementComponent;

    private void Start()
    {
        movementComponent = GetComponent<MovementComponent>();
    }


    private void OnTriggerEnter(Collider other)
    {
        LadderClimbingComponent otherClimbingComp = other.GetComponent<LadderClimbingComponent>();
        if (otherClimbingComp != null)
        {
            //Debug.Log("I have overlapped with the player");
            otherClimbingComp.NotifyLadderNearby(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        LadderClimbingComponent otherClimbingComp = other.GetComponent<LadderClimbingComponent>();
        if (otherClimbingComp != null)
        {
            //Debug.Log("The player left me");
            otherClimbingComp.NotifyLadderExit(this);
        }
    }
    public Transform GetClosestSnapTransform(Vector3 Position)
    {
        float DistanceToTop = Vector3.Distance(Position, TopSnapTranform.position);
        float DistanceToBot = Vector3.Distance(Position, BottomSnapTranform.position);
        return DistanceToTop < DistanceToBot ? TopSnapTranform : BottomSnapTranform;
    }
   
    

    

    private void Update()
    {
        
    }
}
