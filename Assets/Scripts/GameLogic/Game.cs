using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    
    private bool _gameIsPaused;
    private PlayerCharacterManager _playerCharacter;

    // data arrays
    public static CharacterData[] CHARACTER_DATA;
    public static WeaponData[] WEAPON_DATA;

    public Transform CHARACTER_CONTAINER;
    public List<Character> CHARACTERS;

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
        _playerCharacter.Character.SetPosition(Vector3.zero);
        var startingWeapon = _playerCharacter.AddWeapon(new Weapon(gameGlobalParameters.startingWeapon, player));
        _playerCharacter.EquipWeapon(startingWeapon);
        _playerCharacter.SetAsPlayer();
        
        // spawn test weapon in world
        Weapon.SpawnInWorld(DataHandler.LoadWeapon("Sword"), new Vector2(1f, -5f));
        Weapon.SpawnInWorld(DataHandler.LoadWeapon("Wand"), new Vector2(3f, -6f));

        // for testing enemy
        
        var enemy = new Character(DataHandler.LoadCharacter("Test Enemy"))
        {
            Transform =
            {
                position = new Vector3(0, 5, 0),
                name = "Enemy"
            }
        };
        var enemyCharacter = enemy.Transform.GetComponent<CharacterManager>();
        var enemyWep = enemyCharacter.AddWeapon(new Weapon(gameGlobalParameters.startingWeapon, enemy));
        enemyCharacter.EquipWeapon(enemyWep);

        TestEnemy = enemy.Transform.GetComponent<AICharacterManager>();
        
        /*
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
        */
        
        
        GetComponent<InputManager>().Initialize(InputState.ControllingPlayer);
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
    public PlayerCharacterManager PlayerCharacter => _playerCharacter;
    public AICharacterManager TestEnemy;

    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        DataHandler.SaveGameData();
#endif
    }

}
