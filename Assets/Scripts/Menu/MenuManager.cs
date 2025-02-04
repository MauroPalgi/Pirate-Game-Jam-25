using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //move this downward
    public GameObject MainMenu;

    //scale this up to 150 in 0.3f, then hide it
    public GameObject UIMainMenu;
    public GameObject UIGame;

    private HUDController hUDController;
    public Camera mainCam;
    float scalingUp = 150f;
    float scalingDown = 3f;

    public float downwardsPos;
    public float upwardsPos;

    private bool MainMenuUp = true;

    private void Awake()
    {
        UIMainMenu.gameObject.SetActive(true);
        UIGame.gameObject.SetActive(false);
        hUDController = UIGame.GetComponent<HUDController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMainMenu();
        }
    }

    public void ToggleMainMenu()
    {
        if (MainMenuUp)
        {
            MainMenu.transform.DOMove(
                new Vector3(
                    mainCam.transform.position.x,
                    downwardsPos,
                    mainCam.transform.position.z
                ),
                0.3f
            );
            UIMainMenu
                .transform.DOScale(new Vector3(scalingUp, scalingUp, scalingUp), 0.3f)
                .OnComplete(() => UIMainMenu.gameObject.SetActive(false));
            UIGame.gameObject.SetActive(true);
            MainMenuUp = !MainMenuUp;
            GameManager.Instance.ChangeState(GameState.SpawningLevel);

        }
        else
        {
            Vector3 tempPos = new Vector3(
                mainCam.transform.position.x,
                80,
                mainCam.transform.position.z
            );
            MainMenu.transform.position = tempPos;
            MainMenu.transform.DOMove(
                new Vector3(mainCam.transform.position.x, upwardsPos, mainCam.transform.position.z),
                0.3f
            );

            UIMainMenu.gameObject.SetActive(true);
            UIGame.gameObject.SetActive(false);
            UIMainMenu.transform.DOScale(new Vector3(scalingDown, scalingDown, scalingDown), 0.3f);
            MainMenuUp = !MainMenuUp;
        }
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        Debug.Log(state.ToString());
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    }

    public void OnClickPlay()
    {
        ToggleMainMenu();
    }

    public void UpdateScore(int score)
    {
        if (hUDController != null)
        {
            hUDController.UpdateScore(score);
        }
    }

    internal void UpdateEnemies(int amount)
    {
        if (hUDController != null)
        {
            hUDController.UpdateEnemies(amount);
        }
    }
}
