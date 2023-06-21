using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    [HideInInspector] public List<Vector2> validEnemySpawnPoints;
    private Dictionary<CharacterData, int> _spawnPool;
    [SerializeField] private LayerMask invalidSpawnMask; // blocks spawn areas
    
    [SerializeField] private Tilemap floorTileMap;
    private Vector2 tileMapCenter;
    private float _tileMapWidth, _tileMapHeight,
        _spawnAreaWidth, _spawnAreaHeight;
    
    private void Awake()
    {
        _spawnPool = new Dictionary<CharacterData, int>();
    }

    public void Initialize()
    {
        tileMapCenter = new Vector3(
            floorTileMap.origin.x + (floorTileMap.size.x / 2),
            floorTileMap.origin.y + (floorTileMap.size.y / 2)
            );
        
        _tileMapWidth = floorTileMap.size.x;
        _tileMapHeight = floorTileMap.size.y;
        
        Vector2 bottomLeft = new Vector2(floorTileMap.origin.x, floorTileMap.origin.y);
        Vector2 bottomRight = bottomLeft + (Vector2.right * _tileMapWidth);
        Vector2 topLeft = bottomLeft + (Vector2.up * _tileMapHeight);
        Vector2 topRight = bottomRight + (Vector2.up * _tileMapHeight);
        
        Debug.DrawLine(tileMapCenter, bottomRight, Color.green, 20f);
        Debug.DrawLine(tileMapCenter, bottomLeft, Color.green, 20f);
        Debug.DrawLine(tileMapCenter, topLeft, Color.green, 20f);
        Debug.DrawLine(tileMapCenter, topRight, Color.green, 20f);
        _spawnAreaWidth = 30f;
        _spawnAreaHeight = 30f;
        
        validEnemySpawnPoints = GetValidMapSpawnLocations(1f, new Vector2(_spawnAreaWidth, _spawnAreaHeight));

        foreach (var point in validEnemySpawnPoints)
        {
            Debug.DrawLine(point, point + Vector2.right * 0.25f, Color.cyan, 10f);
            Debug.DrawLine(point, point + -Vector2.right * 0.25f, Color.cyan, 10f);
            Debug.DrawLine(point, point + Vector2.up * 0.25f, Color.cyan, 10f);
            Debug.DrawLine(point, point + -Vector2.up * 0.25f, Color.cyan, 10f);
        }
    }

    public void SpawnEnemyCount(int amount, CharacterData characterData)
    {
        int spawned = 0;

        while (spawned < amount)
        {
            Vector2 point = validEnemySpawnPoints[Random.Range(0, validEnemySpawnPoints.Count)];
            Spawn(point, characterData);
            spawned++;
        }
    }

    public List<Character> SpawnWave(float difficulty)
    {
        Vector2 spawnPoint = validEnemySpawnPoints[Random.Range(0, validEnemySpawnPoints.Count)];

        List<Character> spawnedCharacters = new List<Character>();

        //string readout = $"WAVE: {Game.Instance.currentWave} DIFFICULTY: {difficulty}";

        // spawn the amount of each character type as stored in the wave data
        foreach (CharacterData charKey in _spawnPool.Keys)
        {
            int amountToSpawn = Mathf.CeilToInt(_spawnPool[charKey] * difficulty);
            //readout += $"\n{charKey.ToString()}: {amountToSpawn}";
            for (int i = 0; i < amountToSpawn; i++)
            {
                Character c = Spawn(spawnPoint, charKey);
                spawnedCharacters.Add(c);
            }
        }
        
        return spawnedCharacters;
    }

    public Character Spawn(Vector2 location, CharacterData characterData)
    {
        Character character = new Character(characterData);
        character.SetPosition(location);

        return character;
    }

    public void AddToSpawnPool(CharacterData character, int max)
    {
        _spawnPool[character] = max;
    }

    public void RemoveFromSpawnPool(CharacterData character)
    {
        if (_spawnPool.ContainsKey(character)) _spawnPool.Remove(character);
    }

    public List<Vector2> GetValidMapSpawnLocations(float radius, Vector2 sampleRegionSize)
    {
        List<Vector2> spawns = PoissonDiscSampling.GeneratePoints(radius, sampleRegionSize);
        //Debug.Log("spawns generated with radius of " + radius + ": " + spawns.Count);
        List<Vector2> validSpawns = new List<Vector2>();

        foreach (Vector2 point in spawns)
        {
            var centeredPoint = new Vector2(point.x - (_spawnAreaWidth / 2), point.y - (_spawnAreaHeight / 2));
            
            // check if any illegal objects are present in the spawn area
            Collider2D[] overlapingColliders = Physics2D.OverlapCircleAll(centeredPoint, radius, invalidSpawnMask);
            
            if (overlapingColliders.Length > 0) continue;
            validSpawns.Add(centeredPoint);
        }

        return validSpawns;
    }
}
