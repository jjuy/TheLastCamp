using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera DestinationCam;
    [SerializeField] float TransitionTime = 1.0f;
     CinemachineBrain cinemachenBrain;

    private void Start()
    {
        cinemachenBrain =Camera.main.GetComponent<CinemachineBrain>();
    }
    private void OnTriggerEnter(Collider other)
    {
        cinemachenBrain.m_DefaultBlend.m_Time = TransitionTime;
        if (other.GetComponent<Player>()!= null)
        {
            DestinationCam.Priority = 11;
        }
        //move camera to a location \
        

    }
    private void OnTriggerExit(Collider other)
    {
        //move camera back to the player
        if (other.GetComponent<Player>() != null)
        {
            DestinationCam.Priority = 1;
        }
    }
    public void CameraIn(CinemachineVirtualCamera camera)
    {
        camera.Priority = 12;
    }
    public void CameraOut(CinemachineVirtualCamera camera)
    {
        camera.Priority = 2;
    }
}

