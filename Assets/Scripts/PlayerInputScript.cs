using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    CameraControlScript cc;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CameraControlScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetObjectCurrentlyLookedAt(){
        RaycastHit hit;
        Ray ray = cc.GetRay();
        if(Physics.Raycast(ray, out hit)){
            Debug.DrawLine(this.transform.position, hit.point, Color.green);
            return hit.collider.gameObject;
        }
        else{
            Debug.DrawRay(this.transform.position, ray.direction * 1000f, Color.green);
            return null;
        }
    }
}
