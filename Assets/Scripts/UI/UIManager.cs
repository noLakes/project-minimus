using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform playerInfoPanel;
    [SerializeField] private Text playerHealthText;
    [SerializeField] private Text playerEquippedWeaponText;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerInfoPanel();
    }

    private void UpdatePlayerInfoPanel()
    {
        CharacterManager playerCM = Game.Instance.PlayerCharacter;
        Character player = playerCM.Character;
        
        playerHealthText.text = "HP: " + player.Health + "/" + player.MaxHealth;
        playerEquippedWeaponText.text = playerCM.CurrentWeapon.Data.name;
    }

    private void OnPlayerStatsChange() => UpdatePlayerInfoPanel();

    private void OnEnable()
    {
        EventManager.AddListener("PlayerStatsChange", OnPlayerStatsChange);
        EventManager.AddListener("PlayerWeaponChange", OnPlayerStatsChange);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("PlayerStatsChange", OnPlayerStatsChange);
        EventManager.RemoveListener("PlayerWeaponChange", OnPlayerStatsChange);
    }
}
