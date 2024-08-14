using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class TPAbility : MonoBehaviour
{
    private new Camera camera;
    public GameObject teleportSeed;
    private GameObject currentTeleport;
    public Transform firePoint;
    public float throwForce;
    public bool isTeleporterOut = false;
    public new ParticleSystem particleSystem;
    private Coroutine particlePlay;

    private Animator _animator;
    private int _throwID;
    private int _animIDisThrowing;
    private int animationState;
    private float _animatorSwapCurrentCD;
    private readonly float _animatorSwapCooldown = 0.7f;

    [SerializeField] public float _teleportCooldown = 5f;
    private float _teleportCurrentCD;

    [SerializeField] private AbilityStatus _abilityStatus;

    [SerializeField] private LayerMask aimColliderLayerMask;

    [Header("UI Regular")]
    [SerializeField] private Sprite spriteRegular;
    [SerializeField] private Color spriteColorRegular;

    [Header("UI Active")]
    [SerializeField] private Sprite spriteActive;
    [SerializeField] private Color spriteColorActive;

    public AudioSource audioSource;
    public AudioClip tpThrowSound;
    public AudioClip tpSound;

    // Start is called before the first frame update
    void Start()
    {
        _teleportCurrentCD = 0;
        TryGetComponent(out _animator);
        _throwID = _animator.GetLayerIndex("Left Arm Ability");
        _animIDisThrowing = Animator.StringToHash("isThrowing");
        _abilityStatus.fill.sprite = spriteRegular;
        _abilityStatus.fill.color = spriteColorRegular;
        _abilityStatus.background.color = spriteColorRegular;

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _animatorSwapCurrentCD -= Time.deltaTime;
        _teleportCurrentCD -= Time.deltaTime;
        _teleportCurrentCD = Mathf.Clamp(_teleportCurrentCD, 0, _teleportCooldown);

        if (!isTeleporterOut)
        {
            _abilityStatus.fill.fillAmount = 1 - (_teleportCurrentCD / _teleportCooldown);
        }

        if (_teleportCurrentCD <= 0)
        {
            _abilityStatus.keyIndicator.SetActive(true);
        }

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        if (Input.GetKeyDown(KeyCode.E) && _teleportCurrentCD <= 0)
        {
            if (!isTeleporterOut)
            {
                // Make a new teleport seed in the scene.
                Vector3 aimDir = (mouseWorldPosition - firePoint.position).normalized;
                currentTeleport = Instantiate(teleportSeed, firePoint.position, Quaternion.LookRotation(aimDir, Vector3.up));

                // Actually give momentum/force to the seed for it to fly.
                currentTeleport.GetComponent<Rigidbody>().AddForce(currentTeleport.transform.forward * throwForce, ForceMode.Impulse);
                isTeleporterOut = true;
                _abilityStatus.fill.sprite = spriteActive;
                _abilityStatus.fill.color = spriteColorActive;
                _abilityStatus.background.color = spriteColorActive;

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);

                Vector3 camDir = Camera.main.transform.forward;
                camDir = Vector3.ProjectOnPlane(camDir, Vector3.up);
                transform.forward = camDir;
                _animator.SetBool(_animIDisThrowing, true);
                animationState = 1;

                audioSource.PlayOneShot(tpThrowSound);
                StartCooldown();
            }
            else
            {

                float teleportOffset = GetComponent<CharacterController>().height / 2;
                Vector3 teleportPosition = currentTeleport.transform.position;
                Vector3 teleportLocation = new Vector3(teleportPosition.x, teleportPosition.y + teleportOffset, teleportPosition.z);

                transform.position = teleportLocation;

                // Particle effect
                if (particleSystem != null)
                {
                    if (particlePlay != null)
                    {
                        StopCoroutine(particlePlay);
                    }
                    particlePlay = StartCoroutine(PlayParticleEffect());
                }

                Destroy(currentTeleport);
                currentTeleport = null;
                isTeleporterOut = false;
                _abilityStatus.fill.sprite = spriteRegular;
                _abilityStatus.fill.color = spriteColorRegular;
                _abilityStatus.background.color = spriteColorRegular;

                _animator.SetBool(_animIDisThrowing, false);
                animationState = 0;
                _teleportCurrentCD = _teleportCooldown;
                _abilityStatus.keyIndicator.SetActive(false);
                audioSource.PlayOneShot(tpSound);
            }
        }
        Animate(animationState);
        if (!OnCooldown())
        {
            _animator.SetLayerWeight(_throwID, 0);
        }
    }

    private void Animate(int tpWeight)
    {
        float transitionThrowVelocity = (tpWeight == 1 ? 6f : -6f);
        var currentThrowWeight = _animator.GetLayerWeight(_throwID);
        currentThrowWeight = Mathf.SmoothDamp(currentThrowWeight, tpWeight, ref transitionThrowVelocity, 0.3f);
        _animator.SetLayerWeight(_throwID, currentThrowWeight);
    }

    bool OnCooldown()   // Timer function for animator cooldowns.
    {
        return _animatorSwapCurrentCD > 0;
    }

    private void StartCooldown()    // Function to start timer countdown for animator.
    {
        _animatorSwapCurrentCD = _animatorSwapCooldown;
    }

    public void DestroyTP()
    {
        if (!isTeleporterOut)
            return;
        Destroy(currentTeleport);
        currentTeleport = null;
        isTeleporterOut = false;
        _abilityStatus.fill.color = spriteColorRegular;
        _abilityStatus.fill.sprite = spriteRegular;
        _abilityStatus.background.color = spriteColorRegular;

        _animator.SetBool(_animIDisThrowing, false);
        animationState = 0;
        _teleportCurrentCD = _teleportCooldown;
        _abilityStatus.keyIndicator.SetActive(false);
    }

    private IEnumerator PlayParticleEffect()
    {
        particleSystem.Play();
        yield return new WaitForSeconds(0.25f);
        particleSystem.Stop();
        particlePlay = null;
    }
}
