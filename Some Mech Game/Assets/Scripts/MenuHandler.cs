using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuHandler : MonoBehaviour{

    public TextMeshProUGUI txtSensV;
    public TextMeshProUGUI txtSensH;

    public TextMeshProUGUI txtJump;
    public TextMeshProUGUI txtSprint;
    public TextMeshProUGUI txtCrouch;
    private int control;
    public void VerticalSensSlider(float value){
        PlayerPrefs.SetFloat("VerticalSens", value * 100f);
        txtSensV.text = value.ToString("F2");
    }

    public void HorizontalSensSlider(float value){
        PlayerPrefs.SetFloat("HorizontalSens", value * 100f);
        txtSensH.text = value.ToString("F2");
    }

    public void LoadGauntlet(){
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Exit(){
        Application.Quit();
    }

    private void Start() {
        txtSensV.text = (PlayerPrefs.GetFloat("VeticalSens", 100f) / 100).ToString("F2");
        txtSensH.text = (PlayerPrefs.GetFloat("HorizontalSens", 100f) / 100).ToString("F2");

        txtJump.text = PlayerPrefs.GetString("JumpKey" , "Listen").Substring(8);
        txtSprint.text = PlayerPrefs.GetString("SprintKey" , "Listen").Substring(8);
        txtCrouch.text = PlayerPrefs.GetString("CrouchKey" , "Listen").Substring(8);
    }

    public void OnGUI(){
        foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode))){
            if (Input.GetKey(kcode)){
                if((kcode != KeyCode.Mouse0) || (kcode != KeyCode.Mouse0)){
                    if(control == 0){
                        PlayerPrefs.SetString("JumpKey","KeyCode."+kcode);
                        txtJump.text = kcode.ToString();
                        control = -1;
                        return;
                    }else if(control == 1){
                        PlayerPrefs.SetString("SprintKey","KeyCode."+kcode);
                        txtSprint.text = kcode.ToString();
                        control = -1;
                        return;
                    }else if(control == 2){
                        PlayerPrefs.SetString("CrouchKey","KeyCode."+kcode);
                        txtCrouch.text = kcode.ToString();
                        control = -1;
                        return;
                    }
                }
            }
        }
    }

    public void ControlToBind(int control){
        this.control = control;
    }
}
