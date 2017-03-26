using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject menu;
    public GameObject credits;
    public GameObject instructions;
    public GameObject[] menuItems;
    public Color32 usedColor;

    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    private int index = 0;

    void Start () {
		
	}
	
	void Update () {
        if(menu.activeInHierarchy) {
            if (Input.GetAxisRaw("Vertical") >= 0.1f && Time.time > nextFire) {
                Debug.Log("Vertical down");

                if(index + 1 >= menuItems.Length) {
                    index = 0;
                } else {
                    index++;
                }
                nextFire = Time.time + fireRate;
            }
            else if (Input.GetAxisRaw("Vertical") <= -0.1f && Time.time > nextFire) {
                Debug.Log("Vertical up");

                if (index - 1 < 0) {
                    index = menuItems.Length - 1;
                }
                else {
                    index--;
                }
                nextFire = Time.time + fireRate;
            }

            foreach (var item in menuItems) {
                item.GetComponent<Text>().color = Color.white;
            }

            menuItems[index].GetComponent<Text>().color = usedColor;

            if (Input.GetButtonDown("A_gamepad")) {
                Debug.Log("A");

                if(menuItems[index].name == "Play") {
                    StartCoroutine("launchGame");
                } else if(menuItems[index].name == "Credits") {
                    toggle();
                }
            }
        }
        
        if(credits.activeInHierarchy) {
            if (Input.GetButtonDown("B_gamepad")) {
                Debug.Log("B");
                toggle();
            }
        }
    }

    void toggle() {
        if(menu.activeInHierarchy) {
            menu.SetActive(false);
            credits.SetActive(true);
        } else {
            menu.SetActive(true);
            credits.SetActive(false);
        }
    }

    IEnumerator launchGame() {
        menu.SetActive(false);
        credits.SetActive(false);
        instructions.SetActive(true);

        yield return new WaitForSeconds(6.0f);

        SceneManager.LoadScene("main");
    }
}
