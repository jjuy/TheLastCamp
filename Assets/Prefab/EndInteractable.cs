using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
public class EndInteractable : Interactable
{
    [SerializeField] CinemachineVirtualCamera camera;
    CameraTransition cameraTransition;
    [SerializeField] GameObject EndUI;
    public override void Interact(GameObject InteractingObject = null)
    {
        cameraTransition = GetComponent<CameraTransition>();
        EndUI.SetActive(true);
        cameraTransition.CameraIn(camera);

    }
}
