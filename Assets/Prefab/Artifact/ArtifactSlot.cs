using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] Platform PlatformToMove;

    // Start is called before the first frame update
    public void OnArtifactLeft()
    {
        //Debug.Log("Artifact Left me ");
        PlatformToMove.MOveTo(PlatformToMove.StartTrans);
    }
    public void OnArtifactPlaced()
    {
        //Debug.Log("Artifact Place on me");
        PlatformToMove.MOveTo(PlatformToMove.EndTrans);
    }
    public Transform   GetSlotTrans()
    {
        return ArtifactSlotTrans;
    }
}
