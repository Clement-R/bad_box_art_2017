using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour {

    Image screen;
    public bool screenVisible = false;
    public float effectDuration = 0.25f;

    void Start() {
        screen = GetComponent<Image>();
    }

    public IEnumerator FadeScreen(bool fade) {
        if (screen.color.a < 1 && fade) {
            float t = 0f;
            while (t < 1) {
                t += Time.deltaTime / effectDuration;
                screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, Mathf.Lerp(0, 1, t));
                yield return null;
            }
            screenVisible = true;
        }

        if (screen.color.a > 0 && !fade) {
            float t = 0f;
            while (t < 1) {
                t += Time.deltaTime / effectDuration;
                screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, Mathf.Lerp(1, 0, t));
                yield return null;
            }
            screenVisible = false;
        }
    }
}
