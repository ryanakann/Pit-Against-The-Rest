using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(
    typeof(ThirdPersonCharacter), 
    typeof(NavMeshAgent), 
    typeof(PlayerStats)
    )]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    private Camera _camera;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private ThirdPersonCharacter character;

    void Start()
    {
        _camera = Camera.main;
        agent.updateRotation = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
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
    }
}
