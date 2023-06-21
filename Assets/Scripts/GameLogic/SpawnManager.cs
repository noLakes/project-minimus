using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    [HideInInspector]
    public List<Vector2> validEnemySpawnPoints;
    private float _spawnAreaWidth, _spawnAreaHeight;
    private Dictionary<CharacterData, int> _spawnPool;
    [FormerlySerializedAs("validSpawnMask")] [SerializeField] private LayerMask invalidSpawnMask;

    private void Awake()
    {
        _spawnPool = new Dictionary<CharacterData, int>();
    }

    public void Initialize(float width, float height)
    {
        _spawnAreaWidth = width;
        _spawnAreaHeight = height;
        
        validEnemySpawnPoints = GetValidMapSpawnLocations(1f, new Vector2(_spawnAreaWidth, _spawnAreaHeight));
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

        // adjust wave data spawn amounts according to difficulty
        UpdateSpawnPoolData(difficulty);

        List<Character> spawnedCharacters = new List<Character>();

        //string readout = $"WAVE: {Game.Instance.currentWave} DIFFICULTY: {difficulty}";

        // spawn the amount of each character type as stored in the wave data
        foreach (CharacterData charKey in _spawnPool.Keys)
        {
            int amountToSpawn = Mathf.CeilToInt(_spawnPool[charKey] * difficulty);
            //readout += $"\n{charKey.ToString()}: {amountToSpawn}";
            for (int i = 0; i < amountToSpawn; i++)
            {
                Character c = Spawn(spawnPoint, charKey, 1);
                spawnedCharacters.Add(c.transform.GetComponent<UnitManager>());
            }
        }

        if (moveToCenter)
        {
            if (keyStructure == null) keyStructure = Game.Instance.keyStructure;
            if (spawnedCharacters.Count > 10)
            {
                // move units in batches across many frames
                StartCoroutine(MoveToKeyStructureBatchRoutine(spawnedCharacters));
            }
            else
            {
                foreach (UnitManager unit in spawnedCharacters) MoveToKeyStructure(unit);
            }

        }

        //Debug.Log(readout);
        StartCoroutine(EnableMapDotThroughFogRoutine(spawnedCharacters));
        return spawnedCharacters;
    }

    private IEnumerator EnableMapDotThroughFogRoutine(List<UnitManager> units)
    {
        yield return new WaitForSeconds(0.5f);
        foreach (UnitManager unit in units)
        {
            Transform dot = unit.transform.Find("Cylinder");
            if (dot != null && unit.transform.TryGetComponent<FogRendererToggler>(out var fogToggler))
            {
                fogToggler.RemoveRenderReference(dot);
            }
            yield return null;
        }
    }

    private void MoveToKeyStructure(UnitManager unit)
    {
        Vector3 movePos = Utility.RandomPointOnCircleEdge(keyStructure.targetSize + 1.5f, keyStructure.transform.position);
        unit.behaviorTree.SetDataNextFrame("attackMove", movePos);
    }

    private IEnumerator MoveToKeyStructureBatchRoutine(List<UnitManager> units)
    {
        foreach (UnitManager unit in units)
        {
            MoveToKeyStructure(unit);
            yield return null;
        }
    }

    public Character Spawn(Vector2 location, CharacterData characterData)
    {
        Character character = new Character(characterData);
        character.SetPosition(location);

        return character;
    }

    public Character SpawnWithDestination(Vector3 location, Vector3 destination, CharacterData characterData, int playerId)
    {
        Character character = Spawn(location, characterData, playerId);
        character.transform.GetComponent<CharacterManager>().behaviorTree.SetDataNextFrame("attackMove", destination);

        if (character.data.name == "Zombie" && zombieSpeedMod > 0.0f) character.transform.GetComponent<NavMeshAgent>().speed += zombieSpeedMod;

        return character;
    }

    private void UpdateSpawnPoolData(float difficulty)
    {
        switch (Game.Instance.currentWave)
        {
            case 1:
                // optionally introduce new enemy types to spawn pool
                // or adjust max values....
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            default:
                break;
        }
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
        Vector2 mapCenter = Vector2.zero;

        foreach (Vector2 point in spawns)
        {
            // check if any illegal objects are present in the spawn area
            Collider2D[] overlapingColliders = Physics2D.OverlapCircleAll(point, radius, invalidSpawnMask);
            
            if (overlapingColliders.Length > 0) continue;
            validSpawns.Add(point);
        }

        return validSpawns;
    }
}
