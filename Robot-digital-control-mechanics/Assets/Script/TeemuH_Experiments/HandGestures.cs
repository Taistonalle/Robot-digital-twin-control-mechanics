using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System.Linq;  // <-- Add this line

public class HHandGestures : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    bool stopped = true;

    [Header("Sliders")]
    [SerializeField] Slider Z_pos_slider;

    [Header("Settings")]
    [SerializeField] private string parentObjectName = "Point List Annotation";
    [SerializeField] private int landmarkIndex = 0;
    [SerializeField][Range(0.1f, 5f)] float moveAmount = 0.1f;
    [SerializeField][Range(0.1f, 1f)] float loopWaitTime = 0.1f;

    private GameObject parentObject;

    void Start()
    {
        // Initialize speech recognition actions
        actions.Add("Start Movement", StartMovement);
        actions.Add("Stop Movement", StopMovement);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());  // This line uses ToArray()
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
        Debug.Log("Listening for voice commands.");

        parentObject = GameObject.Find(parentObjectName);

        if (parentObject == null)
        {
            Debug.LogError($"Parent object '{parentObjectName}' not found!");
            return;
        }

        if (parentObject.transform.childCount <= landmarkIndex)
        {
            Debug.LogError($"Landmark index {landmarkIndex} is out of range!");
        }
    }

    void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    void StartMovement()
    {
        stopped = false;
        StartCoroutine(UpdateZPosition());
    }

    void StopMovement()
    {
        stopped = true;
        Debug.Log("Movement stopped.");
    }

    IEnumerator UpdateZPosition()
    {
        while (!stopped)
        {
            if (parentObject != null && parentObject.transform.childCount > landmarkIndex)
            {
                Transform child = parentObject.transform.GetChild(landmarkIndex);
                float zPosition = child.localPosition.z;

                Z_pos_slider.value = Mathf.MoveTowards(Z_pos_slider.value, zPosition, moveAmount * Time.deltaTime);

                Debug.Log($"Hand Landmark Z: {zPosition}, Slider Value: {Z_pos_slider.value}");

                yield return new WaitForSeconds(loopWaitTime);
            }
            else
            {
                Debug.LogError($"Landmark index {landmarkIndex} not found or invalid.");
                yield break;
            }
        }
    }

    void Update()
    {
        // Other updates can be added here.
    }
}
