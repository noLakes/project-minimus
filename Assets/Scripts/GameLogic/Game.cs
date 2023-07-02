using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    
    private bool _gameIsPaused;
    private PlayerCharacterManager _playerCharacter;
    private SpawnManager _spawnManager;

    // data arrays
    public static CharacterData[] CHARACTER_DATA;
    public static WeaponData[] WEAPON_DATA;
    public static ItemData[] ITEM_DATA;

    public Transform CHARACTER_CONTAINER;
    public List<Character> CHARACTERS; // not used yet - may not be needed in this game

    public GameGlobalParameters gameGlobalParameters;

    public LayerMask PlayerMask;
    public LayerMask EnemyMask;
    public LayerMask InteractableMask;
    public LayerMask WallMask;
    public LayerMask GroundMask;
    public LayerMask ProjectilMask;
    public LayerMask TargetPlayerHitScanMask;
    public LayerMask TargetEnemyHitScanMask;
    
    private void Awake()
    {
        // set up singleton 
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
        _playerCharacter = player.Transform.GetComponent<PlayerCharacterManager>();
        _playerCharacter.Character.SetPosition(new Vector3(-3f, 0f, 0f));
        _playerCharacter.SetAsPlayer();

        // test spawn manager points
        _spawnManager = GetComponent<SpawnManager>();
        _spawnManager.Initialize();
        
        // for testing weapon pickups
        if (true)
        {
            Weapon.SpawnInWorld(DataHandler.LoadWeapon("Sword"), new Vector2(1f, -5f));
            Weapon.SpawnInWorld(DataHandler.LoadWeapon("Wand"), new Vector2(3f, -6f));
        }

        // for testing enemy
        if (true)
        {
            _spawnManager.SpawnEnemyCount(5, DataHandler.LoadCharacter("Test Enemy"));
        }
        
        // for testing passive items
        if (false)
        {
            ItemPickup.Create(DataHandler.LoadItem("SpeedTreads"), new Vector2(0f, -6f));
            ItemPickup.Create(DataHandler.LoadItem("BombBag"), new Vector2(0f, -7.5f));
        }

        GetComponent<InputManager>().Initialize(InputState.ControllingPlayer);
    }

    public void Reset()
    {
        SceneManager.LoadScene("Main");
    }
    
    public void Pause()
    {
        _gameIsPaused = true;
        Time.timeScale = 0f;
        EventManager.TriggerEvent("PauseGame");
    }

    public void Resume()
    {
        _gameIsPaused = false;
        Time.timeScale = 1f;
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
    public PlayerCharacterManager PlayerCharacter => _playerCharacter;
    public AICharacterManager TestEnemy;

    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        DataHandler.SaveGameData();
#endif
    }

}
