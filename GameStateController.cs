using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class GameStateController : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private AudioSource deathAudio;
    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private AudioSource ambientMusic;
    [SerializeField] private AudioSource bossMusic;
    public AudioSource victoryMusic;

    [SerializeField] private GameObject endGameSequence;

    private PlayerController playerController;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private PlayerInput playerInput;
    private ThirdPersonShooterController thirdPersonShooterController;
    private CharacterController characterController;
    private TPAbility tpability;
    private Animator animator;
    private WeaponIK weaponIK;
    private int runningLayerID;
    private int animIDonDeath;
    private int upperBodyLayerID;
    private int lowerBodyLayerID;
    public bool inCutscene;
    public bool inEndSequence;
    public int gameIsPaused;   // 0 is the state in the beginning of the game to avoid pausing during the main menu. 1 means game is unpaused, 2 means game is paused.


    // Start is called before the first frame update
    void Start()
    {
        mainMenuMusic.Play();
        initialize();
        inCutscene = false;
        inEndSequence = false;
    }

    private void Update()
    {
        CheckForPause();

        if (gameIsPaused == 0 || gameIsPaused == 2)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void initialize()
    {
        characterController = player.GetComponent<CharacterController>();
        playerController = player.GetComponent<PlayerController>();
        thirdPersonController = player.GetComponent<ThirdPersonController>();
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        playerInput = player.GetComponent<PlayerInput>();
        thirdPersonShooterController = player.GetComponent<ThirdPersonShooterController>();
        tpability = player.GetComponent<TPAbility>();
        animator = player.GetComponent<Animator>();
        weaponIK = player.GetComponent<WeaponIK>();
        runningLayerID = animator.GetLayerIndex("Free Run");
        animIDonDeath = Animator.StringToHash("onDeath");
        upperBodyLayerID = animator.GetLayerIndex("Upper Body Shooting");
        lowerBodyLayerID = animator.GetLayerIndex("Lower Body Shooting");
        uiCanvas.SetActive(false);
        gameIsPaused = 0;
    }


    public void QuitGame()  // Function to quit out of the game.
    {
        Application.Quit();
    }

    // Functions that initiate when the play button is pressed.
    public void StartGame()
    {
        StartCoroutine(StartGameProcess());
    }

    IEnumerator StartGameProcess()
    {
        StartCoroutine(StartFade(mainMenuMusic, 3, 0));
        gameIsPaused = 1;
        yield return new WaitForSeconds(1.7f);

        uiCanvas.SetActive(true);
        playerController.enabled = true;
        thirdPersonController.enabled = true;
        starterAssetsInputs.enabled = true;
        playerInput.enabled = true;
        thirdPersonShooterController.enabled = true;
        tpability.enabled = true;
        weaponIK.enabled = true;

        yield return new WaitForSeconds(1f);
        float currentVolume = ambientMusic.volume;
        ambientMusic.volume = 0;
        ambientMusic.Play();
        StartCoroutine(StartFade(ambientMusic, 5, currentVolume));
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        if (audioSource.volume == 0)
        {
            audioSource.Stop();
        }
        yield break;
    }

    // Functions related to player death.

    public void OnDeath()
    {
        gameIsPaused = 0;

        deathCanvas.SetActive(true);
        uiCanvas.SetActive(false);
        playerController.enabled = false;
        thirdPersonController.isDead = true;
        starterAssetsInputs.enabled = false;
        playerInput.enabled = false;
        thirdPersonShooterController.enabled = false;
        tpability.enabled = false;
        weaponIK.enabled = false;

        animator.SetLayerWeight(upperBodyLayerID, 0);
        animator.SetLayerWeight(lowerBodyLayerID, 0);
        animator.SetLayerWeight(runningLayerID, 1);
        animator.SetTrigger(animIDonDeath);
        AudioSource[] audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Pause();
        }
        deathAudio.Play();
        StartCoroutine(EndGamePause());
    }

    IEnumerator EndGamePause()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(3f);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    void CheckForPause()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (gameIsPaused == 1)
            {
                Pause();
            }
            else if (gameIsPaused == 2)
            {
                Resume();
            }
        }
    }

    void Pause() 
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = 2;
        playerInput.enabled = false;
        playerController.enabled = false;
        thirdPersonShooterController.enabled = false;
        weaponIK.enabled = false;
        tpability.enabled = false;
    }

    public void Resume()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        if (!inEndSequence)
            gameIsPaused = 1;
        if (!inCutscene)
            EnableControls();
    }

    public void DisableControls()
    {
        playerInput.enabled = false;
        playerController.enabled = false;
        thirdPersonShooterController.enabled = false;
        weaponIK.enabled = false;
        tpability.enabled = false;
    }

    public void EnableControls()
    {
        playerInput.enabled = true;
        playerController.enabled = true;
        thirdPersonShooterController.enabled = true;
        weaponIK.enabled = true;
        tpability.enabled = true;
    }

    public void VictoryScreenSequence()
    {
        StartCoroutine(StartFade(bossMusic, 10, 0));
        inCutscene = true;
        inEndSequence = true;
        endGameSequence.GetComponent<EndGameSequence>().StartVictorySequence();
    }

}
