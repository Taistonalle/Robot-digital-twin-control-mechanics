using System.Collections;
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
    Coroutine lineTimer;
    int pointsVisited;

    [Header("Line renderer related")]
    [SerializeField] LineRenderer mazeLineRenderer;
    [SerializeField] MeshCollider mazeLineCollider;
    [SerializeField] float timeOnLine;
    [SerializeField] float timeOutOfLine;

    void Start() {
        displayedTxt = timerObject.GetComponent<TextMeshProUGUI>();
        AddMeshToLine();
    }

    void CompleteTask() {
        //Stop the timer
        taskStarted = false;

        //Calculate the time into minutes and seconds
        int minutes = (int)time / 60;
        decimal seconds = decimal.Round((decimal)time - (minutes * 60), 2);

        //Calculate time spent on line
        timeOnLine = time - timeOutOfLine;
        int onLineMinutes = (int)timeOnLine / 60;
        decimal onLineSeconds = decimal.Round((decimal)timeOnLine - (onLineMinutes * 60), 2);

        //Calculate time spent of the line
        int outOfLineMin = (int)timeOutOfLine / 60;
        decimal outOfLineSeconds = decimal.Round((decimal)timeOutOfLine - (outOfLineMin * 60), 2);

        //Display task method, taks name and final time
        displayedTxt.text = $"{startingText}\nTotal time: {minutes} min {seconds} sec" +
                            $"\nTime on line: {onLineMinutes} min {onLineSeconds} sec" +
                            $"\nTime out of line: {outOfLineMin} min {outOfLineSeconds} sec";
        
    }

    public void AddVisitedPoint() {
        pointsVisited++;
        if (pointsVisited == 8) CompleteTask(); //Notice! Hardcoded value
    }

    public void StartTimer() {
        taskTimer = StartCoroutine(Timer());
    }

    public void StartLineTimer() {
        lineTimer = StartCoroutine(OutOfLineTimer());
    }

    public void StopLineTimer() {
        if (lineTimer == null) return;
        StopCoroutine(lineTimer);
    }

    void AddMeshToLine() {
        //Create empty new mesh
        Mesh lineMesh = new();

        //Bake the drawn line renderer mesh
        mazeLineRenderer.BakeMesh(lineMesh);

        //Set shared mesh of the mesh collider
        mazeLineCollider.sharedMesh = lineMesh;
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

    IEnumerator OutOfLineTimer() {
        //Do the loop until its called to stop. "StopCoroutine(reference)"
        while (true) {
            timeOutOfLine += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
    }
}