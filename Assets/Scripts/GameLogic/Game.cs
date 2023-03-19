using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum InGameResource
{

}

[RequireComponent(typeof(InputManager))]
public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    //public static int UNIT_MASK = 1 << 3;
    //public static int PLAYER_UNIT_MASK = 1 << 6;
    //public static int ENEMY_UNIT_MASK = 1 << 7;

    [HideInInspector]
    public bool gameIsPaused;

    [SerializeField]
    private CharacterManager _playerCharacter;
    public CharacterManager PlayerCharacter
    {
        get => _playerCharacter;
    }

    public Transform CHARACTER_CONTAINER;
    public List<Character> CHARACTERS;

    /*
    public static Dictionary<InGameResource, GameResource> GAME_RESOURCES =
        new Dictionary<InGameResource, GameResource>()
        {
            {InGameResource.Wood, new GameResource("Wood", 0)},
            {InGameResource.Stone, new GameResource("Stone", 0)},
            {InGameResource.Gold, new GameResource("Gold", 0)},
            {InGameResource.Mana, new GameResource("Mana", 0)}
        };
    */

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance!");
            return;
        }

        Instance = this;

        gameIsPaused = false;

        //DataHandler.LoadGameData();

        //gameIsPaused = true;

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
