using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region SINGLETON PATTERN
    public static GameManager Instance { get; private set; }

    void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Initialize();
    }
    #endregion

    public GameObject player;
    public GameObject monsterPrefab;

	public void Initialize() {

    }
	
	public void Update () {
		
	}
}
