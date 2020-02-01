using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    CameraControlScript cc;
    ItemInteractionScript ii;
    UIScript uis;

    bool isCutscenePlaying = true;

    public void Init(){
        cc = GetComponent<CameraControlScript>();
        ii = GetComponent<ItemInteractionScript>();
        uis = GameObject.FindObjectOfType<UIScript>();

        isCutscenePlaying = false;
    }

    public void End(){
        isCutscenePlaying = true;
        uis.SetName(null);
    }
    
    void Update()
    {
        if(isCutscenePlaying){
            return;
        }

        if(Input.GetButtonUp("Horizontal")){
            ii.RotateObjectHor(Input.GetAxis("Horizontal"));
        }
        if(Input.GetButtonUp("Vertical")){
            ii.RotateObjectVert(Input.GetAxis("Vertical"));
        }

        GameObject itemCurrentlyLookedAt = ii.itemCurrentlyLookedAt;
        bool isPartOnObject = false;
        if(ii.hasItem
            && itemCurrentlyLookedAt
            && itemCurrentlyLookedAt.GetComponent<ObjectInfo>()
            && itemCurrentlyLookedAt.GetComponent<ObjectInfo>().isBrokenObject){
            isPartOnObject = ii.itemCurrentlyLookedAt.GetComponent<BrokenObjectScript>().CheckPartIsPlacedCorrectly(ii.itemCurrentlyHeld.GetComponent<Part>());
            // print(isPartOnObject);
           ii.HighlightPartOnObject(isPartOnObject);
        }

        uis.SetName(itemCurrentlyLookedAt);

        //     || Input.GetButtonUp("Vertical")){
            
        //     print(Input.GetAxis("Horizontal") + " | " + Input.GetAxis("Vertical"));
        // }

        if(Input.GetMouseButtonUp(0)){
            ii.MouseCLicked(isPartOnObject);
        }
        if(Input.GetMouseButtonUp(1)){
            ii.MouseClickRight();
        }
    }

    public Vector3 GetPositionLookedAt(){
        RaycastHit hit;
        Ray ray = cc.GetRay();
        if(Physics.Raycast(ray, out hit, 6f)){
            Debug.DrawLine(this.transform.position, hit.point, Color.green);
            Vector3 pos = Vector3.Lerp(hit.point, this.transform.position, 0.05f);
            return pos;
        }
        else{
            Debug.DrawRay(this.transform.position, ray.direction * 6f, Color.green);
            return this.transform.position + this.transform.forward * 0.8f;
        }
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
