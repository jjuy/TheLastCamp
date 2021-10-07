using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlatformInteractable : Interactable
{
    [SerializeField] CinemachineVirtualCamera camera;
    // Start is called before the first frame update
    public override void Interact(GameObject InteractingObject = null)
    {
        GetComponentInChildren<CameraTransition>().CameraIn(camera);
        GetComponentInChildren<Platform>().MoveTo(true);
        GetComponentInChildren<CameraTransition>().CameraOut(camera);
    }
}
