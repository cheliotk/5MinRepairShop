using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    bool hasCrosshair = false;
    public int currentSceneIndex = 0;
    
    public int numberOfLevelScenes = 2;
    UIScript uis;
    SceneManagerScript sceneMan;

    bool isLevelEndCard = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if(isLevelEndCard){

        }
    }

    public bool HasCrosshair(){
        return hasCrosshair;
    }

    public void AcquiredCrosshair(){
        hasCrosshair = true;
    }

    public void SetCurrentLoadedScene(){
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
    }

    public void SceneLoaded(){
        SetCurrentLoadedScene();
        uis = GameObject.FindObjectOfType<UIScript>();
        sceneMan = GameObject.FindObjectOfType<SceneManagerScript>();

        sceneMan.Init();
    }

    public void StartLoadNextScene(){
        if(currentSceneIndex < numberOfLevelScenes){
            GameObject titleCard = GameObject.FindGameObjectWithTag("titleCard");
            if(titleCard != null){
                titleCard.SetActive(true);
            }
            SceneManager.LoadSceneAsync(currentSceneIndex+1, LoadSceneMode.Single);

        }
    }

    public void ExitGame(){
        Application.Quit();
    }
}
