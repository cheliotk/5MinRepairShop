using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Ray GetRay(){
        return cam.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height/2f, 0f));
    }
}
