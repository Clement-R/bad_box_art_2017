using Prime31;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour {

    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    public bool isAttacking = false;

    public AudioClip punch;
    public AudioClip death;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    private MonsterIA _ia;
    private AudioSource _aSource;

    [SerializeField]
    private int health = 3;

    private bool touched = false;
    private float _nextHit;
    private bool _dead = false;
    private float _diedAt;
    private PlayerBehavior _player;

    void Awake() {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
        _ia = GetComponent<MonsterIA>();
        _aSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<PlayerBehavior>();

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
    }

    void onTriggerEnterEvent(Collider2D col) {
    }

    void onTriggerStayEvent(Collider2D col) {
        if (col.tag == "Player") {
            if (isAttacking) {
                Debug.Log("Enemy touch player");
                isAttacking = false;
                col.gameObject.GetComponent<PlayerBehavior>().takeDamage();
            }

            if(col.GetComponent<DemoScene>().isAttacking) {
                Debug.Log("Touched");
                takeDamage();
            }
        }
    }


    void onTriggerExitEvent(Collider2D col) {
    }

    #endregion


    void takeDamage() {
        if (Time.realtimeSinceStartup < _nextHit)
            return;
        touched = true;
        health--;
        if(health <= 0){
            _controller.enabled = false;
            _ia.enabled = false;
            _dead = true;
            
            _aSource.clip = death;
            _aSource.Play();
            _animator.SetBool("run", false);
            _animator.SetBool("attack", false);
            /*
            var rb2D = GetComponent<Rigidbody2D>();
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(Vector2.up * 50000, ForceMode2D.Impulse);
            rb2D.AddTorque(90f);
            */

            _diedAt = Time.realtimeSinceStartup;
        }

        _nextHit = Time.realtimeSinceStartup + 1f;
    }

    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update() {
        if(gameObject.transform.position.y <= -100) {
            _dead = true;
            _diedAt = Time.realtimeSinceStartup;
        }
        

        if(_dead && Time.realtimeSinceStartup > _diedAt) {
            _player.IncrementScore();
            GameObject.Destroy(gameObject);
            return;
        }

        if (!_controller.enabled)
            return;

        if (_controller.isGrounded)
            _velocity.y = 0;

        if (_ia.GetKey(KeyCode.Keypad6)) {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (_controller.isGrounded)
                _animator.SetBool("run", true);
        }
        else if (_ia.GetKey(KeyCode.Keypad4)) {
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
        if (_controller.isGrounded && _ia.GetKey(KeyCode.Keypad8)) {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _animator.SetBool("run", false);
        }

        if (_ia.GetKey(KeyCode.Keypad0)) {
            _animator.SetBool("attack", true);
            _aSource.clip = punch;
            _aSource.Play();
            isAttacking = true;
        }

        if (touched) {
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _velocity.x = 100000000;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            
            touched = false;
            return;
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

    void stopAttackAnimation() {
        _animator.SetBool("attack", false);
        isAttacking = false;
    }

    public bool IsDead() {
        return _dead;
    }
}
