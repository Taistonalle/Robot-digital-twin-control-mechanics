using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

[Serializable]
public class VoiceMovement : MonoBehaviour {
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    //Sliders
    [SerializeField] Slider X_pos_slider;
    [SerializeField] Slider Y_pos_slider;
    [SerializeField] Slider Z_pos_slider;

    void Start() {
        //Connection
        actions.Add("Connect", FindFirstObjectByType<UI_Ctrl>().TaskOnClick_ConnectBTN);
        actions.Add("Disconnect", FindFirstObjectByType<UI_Ctrl>().TaskOnClick_DisconnectBTN);

        //Movement
        actions.Add("X Up", X_Up);
        actions.Add("X Down", X_Down);
        actions.Add("Left", Left);
        actions.Add("Right", Right);
        actions.Add("Z Up", Z_Up);
        actions.Add("Z Down", Z_Down);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start(); //Without this, voice recognition would not work at all. For optimization it's better to have it behind keybind or a timer to save resources.
        Debug.Log("Listening: " + keywordRecognizer.IsRunning);
    }

    void RecognizedSpeech(PhraseRecognizedEventArgs speech) {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    //Functions that cause robot to do something
    void X_Up() {
        Debug.Log("Robot moves forward!");
        X_pos_slider.value += 50;
    }
    void X_Down() {
        Debug.Log("Robot moves back!");
        X_pos_slider.value -= 50;
    }

    void Left() {
        Debug.Log("Robot moves left!");
        Y_pos_slider.value += 50;

    }
    void Right() {
        Debug.Log("Robot moves right!");
        Y_pos_slider.value -= 50;
    }

    void Z_Up() {
        Debug.Log("Robot moves up!");
        Z_pos_slider.value += 50;
    }

    void Z_Down() {
        Debug.Log("Robot moves down!");
        Z_pos_slider.value -= 50;
    }
}
