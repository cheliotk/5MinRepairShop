using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class SceneManagerScript : MonoBehaviour
{

    public bool isPlayingIntro = false;
    public bool isPlayingOutro = false;

    public float introDuration = 3f;
    public float outroDuration = 3f;

    PuzzleManagerScript pzMan;

    void Start(){
        isPlayingIntro = true;

        Invoke("SetupPlayMode", introDuration);
    }

    void SetupPlayMode(){
        isPlayingIntro = false;
        GameObject.FindObjectOfType<PlayerInputScript>().Init();
        GameObject.FindObjectOfType<CameraControlScript>().Init();
        GameObject.FindObjectOfType<ItemInteractionScript>().Init();
        GameObject.FindObjectOfType<SimpleMouseRotator>().Init();
        GameObject.FindObjectOfType<BrokenObjectScript>().Init();

        pzMan = GameObject.FindObjectOfType<PuzzleManagerScript>();
        pzMan.Init();

    }

    public void PuzzleSolved(){
        isPlayingOutro = true;

        Invoke("PlayOutro", 1f);
    }

    void PlayOutro(){
        GameObject.FindObjectOfType<SimpleMouseRotator>().End();
    }
}
