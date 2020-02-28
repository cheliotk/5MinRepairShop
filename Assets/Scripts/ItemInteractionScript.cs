using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionScript : MonoBehaviour
{
    PlayerInputScript pi;
    CameraControlScript cc;
    BrokenObjectScript bos;
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
        bos = GameObject.FindObjectOfType<BrokenObjectScript>();

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
        
        GameObject newItemCurrentlyLooketAt = pi.GetObjectCurrentlyLookedAt(true);
        if(itemCurrentlyLookedAt != newItemCurrentlyLooketAt){
            SwitchItemFocus(itemCurrentlyLookedAt, newItemCurrentlyLooketAt);
        }
        itemCurrentlyLookedAt = newItemCurrentlyLooketAt;

        if(hasItem){
            itemCurrentlyHeld.GetComponent<Collider>().enabled = false;
            // itemCurrentlyHeld.transform.position = pi.GetPositionLookedAt();
            Part p = itemCurrentlyHeld.GetComponent<Part>();
            if(p != null 
                && pi.GetObjectCurrentlyLookedAt(false) == bos.GetPlaceholderObjForPart(p)
                && bos.CheckPartIsPlacedCorrectly(p)){
                itemCurrentlyHeld.transform.position = bos.GetCorrectPositionOfPart(p);
            }
            else{
                itemCurrentlyHeld.transform.position = pi.GetValidPositionLookedAtForObjectHeld(itemCurrentlyHeld);
            }
        }
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
                bool v = GameObject.FindObjectOfType<BrokenObjectScript>().PlacePartOnObject(itemCurrentlyHeld.GetComponent<Part>());
                if (v){
                    itemCurrentlyHeld.GetComponent<Collider>().enabled = true;
                }
            }
            else{
                itemCurrentlyHeld.transform.parent = null;
                // itemCurrentlyHeld.transform.position = pi.GetPositionLookedAt();
                itemCurrentlyHeld.transform.position = pi.GetValidPositionLookedAtForObjectHeld(itemCurrentlyHeld);
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
