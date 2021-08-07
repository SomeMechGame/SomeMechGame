using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GauntletTimer : MonoBehaviour{

    public static GauntletTimer sharedInstance;

    public static UnityEvent start  = new UnityEvent();
    public static UnityEvent finish = new UnityEvent();
    public static UnityEvent reset = new UnityEvent();
    public static UnityEvent targetDestroyed = new UnityEvent();

    public TextMeshProUGUI txtTime;
    public TextMeshProUGUI oldTxtTime;

    public TextMeshProUGUI txtTargetsDown;

    private float timeElapsed = 0f;
    private bool startTimer = false;

    private float oldTime = 0f;

    public GameObject[] targets;
    private int numTargetsDown = 0;
    private GameObject player;
    private void Awake() {
        start.AddListener(StartTime);
        finish.AddListener(FinishTime);
        reset.AddListener(Reset);
        targetDestroyed.AddListener(targetDest);
    }

    private void Start() {
        oldTime = PlayerPrefs.GetFloat("BestTime", 999f);
        oldTxtTime.text = oldTime.ToString("F3");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void targetDest(){
        numTargetsDown++;
        txtTargetsDown.text = numTargetsDown.ToString() +"/"+ targets.Length;
    }

    void StartTime(){
        timeElapsed = 0f;
        startTimer = true;

        foreach(GameObject target in targets){
            target.SetActive(true);
        }

        numTargetsDown = 0;
        txtTargetsDown.text = numTargetsDown.ToString() +"/"+ targets.Length;
    }

    void FinishTime(){
        startTimer = false;

        if(timeElapsed < oldTime){
            oldTime = timeElapsed;
            PlayerPrefs.SetFloat("BestTime", timeElapsed);
            oldTxtTime.text = timeElapsed.ToString("F3");
        }

        foreach(GameObject target in targets){
            target.SetActive(false);
        }
    }

    void Reset(){
        startTimer = false;
        timeElapsed = 0f;
        txtTime.text=timeElapsed.ToString("F3");
        player.transform.SetPositionAndRotation(new Vector3(-6, 1.5f, -10),new Quaternion(0,0,0,0));

        foreach(GameObject target in targets){
            target.SetActive(false);
        }

        numTargetsDown = 0;
        txtTargetsDown.text = numTargetsDown.ToString() +"/"+ targets.Length;
    }
    void Update(){
        if(startTimer){
            timeElapsed+=Time.deltaTime;
            txtTime.text=timeElapsed.ToString("F3");
        }
        if(Input.GetKeyDown(KeyCode.R)){
            Reset();
        }
    }
}
