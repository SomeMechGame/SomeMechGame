using UnityEngine;
using TMPro;
public class TimeClear : MonoBehaviour{
    public float hp = 100f;
    public TextMeshProUGUI oldTxtTime;
    public void Damage(float dmg){
        hp -= dmg;
        if(hp<=0){
            PlayerPrefs.SetFloat("BestTime", 999f);
            oldTxtTime.text = PlayerPrefs.GetFloat("BestTime", 999f).ToString("F3");
        }
    }
}
