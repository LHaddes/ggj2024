using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public List<CardScriptable> cardList = new List<CardScriptable>();
    public Slider lifeSlider;
    public int life;
    public float sliderUpdateTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLifeSlider()
    {
        lifeSlider.DOValue(life, sliderUpdateTime);
    }
}
