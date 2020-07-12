using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Broodmother : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private void Update()
    {
        
    }

    public GameObject GenerateStartingPlayer()
    {
        GameObject player = GenerateRandomPlayer();
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.Acceleration /= 10f;
        playerStats.Constitution /= 10f;
        playerStats.Poise /= 10f;
        playerStats.Speed /= 10f;
        playerStats.Stamina /= 10f;
        playerStats.Style /= 10f;
        player.GetComponent<PlayerController>().UpdatePropertiesFromStats();
        return player;
    }

    public GameObject GenerateRandomPlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.RandomizePlayerStats();
        player.GetComponent<PlayerController>().UpdatePropertiesFromStats();
        return player;
    }

    public void Breed(PlayerStats playerStats1, PlayerStats playerStats2)
    {
        GameObject player = Instantiate(playerPrefab, Vector3.right * 5f, Quaternion.identity);
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.RandomizePlayerStats();
        playerStats.Stamina = (playerStats1.Stamina + playerStats2.Stamina + playerStats.Stamina) / 3f;
        playerStats.Style = (playerStats1.Style + playerStats2.Style + playerStats.Style) / 3f;
        playerStats.Constitution = (playerStats1.Constitution + playerStats2.Constitution + playerStats.Constitution) / 3f;
        playerStats.Speed = (playerStats1.Speed + playerStats2.Speed + playerStats.Speed) / 3f;
        playerStats.Acceleration = (playerStats1.Acceleration + playerStats2.Acceleration + playerStats.Acceleration) / 3f;
        playerStats.Poise = (playerStats1.Poise + playerStats2.Poise + playerStats.Poise) / 3f;
        playerStats.Color = ((Vector4)playerStats1.Color + (Vector4)playerStats2.Color + (Vector4)playerStats.Color) / 3f;
        player.GetComponent<PlayerController>().UpdatePropertiesFromStats();
    }
}
