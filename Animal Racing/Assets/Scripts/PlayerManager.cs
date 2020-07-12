using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float spacing = 3f;
    [SerializeField] private Broodmother broodmother;
    [SerializeField] private RaceManager raceManager;
    
    private HashSet<GameObject> _players;
    private int _activePlayerIndex;
    private int _activePlayerCount;

    [SerializeField] private TMP_Text parentSlot1Text;
    [SerializeField] private TMP_Text parentSlot2Text;
    [SerializeField] private GameObject _parentSlot1;
    [SerializeField] private GameObject _parentSlot2;

    private Vector3 _initialPosition;
    private bool _transitioning;
    private Vector3 _targetPos;

    private void Awake()
    {
        _initialPosition = transform.position;
        _activePlayerIndex = 0;
        _players = new HashSet<GameObject>();

        _transitioning = false;
        
        RegisterPlayer(broodmother.GenerateStartingPlayer());
        RegisterPlayer(broodmother.GenerateStartingPlayer());
        Organize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            RegisterPlayer(broodmother.GenerateRandomPlayer());
        }
    }

    public void RegisterPlayer(GameObject player)
    {
        _players.Add(player);
        player.transform.SetParent(transform);
        _activePlayerCount++;
        player.transform.SetSiblingIndex(_activePlayerIndex);
        Organize();
    }

    public void UnregisterPlayer(GameObject player, bool deletePlayer = true)
    {
        if (!_players.Contains(player)) return;
        _players.Remove(player);
        if (deletePlayer) Destroy(player);
        player.transform.SetParent(null);
        _activePlayerCount--;
        if (_activePlayerIndex >= _activePlayerCount - 1)
        {
            _activePlayerIndex--;
        }
        Organize();
    }

    public GameObject GetActivePlayer() => transform.GetChild(_activePlayerIndex).gameObject;

    public void IncrementActivePlayer()
    {
        _activePlayerIndex = (_activePlayerIndex + 1) % _activePlayerCount;
        MoveToActivePlayer();
    }

    public void DecrementActivePlayer()
    {
        _activePlayerIndex = (_activePlayerIndex - 1) % _activePlayerCount;
        if (_activePlayerIndex < 0f) _activePlayerIndex = _activePlayerCount - 1;
        MoveToActivePlayer();
    }

    private void MoveToActivePlayer()
    {
        _targetPos = _initialPosition + transform.right * (spacing * _activePlayerIndex);
        if (!_transitioning)
        {
            StartCoroutine(MoveToActivePlayerCR());
        }
    }

    private IEnumerator MoveToActivePlayerCR()
    {
        _transitioning = true;
        _targetPos = _initialPosition + transform.right * (spacing * _activePlayerIndex);
        Vector3 velRef = Vector3.zero;
        
        while (Vector3.SqrMagnitude(_targetPos - transform.position) > 0.01f * 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref velRef, 0.2f);
            yield return new WaitForEndOfFrame();
        }
        transform.position = _targetPos;
        _transitioning = false;
    }
    
    private void Organize()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            child.localRotation = Quaternion.identity;
            child.localPosition = Vector3.left * (spacing * i) ;
            i++;
        }
        MoveToActivePlayer();
    }

    public void SetParentSlot1()
    {
        if (_parentSlot1 == GetActivePlayer() || _parentSlot2 == GetActivePlayer()) return;
        _parentSlot1 = GetActivePlayer();
        parentSlot1Text.text = _parentSlot1.GetComponent<PlayerStats>().Name;
        parentSlot1Text.color = _parentSlot1.GetComponent<PlayerStats>().Color;
    }

    public void SetParentSlot2()
    {
        if (_parentSlot1 == GetActivePlayer() || _parentSlot2 == GetActivePlayer()) return;
        _parentSlot2 = GetActivePlayer();
        parentSlot2Text.text = _parentSlot2.GetComponent<PlayerStats>().Name;
        parentSlot2Text.color = _parentSlot2.GetComponent<PlayerStats>().Color;
    }

    public void Breed()
    {
        if (_parentSlot1 == null || _parentSlot2 == null) return;
        GameObject child = broodmother.Breed(_parentSlot1.GetComponent<PlayerStats>(), _parentSlot2.GetComponent<PlayerStats>());
        RegisterPlayer(child);
    }

    public void ClearParentSlot1()
    {
        _parentSlot1 = null;
    }

    public void ClearParentSlot2()
    {
        _parentSlot2 = null;
    }

    public void WinTheRace()
    {
        raceManager.Race(GetActivePlayer());
    }
}
