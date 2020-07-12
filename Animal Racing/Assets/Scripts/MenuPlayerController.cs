using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using Random = UnityEngine.Random;

[RequireComponent(
    typeof(ThirdPersonCharacter), 
    typeof(NavMeshAgent), 
    typeof(PlayerStats)
    )]
public class MenuPlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider navMeshBounds;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private bool wander;
    
    [SerializeField] private PlayerStats stats;
    private Camera _camera;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private ThirdPersonCharacter character;

    private Vector3 _destination;
    
    void Start()
    {
        _camera = Camera.main;
        agent.updateRotation = false;
        transform.eulerAngles = Vector3.up * Random.Range(0f, 360f);
        UpdatePropertiesFromStats();
        if (wander)
        {
            StartCoroutine(Wander());
        }
    }

    IEnumerator Wander()
    {
        while (wander)
        {
            var bounds = navMeshBounds.transform;
            var size = navMeshBounds.size / 2f;
            var origin = bounds.position + navMeshBounds.center +
                         bounds.right * Random.Range(-size.z, size.z) +
                         bounds.forward * Random.Range(-size.x, size.x);
            var ray = new Ray(origin, Vector3.down);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
            {
                _destination = hit.point;
                agent.SetDestination(hit.point);
            }
            yield return new WaitForSeconds(Random.Range(5f, 20f));
        }
    }

    void Update()
    {
        Debug.DrawLine(transform.position, _destination, Color.cyan);
        character.Move(agent.remainingDistance > agent.stoppingDistance ? agent.desiredVelocity : Vector3.zero,
            false, false);
    }

    public void UpdatePropertiesFromStats()
    {
        gameObject.name = stats.Name;
        meshRenderer.material.color = stats.Color;
    }
}
