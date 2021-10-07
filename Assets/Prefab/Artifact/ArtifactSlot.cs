using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] GameObject TogglingObject;

    // Start is called before the first frame update
    public void OnArtifactLeft()
    {
        //Debug.Log("Artifact Left me ");
        TogglingObject.GetComponent<Togglable>().ToggleOff();
    }
    public void OnArtifactPlaced()
    {
        //Debug.Log("Artifact Place on me");
        TogglingObject.GetComponent<Togglable>().ToggleOn();
    }
    public Transform   GetSlotTrans()
    {
        return ArtifactSlotTrans;
    }
}
