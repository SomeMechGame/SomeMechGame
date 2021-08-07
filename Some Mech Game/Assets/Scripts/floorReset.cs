using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorReset : MonoBehaviour{
    
    private void OnTriggerEnter(Collider other) {
        GauntletTimer.reset.Invoke();
    }
}
