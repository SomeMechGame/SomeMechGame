using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauntletCheckpoints : MonoBehaviour{
    
    public bool finish = false;
    private void OnTriggerEnter(Collider other) {
        if(!finish)
            GauntletTimer.start.Invoke();
        else
            GauntletTimer.finish.Invoke();
    }

}
