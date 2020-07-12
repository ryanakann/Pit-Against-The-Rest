using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private CharacterJoint[] _joints;
    public bool isEnabled;
    
    void Awake()
    {
        _joints = gameObject.GetComponentsInChildren<CharacterJoint>();
        DisableRagdoll();
    }
    
    void Update()
    {
        
    }

    public void EnableRagdoll()
    {
        isEnabled = true;
        foreach (var joint in _joints)
        {
            var component = joint.GetComponent<Rigidbody>();
            if (!component)
            {
                joint.gameObject.AddComponent<Rigidbody>();
            }
        }
    }
    
    public void DisableRagdoll()
    {
        return;
        isEnabled = false;
        foreach (var joint in _joints)
        {
            var component = joint.GetComponent<Rigidbody>();
            if (component)
            {
                Destroy(component);
            }
        }
    }

    public void ToggleRagdoll()
    {
        if (isEnabled) EnableRagdoll();
        else DisableRagdoll();
        isEnabled = !isEnabled;
    }
}
