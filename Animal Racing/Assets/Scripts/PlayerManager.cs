using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float spacing = 3f;
    [SerializeField] private Tween tween;
    [SerializeField] private Broodmother broodmother;

    private HashSet<GameObject> _players;
    private int _activePlayerIndex;
    private int _activePlayerCount;

    private GameObject _parentSlot1;
    private GameObject _parentSlot2;

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

    private bool SetParentSlot1(GameObject player)
    {
        if (_parentSlot1 != null) return false;
        _parentSlot1 = player;
        return true;
    }

    private bool SetParentSlot2(GameObject player)
    {
        if (_parentSlot2 != null) return false;
        _parentSlot2 = player;
        return true;
    }

    public bool AddParentToSlots(GameObject player)
    {
        if (_parentSlot1 == null)
        {
            SetParentSlot1(player);
            return true;
        }
        if (_parentSlot2 == null)
        {
            SetParentSlot2(player);
            return true;
        }
        return false;
    }

    public void ClearParentSlot1()
    {
        _parentSlot1 = null;
    }

    public void ClearParentSlot2()
    {
        _parentSlot2 = null;
    }
}
