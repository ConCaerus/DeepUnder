using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    private void Start() {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("PlayButton").clicked += play;
    }

    void play() {
        SceneManager.LoadScene("Cave");
    }
}
