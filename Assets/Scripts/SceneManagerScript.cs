using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SceneManagerScript : MonoBehaviour
{

    public bool isPlayingIntro = false;
    public bool isPlayingOutro = false;

    public float introDuration = 3f;
    public float outroDuration = 3f;

    PuzzleManagerScript pzMan;
    GameManagerScript gm;

    [Header("UI STUFF")]
    Canvas canvas;
    public GameObject cutsceneOverlay;
    public UICircle progressCircle;
    public Text cutsceneText;
    public GameObject crosshairUI;
    public bool hasCrosshair = true;
    float cutsceneStartTime = -1f;

    void Start(){
        gm = GameObject.FindObjectOfType<GameManagerScript>();
        if(gm == null){
            GameObject bob = new GameObject();
            gm = bob.AddComponent<GameManagerScript>();
            gm.AcquiredCrosshair();
        }
        hasCrosshair = gm.HasCrosshair();
        Cursor.visible = false;
        isPlayingIntro = true;
        cutsceneStartTime = Time.timeSinceLevelLoad;
        cutsceneText.text = "INTRO CUTSCENE IN PROGRESS";
        
        Invoke("SetupPlayMode", introDuration);
    }

    void Update(){
        if (isPlayingIntro){
            // cutsceneOverlay.SetActive(true);
            float cutsceneProgress = (Time.timeSinceLevelLoad - cutsceneStartTime) / introDuration;
            progressCircle.SetProgress(cutsceneProgress);
        }
        else if(isPlayingOutro){
            // cutsceneOverlay.SetActive(true);
            float cutsceneProgress = (Time.timeSinceLevelLoad - cutsceneStartTime) / introDuration;
            progressCircle.SetProgress(cutsceneProgress);
            if(cutsceneProgress > 1f){
                //LOAD NEXT LEVEL
            }
        }
    }

    void SetupPlayMode(){
        if(hasCrosshair){
            crosshairUI.SetActive(true);
        }
        isPlayingIntro = false;
        cutsceneOverlay.SetActive(false);
        GameObject.FindObjectOfType<PlayerInputScript>().Init();
        GameObject.FindObjectOfType<CameraControlScript>().Init();
        GameObject.FindObjectOfType<ItemInteractionScript>().Init();
        GameObject.FindObjectOfType<SimpleMouseRotator>().Init();
        GameObject.FindObjectOfType<BrokenObjectScript>().Init();
        GameObject.FindObjectOfType<UIScript>().Init();

        pzMan = GameObject.FindObjectOfType<PuzzleManagerScript>();
        pzMan.Init();

    }

    public void PuzzleSolved(){
        

        Invoke("PlayOutro", 1f);
    }

    void PlayOutro(){
        cutsceneOverlay.SetActive(true);
        isPlayingOutro = true;
        cutsceneStartTime = Time.timeSinceLevelLoad;
        cutsceneText.text = "OUTRO CUTSCENE IN PROGRESS";
        GameObject.FindObjectOfType<SimpleMouseRotator>().End();
        GameObject.FindObjectOfType<PlayerInputScript>().End();
        GameObject.FindObjectOfType<ItemInteractionScript>().End();
    }
}
