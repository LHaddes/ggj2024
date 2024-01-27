using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum BattleState{ Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public CardType goodType;
    public CardType badType;
    public GameObject cardPrefab;
    
    [Header("Player")]
    public GameObject playerPrefab;
    public Transform playerSpawnPos;
    public Transform playerParent;
    public Transform cardZone;
    
    private GameObject _player;
    private Player _playerScript;
    
    [Header("Enemy")]
    public GameObject enemyPrefab;
    public Transform enemySpawnPos;
    public Transform enemyParent;
    
    private GameObject _enemy;
    private Unit _enemyScript;
    
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

   IEnumerator SetupBattle()
    {
        _player = Instantiate(playerPrefab, playerParent);
        _player.transform.position = playerSpawnPos.position;
        _playerScript = _player.GetComponent<Player>();

        _enemy = Instantiate(enemyPrefab, enemyParent);
        _enemy.transform.position = enemySpawnPos.position;
        _enemyScript = _enemy.GetComponent<Unit>();

        yield return new WaitForSeconds(1f);
        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        Debug.Log("Player Action");
        int randomGoodJoke = Random.Range(0, Enum.GetValues(typeof(CardType)).Length);
        int randomBadJoke = Random.Range(0, Enum.GetValues(typeof(CardType)).Length);
        
        if(randomBadJoke == randomGoodJoke)
            randomBadJoke = Random.Range(0, Enum.GetValues(typeof(CardType)).Length);

        goodType = (CardType)randomGoodJoke;
        badType = (CardType)randomBadJoke;
        
        Debug.Log($"Good joke is {randomGoodJoke}");
        Debug.Log($"Bad joke is {randomBadJoke}");
        
        //TODO DRAW CARDS
        
        _playerScript.DrawHand();

        foreach (var card in _playerScript.playerHand)
        {
            CreateCardOnScreen(card);
        }
    }

    public void CreateCardOnScreen(CardScriptable card)
    {
        GameObject crdObj = Instantiate(cardPrefab, cardZone);
        Card crdScript = crdObj.GetComponent<Card>();

        crdScript.cardScriptable = card;
        crdScript.InitCard();
    }

    public void OnCardButton(GameObject card)
    {
        Debug.Log(card.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
