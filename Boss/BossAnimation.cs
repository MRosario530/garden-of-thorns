using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;

public class BossAnimation : MonoBehaviour
{
    public GameStateController gameStateManager;
    [SerializeField] private GameObject vfx;
    private Animator animator;

    public AudioSource audioSRC;
    public AudioClip roarClip;
    public AudioClip stompClip;
    public AudioClip armSwingClip;
    public AudioClip stompAttackClip;

    public Transform jumpPoint;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineVirtualCamera virtualCamera2;
    [SerializeField] private float shakeTimer;
    [SerializeField] private float shakeTimerTotal;
    [SerializeField] private float startingIntensity;
    public GameObject UICanvas;
    private GameObject groundShatter;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSRC = GetComponent<AudioSource>();
        vfx = Resources.Load<GameObject>("Prefabs/VFX/GroundSlam");
    }

    public void AnimStart()
    {
        StartCoroutine("IShiftCamera");
        animator.enabled = true;
        UICanvas.SetActive(false);
        gameStateManager.DisableControls();
        gameStateManager.inCutscene = true;
    }

    IEnumerator IShiftCamera()
    {
        virtualCamera2.m_Priority = 60;
        yield return new WaitForSeconds(3);
        virtualCamera.m_Priority = 61;
        virtualCamera2.m_Priority = 1;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isGrounded", IsGrounded());
        if (transform.position.z <= jumpPoint.position.z)
            animator.SetBool("timeToJump", true);

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f,(1 - (shakeTimer / shakeTimerTotal)));
            }
        }

    }

    bool IsGrounded()
    {
        return GetComponent<Rigidbody>().velocity.y == 0;
    }

    void LandingEvent()
    {
        groundShatter = Instantiate(vfx, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        Destroy(groundShatter, 10f);
    }

    IEnumerator SwapAnimator()
    {
        yield return new WaitForSeconds(2);
        animator.SetLayerWeight(1, 1);
        virtualCamera.m_Priority = 1;

        yield return new WaitForSeconds(2);
        UICanvas.SetActive(true);
        gameStateManager.inCutscene = false;
        gameStateManager.EnableControls();
        gameObject.GetComponent<BossController>().enabled = true;
    }

    void RoarEvent()
    {
        audioSRC.volume = 0.7f;
        audioSRC.PlayOneShot(roarClip);
        ShakeCamera(1f, 2f);
        StartCoroutine("SwapAnimator");
    }

    private void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    void FootstepEvent()
    {
        audioSRC.volume = 0.1f;
        audioSRC.PlayOneShot(stompClip);
    }

    void SlamEvent()
    {
        audioSRC.volume = .8f;
        audioSRC.PlayOneShot(stompAttackClip);
    }

    void ArmSwingEvent()
    {
        audioSRC.volume = 1f;
        audioSRC.PlayOneShot(armSwingClip);
    }

    void DisableRootMotion()
    {
        animator.applyRootMotion = false;
    }
}
