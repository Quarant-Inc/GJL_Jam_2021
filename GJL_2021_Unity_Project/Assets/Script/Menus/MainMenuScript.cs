using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public AK.Wwise.Event clickSound = null;
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject exitButton;
    public Text lblLoading;
    GameObject playTextGO;
    GameObject optionsTextGO;
    GameObject exitTextGO;


    // Start is called before the first frame update
    void Start()
    {
        playTextGO = playButton.GetComponentInChildren<Text>().gameObject;
        optionsTextGO = optionsButton.GetComponentInChildren<Text>().gameObject;
        exitTextGO = exitButton.GetComponentInChildren<Text>().gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        playButton.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
        clickSound.Post(gameObject);
        StartCoroutine(DelayPlayGame());
    }

    IEnumerator DelayPlayGame()
    {
        yield return new WaitForSeconds(0.04f);
        playButton.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
        playButton.SetActive(false);
        optionsButton.SetActive(false);
        exitButton.SetActive(false);
        lblLoading.gameObject.SetActive(true);
        //SceneManager.LoadScene("MainGame");
        Util.LoadScene(SCENE.MAINGAME);
    }

    public void GoOptions()
    {
        optionsButton.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
        clickSound.Post(gameObject);
        StartCoroutine(DelayGoOptions());
    }

    IEnumerator DelayGoOptions()
    {
        yield return new WaitForSeconds(0.04f);
        optionsButton.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
        //SceneManager.LoadScene("Options");
        Util.LoadScene(SCENE.OPTIONS);
    }

    public void ExitGame()
    {
        exitButton.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
        clickSound.Post(gameObject);
        StartCoroutine(DelayExitGame());
    }

    IEnumerator DelayExitGame()
    {
        yield return new WaitForSeconds(0.04f);
        exitButton.GetComponentInChildren<Text>().alignment = TextAnchor.UpperCenter;
        Util.Quit();
    }

}
