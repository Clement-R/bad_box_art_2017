using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float allowedDistance = 32f;
    public float centerToBound = 192;
    public GameObject leftBound;
    public GameObject rightBound;

    private GameObject _player;

    void Start() {
        _player = GameManager.Instance.player;
    }

    void LateUpdate() {
        var current = gameObject.transform.position;
        var playerPos = _player.transform.position;
        var delta = playerPos.x - current.x;
        var goingLeft = Mathf.Sign(delta) < 0;
        if (goingLeft && Mathf.Abs(leftBound.transform.position.x - current.x) < centerToBound)
            return;
        if (!goingLeft && Mathf.Abs(rightBound.transform.position.x - current.x) < centerToBound)
            return;

        if (Mathf.Abs(delta) > allowedDistance) {
            current.x = _player.transform.position.x - (Mathf.Sign(delta) * allowedDistance);
            gameObject.transform.position = current;
        }
    }
}
