using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptable cardScriptable;
    public Image img;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitCard()
    {
        if (!cardScriptable.img)
        {
            img = cardScriptable.img;
        }
    }
}
