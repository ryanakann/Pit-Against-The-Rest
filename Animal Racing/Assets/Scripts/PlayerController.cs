using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter), typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    private PlayerStats _stats;
    
    private Camera _camera;
    private NavMeshAgent _agent;

    private ThirdPersonCharacter _character;
    
    void Start()
    {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _character = GetComponent<ThirdPersonCharacter>();

        _agent.updateRotation = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                _agent.SetDestination(hit.point);
            }
        }

        _character.Move(_agent.remainingDistance > _agent.stoppingDistance ? _agent.desiredVelocity : Vector3.zero,
            false, false);
    }
}
