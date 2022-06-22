using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    private GameObject dayBackground;
    private GameObject menuCanvas;
    private GameObject audioManager;

    public float transitionTime=1f;
    public float mainMenuAnimation = 1.5f;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            dayBackground = GameObject.Find("Cheese");
            menuCanvas = GameObject.Find("MainMenu");
            audioManager = GameObject.Find("Audio");
        }
    }

    public void BackToMenuPress()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadLevelRestart(0));
    }

    public void MainMenuPlayPress()
    {
        GameManager.time = 300;
        GameManager.exito = 0;
        GameManager.calidad = 0;
        GameManager.numeroDeRatones = 0;
      StartCoroutine(LoadLevelMainMenu(1));
    }

    public void GameOver()
    {
        StartCoroutine(LoadLevelRestart(2));
    }


    public void RestartPress()
    {
        GameManager.time = 300;
        GameManager.exito = 0;
        GameManager.calidad = 0;
        GameManager.numeroDeRatones = 0;
        StartCoroutine(LoadLevelRestart(1));
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevelRestart(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevelRestart(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevelMainMenu(int levelIndex)
    {

        dayBackground.gameObject.GetComponent<Animator>().SetTrigger("Start");
        menuCanvas.gameObject.GetComponent<Animator>().SetTrigger("Start");
        ButtonPressed();

        yield return new WaitForSeconds(mainMenuAnimation);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void ButtonPressed()
    {
        audioManager.gameObject.GetComponent<AudioSource>().Play();
    }
}
