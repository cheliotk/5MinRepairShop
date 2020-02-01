using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    Camera cam;

    public void Init(){
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Ray GetRay(){
        return cam.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height/2f, 0f));
    }

    public Vector3 GetScreenPoint(Vector3 worldPoint){
        return cam.WorldToScreenPoint(worldPoint);
    }
}
