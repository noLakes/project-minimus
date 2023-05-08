using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private Character _character;
    private CharacterManager _characterManager;
    private NavMeshAgent _navMeshAgent;
    private bool _isIdle;
    
    public void Initialize(CharacterManager characterManager)
    {
        _characterManager = characterManager;
        _character = _characterManager.Character;
    }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        
    }

    public bool TryMove(Vector2 point)
    {
        // try move
        return true;
    }

    public bool ValidPathTo(Vector2 location)
    {
        return true;
    }
    
    public bool ValidPathTo(Transform target) => ValidPathTo(target.position);

    public void StopMoving()
    {
        // implement
    }

    public void SetIdleStatus(bool status)
    {
        _isIdle = status;
    }

    public void OnSleep()
    {
        // respond to BT sleeping
    }

    public Character Character => _character;
    public CharacterManager CharacterManager => _characterManager;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public bool IsIdle => _isIdle;
}
