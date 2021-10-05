using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] public Transform objectToMove;
    [SerializeField] float TransitionTime = 1;

    public Transform StartTrans;
    public Transform EndTrans;
    


    Coroutine MovingCoroutine;
    public void MOveTo(Transform Destination)
    {
        
        if (MovingCoroutine != null)
        {
            StopCoroutine(MovingCoroutine);
            MovingCoroutine = null;
        }
        MovingCoroutine = StartCoroutine(MoveToTrans(Destination, TransitionTime));
        
        
    }

    IEnumerator MoveToTrans(Transform Destination, float TransitionTime)
    {
        float timmer = 0f;
        while (timmer < TransitionTime)
        {

            timmer += Time.deltaTime;
            objectToMove.position = Vector3.Lerp(objectToMove.position, Destination.position, timmer / TransitionTime);
            objectToMove.rotation = Quaternion.Lerp(objectToMove.rotation, Destination.rotation, timmer / TransitionTime);
            yield return new WaitForEndOfFrame();
        }
    }
 



}
