using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionScript : MonoBehaviour
{
    PlayerInputScript pi;
    CameraControlScript cc;

    public bool hasItem = false;
    GameObject itemCurrentlyHeld;
    GameObject itemCurrentlyLookedAt;
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
