using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    [HideInInspector]
    public bool gameIsPaused;

    [SerializeField]
    private CharacterManager _playerCharacter;
    public CharacterManager PlayerCharacter
    {
        get => _playerCharacter;
    }

    // data arrays
    public static CharacterData[] CHARACTER_DATA;

    public Transform CHARACTER_CONTAINER;
    public List<Character> CHARACTERS;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }

        Instance = this;

        DataHandler.LoadGameData();

        gameIsPaused = false;

    }

    private void Update()
    {
        if (gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("Main");
    }


    public void Pause()
    {
        gameIsPaused = true;
        EventManager.TriggerEvent("PauseGame");
    }

    public void Resume()
    {
        gameIsPaused = false;
        EventManager.TriggerEvent("ResumeGame");
    }

    public void Quit() => Application.Quit();

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    /*
    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        DataHandler.SaveGameData();
#endif
    }
    */
}