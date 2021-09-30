using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPillar : Interactable
{
    //bool Picking = false;
    //bool followingAct = false;
    //[SerializeField] Transform itemPos;
 

    
        
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger");
       
        

    }
    // Update is called once per frame
    void Update()
    {
        //if (followingAct)
        //{
            
       //     transform.position = itemPos.position;
        //}
    }
   
    public virtual void PickedUpBy(GameObject PickerGameObject)
    {
        //Picking = true;
        Transform PicupSocketTransform = PickerGameObject.transform;
        Player PickerAsPlayer = PickerGameObject.GetComponent<Player>();
        if(PickerAsPlayer!= null)
        {
            PicupSocketTransform = PickerAsPlayer.GetPickUpSocketTransfom();
        }

        transform.rotation = PicupSocketTransform.rotation;
        transform.parent = PicupSocketTransform;
        // transform.localPosition = Vector3.forward * 1f;
        transform.localPosition = Vector3.zero;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;

    }

    public virtual void DropedDown()
    {
        //Picking = false;

        transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;


    }
    public override void Interact(GameObject InteractingGameObject)
    {
        
        if(transform.parent != null)
        {
            DropedDown();
        }
        else
        {
            Vector3 DirFromInteractingGameObj = (transform.position - InteractingGameObject.transform.position).normalized;
            Vector3 DirOfInteractingGameObj = InteractingGameObject.transform.position;
            float Dot = Vector3.Dot(DirOfInteractingGameObj, DirFromInteractingGameObj);
            //followingAct = true;
            //Debug.Log($"I am interacting with : {InteractingGameObject}");
            if (Dot > 0)
            {

                PickedUpBy(InteractingGameObject);


            }
        }
       
        
    }


}
