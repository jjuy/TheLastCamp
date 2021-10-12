using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlarformIEnumerator : MonoBehaviour
{
    [SerializeField] public Transform objectToMove;
    [SerializeField] public float TransitionTime = 1;

    public Coroutine MovingCoroutine;
    public IEnumerator MoveToTrans(Transform Destination, float TransitionTime)
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
