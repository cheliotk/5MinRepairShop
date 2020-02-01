using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionScript : MonoBehaviour
{
    PlayerInputScript pi;
    CameraControlScript cc;

    [Header("Item Manipulation Properties")]
    public float angleToRotateObject = 45f;

    [Header("Item Held Properties")]
    public bool hasItem = false;
    public GameObject itemCurrentlyHeld;

    public GameObject itemCurrentlyLookedAt;

    [Header("UI info")]
    public Text itemCurrentlyLookedAt_uiName;
    public Text itemCurrentlyLookedAt_uiDescription;
    public bool showName = false;
    public bool showDescription = false;

    bool isCutscenePlaying = true;

    public void Init(){
        pi = GetComponent<PlayerInputScript>();
        cc = GetComponent<CameraControlScript>();

        isCutscenePlaying = false;
    }

    public void End(){
        isCutscenePlaying = true;
        DehighlightObjectLookedAt();
    }

    // Update is called once per frame
    void Update()
    {
        if(isCutscenePlaying){
            return;
        }
        
        GameObject newItemCurrentlyLooketAt = pi.GetObjectCurrentlyLookedAt();
        if(itemCurrentlyLookedAt != newItemCurrentlyLooketAt){
            SwitchItemFocus(itemCurrentlyLookedAt, newItemCurrentlyLooketAt);
        }
        itemCurrentlyLookedAt = newItemCurrentlyLooketAt;

        if(itemCurrentlyLookedAt != null){
            showName = true;
        }
        else{
            showName = false;
        }

        if(hasItem){
            itemCurrentlyHeld.GetComponent<Collider>().enabled = false;
            itemCurrentlyHeld.transform.position = pi.GetPositionLookedAt();

            // if(Input.GetMouseButtonUp(0)){
            //     itemCurrentlyHeld.transform.position = pi.GetPositionLookedAt();
            //     itemCurrentlyHeld.GetComponent<Collider>().enabled = true;

            //     itemCurrentlyHeld = null;
            //     hasItem = false;
            // }
        }
        // else{
        //     if(Input.GetMouseButtonUp(0)){
        //         if(itemCurrentlyLookedAt != null){
        //             ObjectInfo oi = itemCurrentlyLookedAt.GetComponent<ObjectInfo>();
        //             if(oi != null && oi.isPickable){
        //                 hasItem = true;
        //                 itemCurrentlyHeld = itemCurrentlyLookedAt;
        //             }
        //         }
        //     }
        // }
    }

    string GetItemInfo(GameObject item){
        if(item != null){
            ObjectInfo oi = item.GetComponent<ObjectInfo>();
            if(oi != null){
                return oi.name;
            }
            else{
                return item.name;
            }
        }
        else{
            return null;
        }
    }

    public void MouseCLicked(bool isPartOnObject){
        if(hasItem){
            if(isPartOnObject){
                HighlightPartOnObject(false);
                itemCurrentlyLookedAt.GetComponent<BrokenObjectScript>().PlacePartOnObject(itemCurrentlyHeld.GetComponent<Part>());
            }
            else{
                itemCurrentlyHeld.transform.position = pi.GetPositionLookedAt();
                itemCurrentlyHeld.GetComponent<Collider>().enabled = true;
            }
            itemCurrentlyHeld = null;
            hasItem = false;
        }
        else{
            if(itemCurrentlyLookedAt != null){
                ObjectInfo oi = itemCurrentlyLookedAt.GetComponent<ObjectInfo>();
                if(oi != null && oi.isPickable){
                    hasItem = true;
                    itemCurrentlyHeld = itemCurrentlyLookedAt;
                }
            }
        }
    }

    public void RotateObjectVert(float axisValue){
        if(!hasItem)
            return;

        itemCurrentlyHeld.transform.Rotate(Vector3.right, Mathf.Sign(axisValue) * angleToRotateObject, Space.World);
    }

    public void RotateObjectHor(float axisValue){
        if(!hasItem)
            return;

        itemCurrentlyHeld.transform.Rotate(Vector3.up, -1 * Mathf.Sign(axisValue) * angleToRotateObject, Space.World);

    }

    void SwitchItemFocus(GameObject previousObj, GameObject newObj){
        if(previousObj != null){
            ObjectInfo poi = previousObj.GetComponent<ObjectInfo>();
            previousObj.GetComponent<Outline>().enabled = false;
        }
        if(newObj != null){
            ObjectInfo noi = newObj.GetComponent<ObjectInfo>();
            AddHighlightToObject(newObj);
        }

    }

    void AddHighlightToObject(GameObject obj){
        Outline outline = obj.GetComponent<Outline>();
        if(outline == null){
            outline = obj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
        }

        outline.enabled = true;
    }

    public void HighlightPartOnObject(bool highlight){
        Outline outline = itemCurrentlyHeld.GetComponent<Outline>();
        if(highlight){
            itemCurrentlyLookedAt.GetComponent<Outline>().enabled = false;
            outline.enabled = true;
            outline.OutlineColor = Color.green;
        }
        else{
            itemCurrentlyLookedAt.GetComponent<Outline>().enabled = true;
            outline.enabled = false;
        }
    }

    void DehighlightObjectLookedAt(){
        itemCurrentlyLookedAt.GetComponent<Outline>().enabled = false;
    }
}
