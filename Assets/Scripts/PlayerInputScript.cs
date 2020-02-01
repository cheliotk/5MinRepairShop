﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    CameraControlScript cc;
    ItemInteractionScript ii;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CameraControlScript>();
        ii = GetComponent<ItemInteractionScript>();
    }

    // Update is called once per frame
    void Update()
    {
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

        //     || Input.GetButtonUp("Vertical")){
            
        //     print(Input.GetAxis("Horizontal") + " | " + Input.GetAxis("Vertical"));
        // }

        if(Input.GetMouseButtonUp(0)){
            ii.MouseCLicked(isPartOnObject);
        }
    }

    public Vector3 GetPositionLookedAt(){
        RaycastHit hit;
        Ray ray = cc.GetRay();
        if(Physics.Raycast(ray, out hit)){
            Debug.DrawLine(this.transform.position, hit.point, Color.green);
            Vector3 pos = Vector3.Lerp(hit.point, this.transform.position, 0.1f);
            return pos;
        }
        else{
            Debug.DrawRay(this.transform.position, ray.direction * 1000f, Color.green);
            return this.transform.position + this.transform.forward * 4f;
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
