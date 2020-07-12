using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceManager : MonoBehaviour
{
    private bool _raceInProgress;
    [SerializeField] private Broodmother broodmother;
    [SerializeField] private Transform start;
    [SerializeField] private Transform finish;
    private List<Vector3> startingPositions;
    
    private List<PlayerController> racers;

    public UnityEvent OnRaceReady;
    public UnityEvent OnRaceStart;
    public UnityEvent OnRaceFinish;
    
    void Start()
    {
        racers = new List<PlayerController>();
        startingPositions = new List<Vector3>();

        for (int i = 0; i < 8; i++)
        {
            Vector3 point = Vector3.Lerp(start.GetChild(0).position, start.GetChild(1).position, (i + 0.5f) / 8f);
            Ray ray = new Ray(point + Vector3.up * 10f, Vector3.down);

            if (Physics.Raycast(ray, out var hit))
            {
                startingPositions.Add(hit.point);
            }
        }
    }

    public void RegisterRacer(GameObject racer)
    {
        racer.transform.position = startingPositions[racers.Count];
        racers.Add(racer.GetComponent<PlayerController>());
    }

    public void Race(GameObject player)
    {
        StartCoroutine(RaceCR(player));
    }

    private IEnumerator RaceCR(GameObject player)
    {
        print("Ready...");
        OnRaceReady?.Invoke();
        GameObject playerRacer = broodmother.GenerateRandomPlayer();
        playerRacer.GetComponent<PlayerStats>().CopyPlayerStats(player.GetComponent<PlayerStats>());
        playerRacer.GetComponent<PlayerController>().UpdatePropertiesFromStats();
        playerRacer.transform.Find("StatsUI").gameObject.SetActive(false);
        playerRacer.transform.Find("PointerUI").gameObject.SetActive(true);
        RegisterRacer(playerRacer);
        for (int i = 1; i < 8; i++)
        {
            GameObject npc = broodmother.GenerateRandomPlayer();
            npc.transform.Find("StatsUI").gameObject.SetActive(false);
            npc.transform.Find("PointerUI").gameObject.SetActive(false);
            npc.GetComponent<PlayerController>().SetActive(false);
            RegisterRacer(npc);
        }

        yield return new WaitForSeconds(3f);
        StartRace();
        while (playerRacer.transform.position.x < 165f)
        {
            yield return new WaitForEndOfFrame();
        }
        FinishRace();
    }

    private void StartRace()
    {
        print("Go!");
        OnRaceStart?.Invoke();
        Transform side1 = finish.GetChild(0);
        Transform side2 = finish.GetChild(1);
        
        foreach (PlayerController racer in racers)
        {
            racer.SetActive(true);
            racer.RandomDestination(side1, side2);
        }
    }

    public void FinishRace()
    {
        print("Finished!");
        OnRaceFinish?.Invoke();
        foreach (PlayerController racer in racers)
        {
            racer.finished = true;
            Destroy(racer.gameObject, 2f * Time.deltaTime);
        }
        racers = new List<PlayerController>();
    }
}
