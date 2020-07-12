using UnityEngine;

public class Broodmother : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    
    
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

    public GameObject Breed(PlayerStats playerStats1, PlayerStats playerStats2)
    {
        GameObject player = Instantiate(playerPrefab, Vector3.right * 5f, Quaternion.identity);
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.RandomizePlayerStats();
        playerStats.Stamina = 0.4f * playerStats1.Stamina + 0.4f * playerStats2.Stamina + 0.2f * playerStats.Stamina;
        playerStats.Style = 0.4f * playerStats1.Style + 0.4f * playerStats2.Style + 0.2f * playerStats.Style;
        playerStats.Constitution = 0.4f * playerStats1.Constitution + 0.4f * playerStats2.Constitution + 0.2f * playerStats.Constitution;
        playerStats.Speed = 0.4f * playerStats1.Speed + 0.4f * playerStats2.Speed + 0.2f * playerStats.Speed;
        playerStats.Acceleration = 0.4f * playerStats1.Acceleration + 0.4f * playerStats2.Acceleration + 0.2f * playerStats.Acceleration;
        playerStats.Poise = 0.4f * playerStats1.Poise + 0.4f * playerStats2.Poise + 0.2f * playerStats.Poise;

        Color color;
        float val = Random.value;
        if (val < 1 / 3f)
        {
            color = playerStats1.Color;
        }
        else if (val < 2 / 3f)
        {
            color = playerStats2.Color;
        }
        else
        {
            color = playerStats.Color;
        }
        playerStats.Color = color;
        
        player.GetComponent<PlayerController>().UpdatePropertiesFromStats();
        return player;
    }
}
