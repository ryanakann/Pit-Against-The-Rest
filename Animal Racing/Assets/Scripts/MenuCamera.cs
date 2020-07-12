using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private CinemachineMixingCamera mixingCamera;

    private void Awake()
    {
        SetCamera(0);
    }

    public void SetCamera(int i)
    {
        for (int j = 0; j < mixingCamera.ChildCameras.Length; j++)
        {
            mixingCamera.SetWeight(j, (i == j ? 1 : 0));
        }
    }
}
