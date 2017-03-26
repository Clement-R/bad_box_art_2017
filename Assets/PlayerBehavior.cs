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

        if (Input.GetKeyDown(KeyCode.Space)) {
            _aSource.clip = punch;
            _aSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            _aSource.clip = specialFart;
            _aSource.Play();
        }
    }

    public void takeDamage() {
        if (Time.realtimeSinceStartup < _nextHit)
            return;

        healthBar.value -= 0.1f;
        if (healthBar.value <= 0) {
            Debug.Log("Game Over");
            Time.timeScale = 0f;
        }

        _nextHit = Time.realtimeSinceStartup + 1f;
    }
}
