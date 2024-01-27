using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public List<CardScriptable> playerHand = new List<CardScriptable>(5);

    public void DrawHand()
    {
        List<int> randomMemory = new List<int>();
        
        for (int i = 0; i < 5; i++)
        {
            int random = Random.Range(0, cardList.Count);

            while (randomMemory.Contains(random))
            {
                random = Random.Range(0, cardList.Count);
            }
            

            randomMemory.Add(random);
            playerHand.Add(cardList[random]);
        }
    }

    public void ClearHand()
    {
        playerHand.Clear();
    }
}
