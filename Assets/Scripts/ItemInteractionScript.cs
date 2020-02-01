using System.Collections;
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
        itemCurrentlyLookedAt = pi.GetObjectCurrentlyLookedAt();

        string itemName = GetItemInfo(itemCurrentlyLookedAt);
        if(itemName != null){
            print(itemName);
        }

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
}
