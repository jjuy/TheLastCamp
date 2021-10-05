using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact2 : Interactable
{
    [SerializeField] Platform MovePlatform2;
    [SerializeField] Platform MovePlatform3;
    [SerializeField] Player player;
    
    // Start is called before the first frame update
    public override void Interact(GameObject InteractingGameObject )
    {
        
        Debug.Log("Artifact2 act");
        //player.ChangeTransformToPlatform();
        MovePlatform2.MOveTo(MovePlatform2.EndTrans);
        MovePlatform3.MOveTo(MovePlatform3.EndTrans);
        //player.ReturnTransform();


    }

    
}
