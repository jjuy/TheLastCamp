using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraTransition : MonoBehaviour
{
    public void CameraIn(CinemachineVirtualCamera camera)
    {
        camera.Priority = 12;
    }
    public void CameraOut(CinemachineVirtualCamera camera)
    {
        camera.Priority = 2;
    }
}

