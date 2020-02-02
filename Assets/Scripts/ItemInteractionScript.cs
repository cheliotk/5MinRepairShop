using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionScript : MonoBehaviour
{
    PlayerInputScript pi;
    CameraControlScript cc;
    UIScript uis;

    [Header("Item Manipulation Properties")]
    public float angleToRotateObject = 45f;

    [Header("Item Held Properties")]
    public bool hasItem = false;
    public GameObject itemCurrentlyHeld;

    public GameObject itemCurrentlyLookedAt;

    bool isCutscenePlaying = true;

    public void Init(){
        pi = GetComponent<PlayerInputScript>();
        cc = GetComponent<CameraControlScript>();
        uis = GameObject.FindObjectOfType<UIScript>();

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

        // string itemName = GetItemInfo(itemCurrentlyLookedAt);
        // if(itemName != null){
        //     print(itemName);
        // }

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

    public string GetItemInfo(GameObject item){
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

    public string GetItemDescription(GameObject item){
        if(item != null){
            ObjectInfo oi = item.GetComponent<ObjectInfo>();
            if(oi != null){
                return oi.description;
            }
            else{
                return null;
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
                bool v = itemCurrentlyLookedAt.GetComponent<BrokenObjectScript>().PlacePartOnObject(itemCurrentlyHeld.GetComponent<Part>());
                if (v){
                    itemCurrentlyHeld.GetComponent<Collider>().enabled = true;
                }
            }
            else{
                itemCurrentlyHeld.transform.parent = null;
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

    public void MouseClickRight(){
        uis.SetDescription(itemCurrentlyLookedAt);
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
            uis.SetDescription(null);
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
            outline.OutlineColor = Color.yellow;
            outline.enabled = false;
        }
    }

    void DehighlightObjectLookedAt(){
        itemCurrentlyLookedAt.GetComponent<Outline>().enabled = false;
        uis.SetDescription(null);
    }
}
