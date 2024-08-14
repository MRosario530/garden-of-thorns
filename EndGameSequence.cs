using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.UI;

public class EndGameSequence : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireflies;
    [SerializeField] private GameObject dialogBoxPrefab;
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private GameObject fadeInCanvas;
    [SerializeField] private GameObject fadeOutCanvas;

    [SerializeField] private GameObject campfire;
    [SerializeField] private Transform playerEndPoint;
    [SerializeField] private GameObject player;

    [SerializeField] private GameStateController gameStateController;

    public string finalText;
    public List<CinemachineVirtualCamera> cameras;
    public List<float> timesForCameras;
    private GameObject dialogBox;

    [SerializeField] private Material skybox;
    [SerializeField] private Light lightObject;

    public GameObject butterflies;

    public void StartVictorySequence()
    {
        StartCoroutine(StartSequence());
    }

    public IEnumerator StartSequence()
    {
        // Starts off by displaying the dialog box for the player.
        yield return new WaitForSeconds(10f);
        dialogBox = Instantiate(dialogBoxPrefab, new Vector3(Screen.width / 2, Screen.height - 40, 0), Quaternion.identity);
        dialogBox.transform.SetParent(uiCanvas.transform, true);
        dialogBox.GetComponent<DialogBox>().SetText(finalText);
        dialogBox.SetActive(true);
        dialogBox.GetComponent<DialogBox>().StartDisplay();

        float currentVictoryVolume = gameStateController.victoryMusic.volume;
        gameStateController.victoryMusic.volume = 0;
        gameStateController.victoryMusic.Play();
        StartCoroutine(GameStateController.StartFade(gameStateController.victoryMusic, 5, currentVictoryVolume));

        while (dialogBox != null)
        {
            yield return null;
        }
        // After the dialog box is dismissed fade to black and start the transitions.
        fadeInCanvas.SetActive(true);
        yield return new WaitForSeconds(6f);
        player.GetComponent<TPAbility>().DestroyTP();
        gameStateController.DisableControls();
        gameStateController.gameIsPaused = 0;
        fadeOutCanvas.SetActive(true);
        fadeInCanvas.SetActive(false);
        uiCanvas.SetActive(false);
        victoryCanvas.SetActive(true);

        // Destroy all healthbars
        GameObject healthbar = GameObject.FindGameObjectWithTag("Healthbar");
        if (healthbar != null)
        {
            Destroy(healthbar);
        }

        StartCoroutine(ShiftCamera());
        // Enable player end game position and ambience.
        campfire.SetActive(true);
        player.transform.position = playerEndPoint.position;
        player.transform.rotation = playerEndPoint.rotation;
        player.GetComponent<Animator>().SetLayerWeight(4, 1f);
        RenderSettings.skybox = skybox;
        RenderSettings.skybox.SetFloat("_Rotation", 0);
        fireflies.Play();
        lightObject.intensity = 0f;

        butterflies.SetActive(true);
        OpenAllGates();
    }

    void OpenAllGates()
    {
        GameObject[] gates = GameObject.FindGameObjectsWithTag("Gate");

        foreach (GameObject gate in gates)
        {
            Gate gateScript = gate.GetComponent<Gate>();
            if (gateScript != null)
            {
                gateScript.Open();
            }
        }
    }

    public IEnumerator ShiftCamera()
    {
        int currentCameraPriority = 100;
        var cameraTimers = cameras.Zip(timesForCameras, (c, t) => new { Camera = c, Time = t });
        foreach (var ct in cameraTimers)
        {
            ct.Camera.m_Priority = currentCameraPriority;
            CinemachineTrackedDolly campath = ct.Camera.GetCinemachineComponent<CinemachineTrackedDolly>();
            for (float t = 0f; t < ct.Time; t += Time.deltaTime)
            {
                campath.m_PathPosition = Mathf.Lerp(0, 1, t / ct.Time);
                yield return null;
            }
            campath.m_PathPosition = 1;
            currentCameraPriority++;
        }
    }
}
