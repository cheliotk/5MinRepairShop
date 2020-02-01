using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIScript : MonoBehaviour
{
    ItemInteractionScript ii;
    CameraControlScript cc;
    [Header("References To Objects")]
    public Text objectName;
    public Text objectDescription;
    public UILineRenderer objectNameLine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(){
        ii = GameObject.FindObjectOfType<ItemInteractionScript>();
        cc = GameObject.FindObjectOfType<CameraControlScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName(GameObject itemLookedAt){

        string n = ii.GetItemInfo(itemLookedAt);
        objectName.text = n;
        if(n == "" || n == null){
            objectName.gameObject.SetActive(false);
            objectNameLine.gameObject.SetActive(false);
        }
        else{
            objectName.gameObject.SetActive(true);
            objectName.gameObject.transform.position = cc.GetScreenPoint(itemLookedAt.transform.position);

            objectNameLine.gameObject.SetActive(true);
            Vector3 bob = Camera.main.WorldToViewportPoint(itemLookedAt.transform.position);
            List<Vector2> pointsList = new List<Vector2>(objectNameLine.Points);
            pointsList[0] = new Vector2(0.5f, 0.5f);
            pointsList[1] = new Vector2(0.5f, bob.y - 0.02f);
            pointsList[2] = new Vector2(bob.x, bob.y - 0.02f);
            objectNameLine.Points = pointsList.ToArray();
            // objectNameLine.SetAllDirty();
        }
    }

    public void SetDescription(string description){
        objectDescription.text = description;
        if(description == ""){
            objectDescription.gameObject.SetActive(false);
        }
        else{
            objectDescription.gameObject.SetActive(true);
        }
    }
}
