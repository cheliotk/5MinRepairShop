using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectScript : MonoBehaviour
{
    public List<Part> requiredParts;
    List<Vector3> partsCorrectPosition;
    List<Quaternion> partsCorrectRotation;
    public List<GameObject> locationsOnShelf;
    public List<bool> keepPartOnObject;

    public List<GameObject> correctPartTransforms;

    public float distanceThresholdForCorrectPlacement = 0.1f;
    public float angleThresholdForCurrentPlacement = 5f;

    bool isPuzzleSolved = false;

    public void Init(){
        DisassembleObject();
    }

    void DisassembleObject(){
        partsCorrectPosition = new List<Vector3>();
        partsCorrectRotation = new List<Quaternion>();
        correctPartTransforms = new List<GameObject>();

        foreach(Part p in requiredParts){
            partsCorrectPosition.Add(p.transform.localPosition);
            partsCorrectRotation.Add(p.transform.localRotation);

            GameObject g = new GameObject();
            g.name = p.gameObject.name + "_ph";
            g.transform.parent = p.transform.parent;
            g.transform.localPosition = p.transform.localPosition;
            g.transform.localRotation = p.transform.localRotation;

            // g.AddComponent(p.GetComponent<Collider>().GetType());
            Utilities.CopyComponent(p.GetComponent<Collider>(), g);
            Utilities.CopyColliderProperties(p.GetComponent<Collider>(), g.GetComponent<Collider>());

            ObjectInfo oi = g.AddComponent<ObjectInfo>();
            oi.isPartPlaceholder = true;
            g.layer = LayerMask.NameToLayer("partPlaceholder");
            
            correctPartTransforms.Add(g);
        }

        PlacePartsOnShelf();
        TurnOffLocationsOnShelf();
    }

    void TurnOffLocationsOnShelf(){
        for (int i = 0; i < locationsOnShelf.Count; i++)
        {
            GameObject l = locationsOnShelf[i];
            if(!keepPartOnObject[i]){
                l.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    void PlacePartsOnShelf(){
        for (int i = 0; i < requiredParts.Count; i++)
        {
            Part p = requiredParts[i];
            if(!keepPartOnObject[i]){
                p.transform.parent = null;
            }
            p.transform.position = locationsOnShelf[i].transform.position;
            // p.transform.rotation = locationsOnShelf[i].transform.rotation;
        }
    }

    public bool CheckPartIsPlacedCorrectly(Part part){
        bool v = false;
        if(part == null){
            return false;
        }
        int index = requiredParts.FindIndex(p => p == part);

        if(CheckPosition(part.transform.position, partsCorrectPosition[index])
            && CheckRotation(part.transform.rotation, partsCorrectRotation[index])){
            
            v = true;
        }
        
        return v;
    }

    public Vector3 GetCorrectPositionOfPart(Part part){
        int index = requiredParts.FindIndex(p => p == part);
        return transform.TransformPoint(partsCorrectPosition[index]);
    }

    public GameObject GetPlaceholderObjForPart(Part part){
        int index = requiredParts.FindIndex(p => p == part);
        return correctPartTransforms[index];
    }

    bool CheckAllPartsPlaced(){
        bool v = true;
        foreach(Part p in requiredParts){
            if (!CheckPartIsPlacedCorrectly(p)){
                v = false;
            }
        }

        return v;
    }

    public bool PlacePartOnObject(Part part){
        bool val = false;
        part.transform.parent = this.transform;

        int index = requiredParts.FindIndex(p => p == part);
        part.transform.localPosition = partsCorrectPosition[index];
        part.transform.localRotation = partsCorrectRotation[index];
        if(keepPartOnObject[index]){
            val = true;
        }

        if(CheckAllPartsPlaced()){
            GameObject.FindObjectOfType<PuzzleManagerScript>().PuzzleSolved();
            val = false;
        }

        return val;
    }

    bool CheckPosition(Vector3 partPositionInGlobal, Vector3 correctPosition){
        bool v = false;
        Vector3 partPosInLocal = transform.InverseTransformPoint(partPositionInGlobal);
        float dist = Vector3.Distance(partPosInLocal, correctPosition);
        if(dist < distanceThresholdForCorrectPlacement){
            v = true;
        }
        return v;
    }

    bool CheckRotation(Quaternion partRotationInGlobal, Quaternion correctRotation){
        bool v = false;

        Quaternion partRotInLocal = Quaternion.Inverse(this.transform.rotation) * partRotationInGlobal;
        float angle = Quaternion.Angle(partRotInLocal, correctRotation);
        if(angle < angleThresholdForCurrentPlacement){
            v = true;
        }
        return v;
    }
}
