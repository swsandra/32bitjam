using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISounds : MonoBehaviour, ISubmitHandler,  ISelectHandler
{
    [SerializeField]
    private AudioSource Source;

    public void OnSubmit(BaseEventData eventData)
    {
        Source.Play();
    }

    public void OnSelect (BaseEventData eventData) 
    {
        if(!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
            Source.Play();
    }
}
