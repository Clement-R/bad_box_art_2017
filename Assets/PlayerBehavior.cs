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

    public float timeBetweenFart = 3f;
    public float damageReceiveInHealthPercentage = 10f;
    public float powerBySecondInPercentage = 10f;
    public float specialAttackPowerCostInPercentage = 50f;

    private float _nextFart;
    private float _nextHit;
    
    private AudioSource _aSource;

    public void Start() {
        _aSource = GetComponent<AudioSource>();
    }

    public void Update() {
        var currentTime = Time.realtimeSinceStartup;

        if (currentTime > _nextFart) {
            if (Random.Range(0, 2) == 1) {
                _aSource.clip = fart1;
            }
            else {
                _aSource.clip = fart2;
            }
            _aSource.Play();
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
            Debug.Log("Game Over");
            Time.timeScale = 0f;
        }

        _nextHit = Time.realtimeSinceStartup + 1f;
    }

    public bool CanSuper() {
        return powerBar.value >= specialAttackPowerCostInPercentage / 100;
    }
}
