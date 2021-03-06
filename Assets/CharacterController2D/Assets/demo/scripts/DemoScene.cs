﻿using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.SceneManagement;

public class DemoScene : MonoBehaviour {
    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    public bool isAttacking = false;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    private PlayerBehavior _player;
    private bool canMove = true;

    void Awake() {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
        _player = GetComponent<PlayerBehavior>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        _controller.onTriggerStayEvent += onTriggerStayEvent;
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit) {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col) {
    }


    void onTriggerExitEvent(Collider2D col) {
    }

    void onTriggerStayEvent(Collider2D col) {
        if (col.tag == "Enemy") {
            if (isAttacking) {
                isAttacking = false;
            }
        }
    }

    #endregion


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update() {
        if(_player.gameOver) {
            if (Input.GetButtonDown("A_gamepad")) {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene("main");
            }
        } else {
            if (_controller.isGrounded)
                _velocity.y = 0;

            if (_animator.GetBool("special_attack")) {
                _animator.SetBool("special_attack", false);
            }

            if (Input.GetAxisRaw("Horizontal") >= 0.1f && canMove) {
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                if (_controller.isGrounded)
                    _animator.SetBool("run", true);
            }
            else if (Input.GetAxisRaw("Horizontal") <= -0.1f && canMove) {
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                if (_controller.isGrounded)
                    _animator.SetBool("run", true);
            }
            else {
                normalizedHorizontalSpeed = 0;

                if (_controller.isGrounded)
                    _animator.SetBool("run", false);
            }


            // we can only jump whilst grounded
            if (_controller.isGrounded && Input.GetButtonDown("A_gamepad")) {
                _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                _animator.SetBool("run", false);
            }

            if (Input.GetButtonDown("B_gamepad")) {
                if (!_animator.GetBool("attack")) {
                    _animator.SetBool("attack", true);
                    isAttacking = true;
                }
            }

            if (Input.GetButtonDown("Y_gamepad")) {
                var player = GetComponent<PlayerBehavior>();
                if (!player.CanSuper())
                    return;

                if (!_animator.GetBool("special_attack")) {
                    _animator.SetBool("special_attack", true);
                    isAttacking = true;
                }
            }

            // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
            var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

            // apply gravity before moving
            _velocity.y += gravity * Time.deltaTime;

            _controller.move(_velocity * Time.deltaTime);

            // grab our current _velocity to use as a base for all calculations
            _velocity = _controller.velocity;
        }
    }

    void stopAttackAnimation() {
        _animator.SetBool("attack", false);
        isAttacking = false;
    }

    void stopSpecialAttackAnimation() {
        _animator.SetBool("special_attack", false);
        isAttacking = false;
    }
}
