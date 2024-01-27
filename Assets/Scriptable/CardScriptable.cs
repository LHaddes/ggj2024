using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardType{ Type1, Type2, Type3, Type4, Type5 }

[CreateAssetMenu(fileName = "New Card", menuName = "New Card")]
public class CardScriptable : ScriptableObject
{
    public Image img;
    public string txt;

    public CardType cardType;
}
