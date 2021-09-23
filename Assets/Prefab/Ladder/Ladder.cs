using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] Transform TopSnapTranform;
    [SerializeField] Transform BottomSnapTranform;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Player otherAsPlayer = other.GetComponent<Player>();
        if (otherAsPlayer != null)
        {
            //Debug.Log("I have overlapped with the player");
            otherAsPlayer.NotifyLadderNearby(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player otherAsPlayer = other.GetComponent<Player>();
        if (otherAsPlayer != null)
        {
            //Debug.Log("The player left me");
            otherAsPlayer.NotifyLadderExit(this);
        }
    }
    public Transform GetClosestSnapTransform(Vector3 Position)
    {
        float DistanceToTop = Vector3.Distance(Position, TopSnapTranform.position);
        float DistanceToBot = Vector3.Distance(Position, BottomSnapTranform.position);
        return DistanceToTop < DistanceToBot ? TopSnapTranform : BottomSnapTranform;
    }
}
