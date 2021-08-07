using UnityEngine;

public class MoveCamera : MonoBehaviour{
    [SerializeField] Transform cameraPosition = null;

    void Update(){
        transform.position = cameraPosition.position;
    }
}