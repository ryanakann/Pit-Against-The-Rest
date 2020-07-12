using System;
using System.Collections;
using System.Collections.Generic;
using ES3Types;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(
    typeof(ThirdPersonCharacter), 
    typeof(NavMeshAgent), 
    typeof(PlayerStats)
    )]
public class DebugPlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private ThirdPersonCharacter character;
    private Camera _camera;
    void Start()
    {
        agent.updateRotation = false;
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                agent.SetDestination(hit.point);
            }
        }
        character.Move(agent.remainingDistance > agent.stoppingDistance ? agent.desiredVelocity : Vector3.zero,
            false, false);
    }

    public void UpdatePropertiesFromStats()
    {
        gameObject.name = stats.Name;
        meshRenderer.material.color = stats.Color;
        character.m_MoveSpeedMultiplier = Mathf.Lerp(1f, 2f, stats.Speed / 100f);
        agent.acceleration = Mathf.Lerp(6f, 20f, stats.Speed / 100f);
    }
}
