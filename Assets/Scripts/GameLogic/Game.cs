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
            Debug.LogError("There is more than one instance!");
            return;
        }

        Instance = this;

        DataHandler.LoadGameData();

        gameIsPaused = false;

    }

    private void Start()
    {
        // spawn test player
        Character player = new Character(DataHandler.LoadCharacter("Test Player"));
        _playerCharacter = player.transform.GetComponent<CharacterManager>();
        _playerCharacter.Character.SetPosition(Vector3.zero);
        Weapon startingWeapon = _playerCharacter.AddWeapon(new Weapon(DataHandler.LoadWeapon("Crossbow"), player));
        _playerCharacter.EquipWeapon(startingWeapon);

        for (int i = 0; i < 5; i++)
        {
            Character enemy = new Character(DataHandler.LoadCharacter("Test Enemy"));
            CharacterManager enemyCharacter = enemy.transform.GetComponent<CharacterManager>();
            enemyCharacter.Character.SetPosition(Vector3.zero + (Vector3.up * i) * 2);
            enemyCharacter.transform.name = "Enemy" + i;
        }
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


    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        DataHandler.SaveGameData();
#endif
    }

}
