using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HP : MonoBehaviour{
    public float hp = 100f;
    public TextMeshProUGUI oldTxtTime;

    public string methodToExectute = "";

    public void Damage(float dmg){
        if(methodToExectute.Equals("")){
            hp -= dmg;
            if(hp <= 0){
                GauntletTimer.targetDestroyed.Invoke();
                this.gameObject.SetActive(false);
                //Destroy(this.gameObject);
            }
        }else{
            Invoke(methodToExectute, 0.1f);
        }
    }

    public void Exit(){
        Application.Quit();
    }

    public void ClearTime(){
        PlayerPrefs.SetFloat("BestTime", 999f);
        oldTxtTime.text = PlayerPrefs.GetFloat("BestTime", 999f).ToString("F3");
    }

    public void LoadMenu(){
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
