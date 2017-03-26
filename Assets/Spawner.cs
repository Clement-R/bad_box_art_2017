using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    // Spawn n monsters as soon as the previous n monsters have died.
    public int numberBySpawn;
    public GameObject monsterPrefab;

    private List<GameObject> liveMonsters;

	void Start () {
        liveMonsters = new List<GameObject>();
	}
	
	void Update () {
		if(liveMonsters.Count == 0) {
            for(var i = 0; i < numberBySpawn; i++) {
                liveMonsters.Add(GameObject.Instantiate(monsterPrefab, gameObject.transform.position, Quaternion.identity));
            }
        }
        else {
            for(var i = liveMonsters.Count -1; i >= 0; i--) {
                if(liveMonsters[i].GetComponent<MonsterBehavior>().IsDead()) {
                    liveMonsters.RemoveAt(i);
                }    
            }
        }
	}
}
