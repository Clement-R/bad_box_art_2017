using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIA : MonoBehaviour {

    public float attackDelay = 1f;
    public float waitDuration = 1f;
    public float moveDuration = 0.1f;

    public float attackRange = 1f;    

    private GameObject _player;
    private KeyCode _currentKey;

    private float _nextAttack;
    private float _waitEnd;
    private float _moveEnd;

	void Start () {
        _player = GameManager.Instance.player;
	}
	
	void Update () {
        // Decisional tree
        var currentTime = Time.realtimeSinceStartup;

        if (currentTime < _moveEnd || currentTime < _waitEnd) {
            return;
        }

        var currentPlayerPos = _player.transform.position;
        var currentMonsterPos = gameObject.transform.position;
        if (Vector2.Distance(currentPlayerPos, currentMonsterPos) > attackRange) {
            // If player not in range
            switch(Random.Range(0, 2)) {
                case 0:
                    // Move towards the player;
                    if (currentPlayerPos.x < currentMonsterPos.x)
                        _currentKey = KeyCode.Keypad4; // going left
                    else
                        _currentKey = KeyCode.Keypad6; // going right

                    _moveEnd = currentTime + moveDuration;
                    break;
                case 1:
                    // Wait
                    _currentKey = KeyCode.None;
                    _waitEnd = currentTime + waitDuration;
                    break;
            }
        }
        else {
            // If player in range
            var decision = Random.Range(0, 3);

            switch (decision) {
                case 0:
                    // Move away from the player to avoid getting hit;
                    if (currentPlayerPos.x > currentMonsterPos.x)
                        _currentKey = KeyCode.Keypad4; // going left
                    else
                        _currentKey = KeyCode.Keypad6; // going right

                    _moveEnd = currentTime + moveDuration;
                    break;
                case 1:
                    // Wait
                    _currentKey = KeyCode.None;
                    _waitEnd = currentTime + waitDuration;
                    break;
                case 2:
                    // Attack
                    if (currentTime < _nextAttack) { 
                        // If decision is attack but we're in cooldown
                        _currentKey = KeyCode.None;
                    }
                    else {
                        _currentKey = KeyCode.Keypad0;
                        _nextAttack = currentTime + attackDelay;
                    }
                    break;
            }
        }
	}

    public bool GetKey(KeyCode testCode) {
        return _currentKey == testCode;
    }
}
