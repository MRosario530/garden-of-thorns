using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class TestTeleportAbility : MonoBehaviour
{

    private new Camera camera;
    public GameObject teleportSeed;
    public GameObject currentTeleport;
    public Transform firePoint;
    public float throwForce;
    private bool isTeleporterOut = false;

    private Animator _animator;
    private bool _hasAnimator;
    private int _throwID;
    private int _animIDisThrowing;
    private int animationState;
    private float _animatorSwapCurrentCD;
    private float _animatorSwapCooldown = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
        _throwID = _animator.GetLayerIndex("Left Arm Ability");
        _animIDisThrowing = Animator.StringToHash("isThrowing");

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _animatorSwapCurrentCD -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isTeleporterOut)
            {
                // Make a new teleport seed in the scene.
                currentTeleport = Instantiate(teleportSeed, firePoint.position, Quaternion.identity) as GameObject;
                // Actually give momentum/force to the seed for it to fly.
                currentTeleport.GetComponent<Rigidbody>().AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
                isTeleporterOut = true;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
                Vector3 camDir = Camera.main.transform.forward;
                camDir = Vector3.ProjectOnPlane(camDir, Vector3.up);
                transform.forward = camDir;
                _animator.SetBool(_animIDisThrowing, true);
                animationState = 1;
                startCooldown();
            }
            else
            {
                float teleportOffset = GetComponent<CharacterController>().height / 2;
                Vector3 teleportPosition = currentTeleport.transform.position;
                Vector3 teleportLocation = new Vector3(teleportPosition.x, teleportPosition.y + teleportOffset, teleportPosition.z);
                transform.position = teleportLocation;
                Destroy(currentTeleport);
                currentTeleport = null;
                isTeleporterOut = false;
                _animator.SetBool(_animIDisThrowing, false);
                animationState = 0;
            }
        }
        animate(animationState);
        if (!onCooldown())
        {
            Debug.Log(_animatorSwapCurrentCD);
            _animator.SetLayerWeight(_throwID, 0);
        }
    }

    private void animate(int tpWeight)
    {
        float transitionThrowVelocity = (tpWeight == 1 ? 6f : -6f);
        var currentThrowWeight = _animator.GetLayerWeight(_throwID);
        currentThrowWeight = Mathf.SmoothDamp(currentThrowWeight, tpWeight, ref transitionThrowVelocity, 0.3f);
        _animator.SetLayerWeight(_throwID, currentThrowWeight);
    }

    bool onCooldown()   // Timer function for animator cooldowns.
    {
        return _animatorSwapCurrentCD > 0;
    }

    private void startCooldown()    // Function to start timer countdown for animator.
    {
        _animatorSwapCurrentCD = _animatorSwapCooldown;
    }
}
