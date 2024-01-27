using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptable cardScriptable;
    public Image img;
    

    public void InitCard()
    {
        if (!cardScriptable.img)
        {
            img = cardScriptable.img;
        }
    }

    public void PlayCard()
    {
        StartCoroutine(BattleSystem.Instance.OnCardButton(cardScriptable));
    }
}
