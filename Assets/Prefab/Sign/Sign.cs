using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    [SerializeField] Image DialogBG;
    [SerializeField] Text DialogText;
    [SerializeField] float TransitionSpeed = 1f;
    [SerializeField] string[] dialogs;
    int currentDialogIndex = 0;
    Color DialogTextColor;
    Color DialogBGColor;
    float Opacity;
    Coroutine TransitionCoroutine;
    void GoTONextDialog()
    {
        if(dialogs.Length==0)
        {
            return;
        }
        currentDialogIndex = (currentDialogIndex + 1) % dialogs.Length;
        DialogText.text = dialogs[currentDialogIndex];
    }
    // Start is called before the first frame update
    void Start()
    {
        DialogTextColor = DialogText.color;
        DialogBGColor = DialogBG.color;
        SetOpacity(0);
        if(dialogs.Length !=0)
        {
            DialogText.text = dialogs[0];
        }
        else
        {
            DialogText.text = "";
        }
    }

    void SetOpacity(float opacity)
    {
        
        opacity = Mathf.Clamp(opacity, 0, 1);
        Color ColorMult = new Color(1f, 1f, 1f, opacity);
        DialogText.color = DialogTextColor* ColorMult;
        DialogBG.color = DialogBGColor * ColorMult;
        Opacity = opacity;
        //DialogText.CrossFadeAlpha();
    }
    IEnumerator TransitionOpacityTo (float newOpacity)
    {
        
        float Dir = newOpacity - Opacity > 0 ? 1 : -1;
        while (Opacity != newOpacity)
        {
            SetOpacity(Opacity+Dir*TransitionSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        SetOpacity(newOpacity);
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractComponent interactableComp = other.GetComponent<InteractComponent>();
        if(interactableComp!=null)
        {
            if(TransitionCoroutine!=null)
            {
                StopCoroutine(TransitionCoroutine);
                TransitionCoroutine = null;
            }
            TransitionCoroutine = StartCoroutine(TransitionOpacityTo(1));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractComponent interactableComp = other.GetComponent<InteractComponent>();
        if (interactableComp != null)
        {
            TransitionCoroutine = StartCoroutine(TransitionOpacityTo(0)); ;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        //base.Interact();
        //Debug.Log("Sign is Interacted");
        GoTONextDialog();
    }
}
