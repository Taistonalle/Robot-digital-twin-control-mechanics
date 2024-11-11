using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class VoiceMovement_Continuous : MonoBehaviour {
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    bool stopped = true;

    [Header("Values")]
    [SerializeField][Range(0.1f, 5f)] float moveAmount;
    [SerializeField][Range(0.1f, 5f)] float rotationAmount;
    [SerializeField][Range(0.1f, 1f)] float loopWaitTime;

    [Header("Sliders")]
    [SerializeField] Slider X_pos_slider;
    [SerializeField] Slider Y_pos_slider;
    [SerializeField] Slider Z_pos_slider;
    [SerializeField] Slider X_rot_slider;
    [SerializeField] Slider Y_rot_slider;
    [SerializeField] Slider Z_rot_slider;

    void Start() {
        //Connection
        actions.Add("Connect", FindFirstObjectByType<UI_Ctrl>().TaskOnClick_ConnectBTN);
        actions.Add("Disconnect", FindFirstObjectByType<UI_Ctrl>().TaskOnClick_DisconnectBTN);

        //Movement
        actions.Add("X Up", X_Up);
        actions.Add("Forward", X_Up); //Alternative
        actions.Add("X Down", X_Down);
        actions.Add("Back", X_Down); //Alternative
        actions.Add("Y Up", Y_Up);
        actions.Add("Left", Y_Up); //Alternative
        actions.Add("Y Down", Y_Down);
        actions.Add("Right", Y_Down); //Alternative
        actions.Add("Z Up", Z_Up);
        actions.Add("Height Up", Z_Up); //Alternative
        actions.Add("Z Down", Z_Down);
        actions.Add("Height Down", Z_Down); //Alternative

        //Rotation
        actions.Add("RX Up", RX_Up);
        actions.Add("RX Down", RX_Down);
        actions.Add("RY Up", RY_Up);
        actions.Add("RY Down", RY_Down);
        actions.Add("RZ Up", RZ_Up);
        actions.Add("RZ Down", RZ_Down);

        actions.Add("Stop", StopMove);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start(); //Without this, voice recognition would not work at all. For optimization it's better to have it behind keybind or a timer to save resources.
        Debug.Log("Listening: " + keywordRecognizer.IsRunning);
    }

    void RecognizedSpeech(PhraseRecognizedEventArgs speech) {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    //Based on keyword, start a routine that loops until "Stop" keyword is heard.
    void X_Up() {
        stopped = false;
        StartCoroutine(X_Up_Routine());
    }

    void X_Down() {
        stopped = false;
        StartCoroutine(X_Down_Routine());
    }

    void Y_Up() {
        stopped = false;
        StartCoroutine(Y_Up_Routine());
    }

    void Y_Down() {
        stopped = false;
        StartCoroutine(Y_Down_Routine());
    }

    void Z_Up() {
        stopped = false;
        StartCoroutine(Z_Up_Routine());
    }

    void Z_Down() {
        stopped = false;
        StartCoroutine(Z_Down_Routine());
    }

    void RX_Up() {
        stopped = false;
        StartCoroutine(RX_Up_Routine());
    }

    void RX_Down() {
        stopped = false;
        StartCoroutine(RX_Down_Routine());
    }

    void RY_Up() {
        stopped = false;
        StartCoroutine(RY_Up_Routine());
    }

    void RY_Down() {
        stopped = false;
        StartCoroutine(RY_Down_Routine());
    }

    void RZ_Up() {
        stopped = false;
        StartCoroutine(RZ_Up_Routine());
    }

    void RZ_Down() {
        stopped = false;
        StartCoroutine(RZ_Down_Routine());
    }

    IEnumerator X_Up_Routine() {
        while (!stopped) {
            //Debug.Log("While loop running");
            X_pos_slider.value += moveAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        //Debug.Log("loop ended");
        yield return null;
    }

    IEnumerator X_Down_Routine() {
        while (!stopped) {
            X_pos_slider.value -= moveAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator Y_Up_Routine() {
        while (!stopped) {
            Y_pos_slider.value += moveAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator Y_Down_Routine() {
        while (!stopped) {
            Y_pos_slider.value -= moveAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator Z_Up_Routine() {
        while (!stopped) {
            Z_pos_slider.value += moveAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator Z_Down_Routine() {
        while (!stopped) {
            Z_pos_slider.value -= moveAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator RX_Up_Routine() {
        while (!stopped) {
            X_rot_slider.value += rotationAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator RX_Down_Routine() {
        while (!stopped) {
            X_rot_slider.value -= rotationAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator RY_Up_Routine() {
        while (!stopped) {
            Y_rot_slider.value += rotationAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator RY_Down_Routine() {
        while (!stopped) {
            Y_rot_slider.value -= rotationAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator RZ_Up_Routine() {
        while (!stopped) {
            Z_rot_slider.value += rotationAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    IEnumerator RZ_Down_Routine() {
        while (!stopped) {
            Z_rot_slider.value -= rotationAmount;
            yield return new WaitForSeconds(loopWaitTime);
        }
        yield return null;
    }

    void StopMove() {
        stopped = true;
        Debug.Log("Movement stopped");
    }
}
