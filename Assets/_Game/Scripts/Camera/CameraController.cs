﻿using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerShip;
    [SerializeField] private GameObject targetReticle;
    [SerializeField] private GameObject arena;
    private ICameraBehaviour _camBehaviour;

    [SerializeField] private CamAttributes overShoulderAttribs;
    [SerializeField] private CamAttributes topDownAttribs;
    
    private Arena _arena;
    private Vector3 _velocity;

    private void Awake()
    {
        _arena = arena.GetComponent<Arena>();
        Debug.Assert(_arena, $"{gameObject.name} can't find the Arena! Did you forget to set a reference to it?");
        _arena.OnModeChange += ArenaOnOnModeChange;
        
        Debug.Assert(playerShip, $"{gameObject.name} has no reference to the player ship!");
        Debug.Assert(targetReticle, $"{gameObject.name} has no reference to the target reticle!");

        Debug.Assert(overShoulderAttribs, $"{gameObject.name} doesn't have any assigned Over Shoulder camera attribs!");
        overShoulderAttribs.followTargets = new List<GameObject> { playerShip };
        overShoulderAttribs.lookTargets = new List<GameObject> { targetReticle };

        Debug.Assert(topDownAttribs, $"{gameObject.name} doesn't have any assigned Top Down camera attribs!!");
        topDownAttribs.followTargets = new List<GameObject> { playerShip };
        topDownAttribs.lookTargets = new List<GameObject> { targetReticle };

        _camBehaviour = new OverShoulderCam(ref topDownAttribs);
    }

    private void ArenaOnOnModeChange(Arena.Mode mode)
    {
        if (mode == Arena.Mode.Horizontal)
            _camBehaviour.Attribs = topDownAttribs;
        else
            _camBehaviour.Attribs = overShoulderAttribs;
    }


    private void LateUpdate()
    {
        transform.localPosition = _camBehaviour.GetPosition(transform.localPosition);
        transform.rotation = _camBehaviour.GetRotation(transform);
        Camera.main.fieldOfView = _camBehaviour.GetFoV(Camera.main.fieldOfView);
    }
}
