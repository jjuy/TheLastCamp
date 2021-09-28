using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    List<Interactable> interactables = new List<Interactable>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Overlapped with something");
        Interactable otherAsInteractable = other.GetComponent<Interactable>();
        if (otherAsInteractable!=null)
        {
            Debug.Log("Find Interactable");
            if (!interactables.Contains(otherAsInteractable))
            {
                interactables.Add(otherAsInteractable);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Interactable otherAsInteractable = other.GetComponent<Interactable>();
        if(otherAsInteractable)
        {
            if(interactables.Contains(otherAsInteractable))
            {
                interactables.Remove(otherAsInteractable);
            }
        }
    }

    public void Interact()
    {
        Interactable closestInteractable = GetCloesetInteractable();
        if (closestInteractable != null)
        {
            closestInteractable.Interact();
        }
    }

    Interactable GetCloesetInteractable()
    {
        Interactable closestInteractable = null;
        if(interactables.Count == 0)
        {
            return closestInteractable;
        }
        float ClosestDist = float.MaxValue;
        foreach(var itemInteractable in interactables)
        {
            float Dist = Vector3.Distance(transform.position, itemInteractable.transform.position);
            if(Dist<ClosestDist)
            {
                closestInteractable = itemInteractable;
                ClosestDist = Dist;
            }

        }
        return closestInteractable;

    }
}
    