using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public TMP_Text timerTxt;

    public int goodDamage;
    public int okDamage;
    public int badDamage;
    
    [Header("Player")]
    public GameObject playerPrefab;
    public Transform playerSpawnPos;
    public Transform playerParent;
    public Transform cardZone;
    public float turnDuration;
    public int critChance;
    
    private GameObject _player;
    private Player _playerScript;
    
    [Header("Enemy")]
    public GameObject enemyPrefab;
    public Transform enemySpawnPos;
    public Transform enemyParent;
    
    private GameObject _enemy;
    private Enemy _enemyScript;
    private List<GameObject> _cardOnScreen = new List<GameObject>();
    private float _timer;
    private bool _timerOn;

    public static BattleSystem Instance;

    public void Awake()
    {
        Instance = this;
    }

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
        _enemyScript = _enemy.GetComponent<Enemy>();

        yield return new WaitForSeconds(1f);
        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        int randomGoodJoke = Random.Range(0, Enum.GetValues(typeof(CardType)).Length);
        int randomBadJoke = Random.Range(0, Enum.GetValues(typeof(CardType)).Length);
        
        while(randomBadJoke == randomGoodJoke)
            randomBadJoke = Random.Range(0, Enum.GetValues(typeof(CardType)).Length);

        goodType = (CardType)randomGoodJoke;
        badType = (CardType)randomBadJoke;
        
        _playerScript.DrawHand();

        foreach (var card in _playerScript.playerHand)
        {
            CreateCardOnScreen(card);
        }

        _timer = turnDuration;
        _timerOn = true;
        timerTxt.gameObject.SetActive(true);
    }

    public void CreateCardOnScreen(CardScriptable card)
    {
        GameObject crdObj = Instantiate(cardPrefab, cardZone);
        Card crdScript = crdObj.GetComponent<Card>();

        crdScript.cardScriptable = card;
        crdScript.InitCard();
        
        _cardOnScreen.Add(crdObj);
    }

    public IEnumerator OnCardButton(CardScriptable card)
    {
        Debug.Log($"Play card in {state}");

        int randomCrit = Random.Range(1, 100);

        if (card.cardType == goodType)
        {
            if (state == BattleState.PlayerTurn)
            {
                Debug.Log("Player is good");

                if (randomCrit <= critChance)
                {
                    Debug.Log("Critical hit !");
                    _enemyScript.life += Mathf.RoundToInt(goodDamage * 1.25f);
                }
                else
                {
                    _enemyScript.life += goodDamage;
                }
                 
                _enemyScript.UpdateLifeSlider();
            }
            else
            {
                Debug.Log("Enemy is good");
                _playerScript.life += goodDamage;
                _playerScript.UpdateLifeSlider();
            }
        }
        else if(card.cardType == badType)
        {
            if (state == BattleState.PlayerTurn)
            {
                Debug.Log("Player is bad");
                _enemyScript.life += badDamage;
                _enemyScript.UpdateLifeSlider();
            }
            else
            {
                Debug.Log("Enemy is bad");
                _playerScript.life += badDamage;
                _playerScript.UpdateLifeSlider();
            }
        }
        else
        {
            if (state == BattleState.PlayerTurn)
            {
                Debug.Log("Player is ok");

                if (randomCrit <= critChance)
                {
                    Debug.Log("Critical hit !");
                    _enemyScript.life += Mathf.RoundToInt(okDamage * 1.25f);
                }
                else
                {
                    _enemyScript.life += okDamage;
                }
                    
                _enemyScript.UpdateLifeSlider();
            }
            else
            {
                Debug.Log("Enemy is ok");
                _playerScript.life += okDamage;
                _playerScript.UpdateLifeSlider();
            }
        }
        
        if (state == BattleState.PlayerTurn)
        {
            _timerOn = false;
        }

        yield return new WaitForSeconds(1f);

        ChangeTurn();
    }

    public void ChangeTurn()
    {
        CheckEndConditions();

        switch (state)
        {
            case BattleState.PlayerTurn:
                _playerScript.ClearHand();
                _timerOn = false;
                timerTxt.gameObject.SetActive(false);

                foreach (var crd in _cardOnScreen)
                {
                    Destroy(crd);
                }
                
                _cardOnScreen.Clear();
                state = BattleState.EnemyTurn;
                EnemyTurn();
                break;
            case BattleState.EnemyTurn:
                state = BattleState.PlayerTurn;
                PlayerTurn();
                break;
            case BattleState.Won:
                Debug.Log("You won");
                break;
            case BattleState.Lost:
                Debug.Log("You lost");
                break;
        }
    }

    public void CheckEndConditions()
    {
        if (state == BattleState.PlayerTurn)
        {
            if (_enemyScript.life >= _enemyScript.lifeSlider.maxValue)
            {
                state = BattleState.Won;
            }
        }
        
        else if (state == BattleState.EnemyTurn)
        {
            if (_playerScript.life >= _playerScript.lifeSlider.maxValue)
            {
                state = BattleState.Lost;
            }
        }
    }

    public void EnemyTurn()
    {
        int randomCardInt = Random.Range(0, 100);

        if (randomCardInt <= _enemyScript.goodPercent)
        {
            //GOOD CARD
            for (int i = 0; i < _enemyScript.cardList.Count; i++)
            {
                if (_enemyScript.cardList[i].cardType == goodType)
                {
                    StartCoroutine(OnCardButton(_enemyScript.cardList[i]));
                    return;
                }
            }
            
        }
        else if (randomCardInt > _enemyScript.goodPercent &&
                 randomCardInt <= _enemyScript.goodPercent + _enemyScript.badPercent)
        {
            //BAD CARD
            for (int i = 0; i < _enemyScript.cardList.Count; i++)
            {
                if (_enemyScript.cardList[i].cardType == badType)
                {
                    StartCoroutine(OnCardButton(_enemyScript.cardList[i]));
                    return;
                }
            }
        }
        else
        {
            //OK CARD
            for (int i = 0; i < _enemyScript.cardList.Count; i++)
            {
                if (_enemyScript.cardList[i].cardType != badType && _enemyScript.cardList[i].cardType != goodType)
                {
                    StartCoroutine(OnCardButton(_enemyScript.cardList[i]));
                    return;
                }
            }
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerOn)
        {
            _timer -= Time.deltaTime;
            timerTxt.text = Mathf.RoundToInt(_timer).ToString();
            
            if(_timer <= 0)
                ChangeTurn();
        }
    }
}
