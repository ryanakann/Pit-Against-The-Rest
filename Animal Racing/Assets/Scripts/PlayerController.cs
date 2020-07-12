using System.Collections;
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
    public bool isEnabled;
    public bool isRagdolling;

    [SerializeField] private PlayerStats stats;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private ThirdPersonCharacter character;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private Ragdoll ragdoll;
    public bool finished;

    void Start()
    {
        agent.updateRotation = false;
        SetActive(isEnabled);
    }

    void Update()
    {
        if (!isEnabled) return;
        finished = agent.remainingDistance < agent.stoppingDistance;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetRagdolling(!isRagdolling);
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

    public void RandomRagdoll()
    {
        StartCoroutine(RandomRagdollCR());
    }

    private IEnumerator RandomRagdollCR()
    {
        while (!finished)
        {
            yield return new WaitForSeconds(Random.Range(3f, 10f));
            if (!finished)
            {
                if (Random.value > stats.Constitution / 100f)
                {
                    SetRagdolling(true);
                    yield return new WaitForSeconds(2f);
                    SetRagdolling(false);
                }
            }
        }
    }
    

    public void RandomDestination(Transform side1, Transform side2)
    {
        finished = false;
        StartCoroutine(RandomDestinationCR(side1, side2));
        RandomRagdoll();
    }

    private IEnumerator RandomDestinationCR(Transform side1, Transform side2)
    {
        Vector3 target = transform.position;
        while (transform.position.x < 165f)
        {
            Vector3 point = Vector3.Lerp(side1.position, side2.position, Random.Range(0.1f, 0.9f)) + Vector3.right * 5f;
            Ray ray = new Ray(point + Vector3.up * 10f, Vector3.down);
            if (Physics.Raycast(ray, out var hit))
            {
                target = hit.point;
                agent.SetDestination(target);
            }
            Debug.DrawLine(transform.position, target, Color.cyan);
            yield return new WaitForSeconds(Mathf.Lerp(1f, 5f, Random.value * (1+stats.Constitution) / 101f));
        }

        finished = true;
    }
}
