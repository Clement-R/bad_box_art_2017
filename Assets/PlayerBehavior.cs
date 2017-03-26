using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour {

    public AudioClip punch;
    public AudioClip fart1;
    public AudioClip fart2;
    public AudioClip specialFart;

    public Slider healthBar;
    public Slider powerBar;

    public GameObject gameUI;
    public GameObject gameOverScreen;
    public GameObject gameOverScore;

    public Text textDisplay;

    public bool gameOver = false;

    public float timeBetweenFart = 3f;
    public float damageReceiveInHealthPercentage = 10f;
    public float powerBySecondInPercentage = 10f;
    public float specialAttackPowerCostInPercentage = 50f;

    private float _nextFart;
    private float _nextHit;
    private float _hideTextAt;
    private int _score = 0;
    
    private AudioSource _aSource;

    public void Start() {
        _aSource = GetComponent<AudioSource>();
    }

    public void Update() {
        var currentTime = Time.realtimeSinceStartup;

        textDisplay.transform.localScale = gameObject.transform.localScale;

        if (currentTime > _hideTextAt)
            textDisplay.enabled = false;

        if (currentTime > _nextFart) {
            if (Random.Range(0, 2) == 1) {
                _aSource.clip = fart1;
                textDisplay.text = "I'm sorry...";
            }
            else {
                _aSource.clip = fart2;
                textDisplay.text = "It's so embarrassing...";
            }
            _aSource.Play();
            textDisplay.enabled = true;
            _hideTextAt = currentTime + 2f;
            _nextFart = currentTime + Random.Range(timeBetweenFart, timeBetweenFart + 2f);
        }

        if (Input.GetButtonDown("B_gamepad")) {
            _aSource.clip = punch;
            _aSource.Play();
        }

        if (Input.GetButtonDown("Y_gamepad")) {
            if (!CanSuper())
                return;

            _aSource.clip = specialFart;
            _aSource.Play();
            powerBar.value -= specialAttackPowerCostInPercentage / 100;
        }

        powerBar.value += powerBySecondInPercentage * Time.deltaTime / 100;
        if (powerBar.value > 1)
            powerBar.value = 1;
    }

    public void takeDamage() {
        if (Time.realtimeSinceStartup < _nextHit)
            return;

        healthBar.value -= damageReceiveInHealthPercentage / 100;
        if (healthBar.value <= 0) {
            GameOver();
            Time.timeScale = 0f;
        }

        _nextHit = Time.realtimeSinceStartup + 1f;
    }

    public bool CanSuper() {
        return powerBar.value >= specialAttackPowerCostInPercentage / 100;
    }

    void GameOver() {
        // Pause game and hide UI
        Time.timeScale = 0.0f;

        // Show game over screen and set score
        gameOverScore.GetComponent<Text>().text = "" + _score;
        gameOverScreen.SetActive(true);

        gameOver = true;
    }
}
