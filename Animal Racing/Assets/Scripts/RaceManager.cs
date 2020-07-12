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
        racers.Add(racer.GetComponent<PlayerController>());
        // racer.transform.position = Vec
    }

    public void Race(GameObject player)
    {
        StartCoroutine(RaceCR(player));
    }

    private IEnumerator RaceCR(GameObject player)
    {
        GameObject playerRacer = Instantiate(player.gameObject);
        player.transform.Find("StatsUI").gameObject.SetActive(false);
        player.transform.Find("PointerUI").gameObject.SetActive(true);
        RegisterRacer(player);
        for (int i = 1; i < 8; i++)
        {
            RegisterRacer(broodmother.GenerateRandomPlayer());
        }
        OnRaceReady?.Invoke();
        yield return new WaitForSeconds(3f);
        StartRace();
        while (!player.GetComponent<PlayerController>().finished)
        {
            
        }
    }

    private void StartRace()
    {
        OnRaceStart?.Invoke();
        Transform side1 = finish.GetChild(0);
        Transform side2 = finish.GetChild(1);
        
        foreach (PlayerController racer in racers)
        {
            racer.RandomDestination(side1, side2);
        }
    }

    private void FinishRace()
    {
        OnRaceFinish?.Invoke();
        foreach (PlayerController racer in racers)
        {
            racer.finished = true;
            Destroy(racer.gameObject, 5f);
        }
    }
}
