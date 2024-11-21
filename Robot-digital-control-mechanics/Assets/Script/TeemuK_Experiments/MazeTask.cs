using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class MazeTask : MonoBehaviour {
    [SerializeField] GameObject timerObject;
    [SerializeField] TextMeshProUGUI displayedTxt;

    [TextArea(5, 10)]
    [SerializeField] string startingText;

    [SerializeField] bool taskStarted;
    [SerializeField] float time;

    Coroutine taskTimer;
    int pointsVisited;

    void Start() {
        displayedTxt = timerObject.GetComponent<TextMeshProUGUI>();
    }

    void CompleteTask() {
        //Stop the timer
        taskStarted = false;

        //Calculate the time into minutes and seconds
        int minutes = (int)time / 60;
        int seconds = Mathf.RoundToInt(time - (minutes * 60));

        //Display task method, taks name and final time
        displayedTxt.text = $"{startingText}\nTime: {minutes} min {seconds} sec";
        
    }

    public void AddVisitedPoint() {
        pointsVisited++;
        if (pointsVisited == 8) CompleteTask(); //Notice! Hardcoded value
    }

    public void StartTimer() {
        taskTimer = StartCoroutine(Timer());
    }

    IEnumerator Timer() {
        taskStarted = true;
        while (taskStarted) {
            time += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        Debug.Log("Timer finished");
        CompleteTask();
    }
}