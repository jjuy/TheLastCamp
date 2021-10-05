using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInteractable : Interactable
{
    // Start is called before the first frame update
    public override void Interact(GameObject InteractingObject = null)
    {
        GetComponentInChildren<Platform>().MoveTo(true);
    }
}
