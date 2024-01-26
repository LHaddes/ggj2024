using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isMouseOver;
    public float scaleOnMouseOver;
    public float scaleAnimDuration;
    public float moveUp;

    private float _yPos;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        if (_yPos == 0)
        {
            _yPos = transform.position.y;
        }
        ChangeScale(isMouseOver);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        ChangeScale(isMouseOver);
    }

    public void ChangeScale(bool over)
    {
        if (over)
        {
            transform.DOMoveY(_yPos + moveUp, scaleAnimDuration);
            transform.DOScale(scaleOnMouseOver, scaleAnimDuration);
            
        }
        else
        {
            transform.DOMoveY(_yPos, scaleAnimDuration);
            transform.DOScale(1f, scaleAnimDuration);
            
        }
    }
}
