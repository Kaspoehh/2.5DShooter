using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private bool _paused = false;
    [SerializeField]private GameObject pauseMenu;
    [SerializeField]private GameObject playerUI;
    
    public void Button()
    {
        _paused = !_paused;
        
        if(_paused)
            PauseGame();
        else
            ResumeGame();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
        
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        playerUI.SetActive(false);
    }
    
    private void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
       playerUI.SetActive(true);
    }
}
