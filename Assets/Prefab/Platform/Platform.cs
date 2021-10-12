using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour, Togglable
{
    PlarformIEnumerator iEnumerator;

    public Transform StartTrans;
    public Transform EndTrans;
    
    public void ToggleOn()
    {
        MoveTo(true);
    }
    public void ToggleOff()
    {
        MoveTo(false);
    }

    
    public void MoveTo(bool ToEnd)
    {
        if(ToEnd)
        {
            MoveTo(EndTrans);
        }
        else
        {
            MoveTo(StartTrans);
        }
    }
    public void MOveTo(Transform Destination)
    {
        iEnumerator = GetComponent<PlarformIEnumerator>();
        if (iEnumerator.MovingCoroutine != null)
        {
            StopCoroutine(iEnumerator.MovingCoroutine);
            iEnumerator.MovingCoroutine = null;
        }
        iEnumerator.MovingCoroutine = StartCoroutine(iEnumerator.MoveToTrans(Destination, iEnumerator.TransitionTime));
        
        
    }

    
 



}
