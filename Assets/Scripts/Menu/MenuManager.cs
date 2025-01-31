using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    //move this downward
    public GameObject MainMenu;
    //scale this up to 150 in 0.3f, then hide it
    public GameObject UIMainMenu;

    public Camera mainCam;

    float scalingUp = 150f;
    float scalingDown = 3f;

    public float downwardsPos;
    public float upwardsPos;

    private bool MainMenuUp = true;
    

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            ToggleMainMenu();
        }
    }
    public void ToggleMainMenu(){
        if(MainMenuUp){
            MainMenu.transform.DOMove(new Vector3(mainCam.transform.position.x,downwardsPos,mainCam.transform.position.z), 0.3f);
            UIMainMenu.transform.DOScale( new Vector3(scalingUp,scalingUp,scalingUp),0.3f)
                .OnComplete(() => UIMainMenu.gameObject.SetActive(false)); 
            
            MainMenuUp = !MainMenuUp;
        }else{
            Vector3 tempPos = new Vector3(mainCam.transform.position.x,80,mainCam.transform.position.z);
            MainMenu.transform.position = tempPos;
            MainMenu.transform.DOMove(new Vector3(mainCam.transform.position.x,upwardsPos,mainCam.transform.position.z), 0.3f);
            UIMainMenu.gameObject.SetActive(true);
            UIMainMenu.transform.DOScale( new Vector3(scalingDown,scalingDown,scalingDown),0.3f);

            MainMenuUp = !MainMenuUp;
        }
    }

    public void StartGame(){

        //after .3f start the game
        GameManager.Instance.ChangeState(GameState.Starting);
    }


}
