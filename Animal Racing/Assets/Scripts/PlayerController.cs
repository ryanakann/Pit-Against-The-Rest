using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(
    typeof(ThirdPersonCharacter), 
    typeof(NavMeshAgent), 
    typeof(PlayerStats)
    )]
public class PlayerController : MonoBehaviour
{
    public bool isEnabled;
    public bool isRagdolling;

    [SerializeField] private PlayerStats stats;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private ThirdPersonCharacter character;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private Ragdoll ragdoll;

    void Start()
    {
        agent.updateRotation = false;
        SetActive(isEnabled);
    }

    void Update()
    {
        if (!isEnabled) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetRagdolling(!isRagdolling);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if (!agent.enabled || !animator.enabled) return;
        character.Move(agent.remainingDistance > agent.stoppingDistance ? agent.desiredVelocity : Vector3.zero,
            false, false);
    }

    public void UpdatePropertiesFromStats()
    {
        gameObject.name = stats.Name;
        meshRenderer.material.color = stats.Color;
        character.m_MoveSpeedMultiplier = Mathf.Lerp(1f, 2f, stats.Speed / 100f);
        agent.acceleration = Mathf.Lerp(6f, 20f, stats.Speed / 100f);
        stats.UpdateStatsUI();
    }

    public void SetActive(bool value)
    {
        SetRagdolling(false);
        isEnabled = value;
        if (isEnabled)
        {
            agent.enabled = true;
            rb.isKinematic = false;
        }
        else
        {
            agent.enabled = false;
            rb.isKinematic = true;
        }
    }

    public void SetRagdolling(bool value)
    {
        isRagdolling = value;
        if (isRagdolling)
        {
            agent.enabled = false;
            animator.enabled = false;
            rb.isKinematic = true;
        }
        else
        {
            agent.enabled = true;
            animator.enabled = true;
            rb.isKinematic = false;
        }
    }
}
