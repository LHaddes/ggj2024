using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState{ Start, PlayerTurn, EnemyTurn, Won, Lost }
public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerSpawnPos;
    public Transform enemySpawnPos;
    
    public Transform playerParent;
    public Transform enemyParent;

    private GameObject _player;
    private GameObject _enemy;
    
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

        _enemy = Instantiate(enemyPrefab, enemyParent);
        _enemy.transform.position = enemySpawnPos.position;

        yield return new WaitForSeconds(1f);
        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        Debug.Log("Player Action");
        //TODO DRAW CARDS
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
