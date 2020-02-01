﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        pi = GetComponent<PlayerInputScript>();
        cc = GetComponent<CameraControlScript>();
    }

    // Update is called once per frame
    void Update()
    {
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

            if(Input.GetMouseButtonUp(0)){
                itemCurrentlyHeld.transform.position = pi.GetPositionLookedAt();
                itemCurrentlyHeld.GetComponent<Collider>().enabled = true;

                itemCurrentlyHeld = null;
                hasItem = false;
            }
        }
        else{
            if(Input.GetMouseButtonUp(0)){
                if(itemCurrentlyLookedAt != null){
                    ObjectInfo oi = itemCurrentlyLookedAt.GetComponent<ObjectInfo>();
                    if(oi != null && oi.isPickable){
                        hasItem = true;
                        itemCurrentlyHeld = itemCurrentlyLookedAt;
                    }
                }
            }
        }
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
}
