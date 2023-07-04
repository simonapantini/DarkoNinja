using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour

{
    [SerializeField] private AudioClip menuMusic;
    public string lvl1ToLoad;
    public string lvl2ToLoad;
    public int defaultLives;

    private void Start()
    {
        SoundManager.instance.PlaySound(menuMusic);
        PlayerPrefs.SetInt("CurrentHealth", defaultLives);
    }

    public void StartGame1()
    {
        SceneManager.LoadScene(lvl1ToLoad);
    }

    public void StartGame2()
    {
        SceneManager.LoadScene(lvl2ToLoad);
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }
}
