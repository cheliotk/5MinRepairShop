using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    CameraControlScript cc;
    ItemInteractionScript ii;
    UIScript uis;

    bool isCutscenePlaying = true;

    public LayerMask notPartsLm;
    public LayerMask notPlaceholdersLm;
    [Range(1f,10f)]
    public float lookDist = 5f;

    [Range(0f,1f)]
    float objDistFromCamera = 0.5f;

    BrokenObjectScript bos;

    public void Init(){
        cc = GetComponent<CameraControlScript>();
        ii = GetComponent<ItemInteractionScript>();
        uis = GameObject.FindObjectOfType<UIScript>();

        bos = GameObject.FindObjectOfType<BrokenObjectScript>();

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

        objDistFromCamera += Input.mouseScrollDelta.y * 0.01f;
        if(objDistFromCamera < 0.1f)
            objDistFromCamera = 0.1f;
        if(objDistFromCamera > 1f)
            objDistFromCamera = 1f;

        GameObject itemCurrentlyLookedAt = ii.itemCurrentlyLookedAt;
        bool isPartOnObject = false;
        if(ii.hasItem
            && GetObjectCurrentlyLookedAt(false)
            && GetObjectCurrentlyLookedAt(false).GetComponent<ObjectInfo>()
            && GetObjectCurrentlyLookedAt(false).GetComponent<ObjectInfo>().isPartPlaceholder){
            isPartOnObject = bos.CheckPartIsPlacedCorrectly(ii.itemCurrentlyHeld.GetComponent<Part>());
            // print(isPartOnObject);
           ii.HighlightPartOnObject(isPartOnObject);
        }

        uis.SetName(itemCurrentlyLookedAt);

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
        if(Physics.Raycast(ray, out hit, lookDist, notPartsLm)){
            Debug.DrawLine(this.transform.position, hit.point, Color.green);
            Vector3 pos = Vector3.Lerp(this.transform.position, hit.point, objDistFromCamera);
            return pos;
        }
        else{
            Debug.DrawRay(this.transform.position, ray.direction * lookDist, Color.green);
            return this.transform.position + this.transform.forward * objDistFromCamera * lookDist;
        }
    }

    public Vector3 GetValidPositionLookedAtForObjectHeld(GameObject objHeld){
        // Part p = objHeld.GetComponent<Part>();
        // if(p != null){
        //     if(bos.CheckPartIsPlacedCorrectly(p)){
        //         return bos.GetCorrectPositionOfPart(p);
        //     }
        // }
        RaycastHit hit;
        Ray ray = cc.GetRay();
        if(Physics.Raycast(ray, out hit, lookDist, notPartsLm)){
            Debug.DrawLine(this.transform.position, hit.point, Color.green);
            return GetValidPosFromRay(objHeld, ray, hit.point);
        }
        else{
            Debug.DrawRay(this.transform.position, ray.direction * lookDist, Color.red);
            return GetValidPosFromRay(objHeld, ray, this.transform.position + ray.direction * lookDist);
        }
    }

    Vector3 GetValidPosFromRay(GameObject objHeld, Ray ray, Vector3 point){
        Vector3 pointTest = point;
        Vector3 moveDir = -ray.direction;
        Debug.DrawRay(pointTest, moveDir.normalized, Color.cyan);
        while(CheckObjIsOverlapping(objHeld, pointTest)){
            pointTest += moveDir.normalized*0.1f;
        }
        int i = 1;
        while(!CheckObjIsOverlapping(objHeld, pointTest) && i < 10){
            pointTest -= moveDir.normalized*0.01f;
            i++;
        }
        return pointTest;
    }

    bool CheckObjIsOverlapping(GameObject obj, Vector3 point){
        Collider c = obj.GetComponent<Collider>();
        c.enabled = true;
        Bounds b = c.bounds;
        c.enabled = false;
        bool v = Physics.OverlapBox(point, b.extents, Quaternion.Euler(0f,0f,0f), notPartsLm).Length != 0;
        if(v){
            Debug.DrawRay(point, Vector3.up, Color.red);
        }
        else{
            Debug.DrawRay(point, Vector3.up, Color.green);
        }
        return v;
    }

    public GameObject GetObjectCurrentlyLookedAt(bool ignorePlaceholders){
        LayerMask lmToUse = ~0;
        if(ignorePlaceholders){
            lmToUse = notPlaceholdersLm;
        }

        RaycastHit hit;
        Ray ray = cc.GetRay();
        
        if(Physics.Raycast(ray, out hit, 500f, lmToUse)){
            return hit.collider.gameObject;
        }
        else{
            return null;
        }

        // if(ignorePlaceholders){
        //     if(Physics.Raycast(ray, out hit, 500f, notPlaceholdersLm)){
        //         Debug.DrawLine(this.transform.position, hit.point);
        //         return hit.collider.gameObject;
        //     }
        //     else{
        //         return null;
        //     }
        // }
        // else{
        //     if(Physics.Raycast(ray, out hit)){
        //         Debug.DrawLine(this.transform.position, hit.point);
        //         return hit.collider.gameObject;
        //     }
        //     else{
        //         return null;
        //     }
        // }

        // if(Physics.Raycast(ray, out hit)){
        //     ObjectInfo oi = hit.collider.gameObject.GetComponent<ObjectInfo>();
        //     if(ignorePlaceholders && oi != null && oi.isPartPlaceholder){
        //     }
        //     return hit.collider.gameObject;
        // }
        // else{
        //     return null;
        // }
    }
}
