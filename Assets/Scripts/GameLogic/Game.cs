using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    
    private bool _gameIsPaused;
    private CharacterManager _playerCharacter;

    // data arrays
    public static CharacterData[] CHARACTER_DATA;
    public static WeaponData[] WEAPON_DATA;

    public Transform CHARACTER_CONTAINER;
    public List<Character> CHARACTERS;

    public GameGlobalParameters gameGlobalParameters;

    public static int PLAYER_MASK = 1 << 6;
    public static int ENEMY_MASK = 1 << 7;
    public static int PROJECTILE_MASK = 1 << 8;
    public static int WALL_MASK = 1 << 12;
    public static int GROUND_MASK = 1 << 13;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Instance!");
            return;
        }

        Instance = this;
        DataHandler.LoadGameData();
        _gameIsPaused = false;
    }

    private void Start()
    {
        // spawn test player
        var player = new Character(gameGlobalParameters.startingCharacter);
        _playerCharacter = player.Transform.GetComponent<CharacterManager>();
        _playerCharacter.Character.SetPosition(Vector3.zero);
        var startingWeapon = _playerCharacter.AddWeapon(new Weapon(gameGlobalParameters.startingWeapon, player));
        _playerCharacter.EquipWeapon(startingWeapon);

        for (var i = 0; i < 5; i++)
        {
            var enemy = new Character(DataHandler.LoadCharacter("Test Enemy"))
            {
                Transform =
                {
                    position = Vector3.zero + (Vector3.up * i) * 2,
                    name = "Enemy" + i
                }
            };
        }
    }

    private void Update()
    {
        if (_gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
            }
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
        _gameIsPaused = true;
        EventManager.TriggerEvent("PauseGame");
    }

    public void Resume()
    {
        _gameIsPaused = false;
        EventManager.TriggerEvent("ResumeGame");
    }

    public void Quit() => Application.Quit();

    private void OnEnable()
    {
        // to add listeners
    }

    private void OnDisable()
    {
        // to remove listeners
    }

    public bool GameIsPaused => _gameIsPaused;
    public CharacterManager PlayerCharacter => _playerCharacter;

    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        DataHandler.SaveGameData();
#endif
    }

}
