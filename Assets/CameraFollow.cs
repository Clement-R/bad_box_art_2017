using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float allowedDistance = 32f;
    private GameObject _player;

    void Start() {
        _player = GameManager.Instance.player;
    }

    void LateUpdate() {
        var current = gameObject.transform.position;
        var playerPos = _player.transform.position;
        var delta = playerPos.x - current.x;
        if(Mathf.Abs(delta) > allowedDistance) {
            current.x = _player.transform.position.x - (Mathf.Sign(delta) * allowedDistance);
            gameObject.transform.position = current;
        }
    }
}
