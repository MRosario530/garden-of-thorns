using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private GameObject pfBullet;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private AudioClip revolverShotSound;
    [SerializeField] private AudioSource revolverAudioSource;
    public float fireRate = 2f;

    private float timeSinceLastFire;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    private Vector3 mouseWorldPosition;

    [Header("Sensitivity")]
    [SerializeField] private float sensitivity;
    [SerializeField] private float aimSensitivityMultiplier;

    [Header("Pause Menu Canvas")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider aimSensitivityMultiplierSlider;
    [SerializeField] private TMP_Text sensitivityText;
    [SerializeField] private TMP_Text aimSensitivityMultiplierText;

    [Header("Settings Canvas")]
    [SerializeField] private Slider sensitivitySliderSettings;
    [SerializeField] private Slider aimSensitivityMultiplierSliderSettings;
    [SerializeField] private TMP_Text sensitivityTextSettings;
    [SerializeField] private TMP_Text aimSensitivityMultiplierTextSettings;

    private const string sensitivityKey = "sensitivity";
    private const string aimSensitivityMultiplierKey = "aimSensitivityMultiplier";
    private const float defaultSensitivity = 1f;
    private const float defaultAimSensitivityMultiplier = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool raycastDebug = false;
    [SerializeField] private GameObject redBallPrefab;
    private GameObject redBall;
    [SerializeField] private GameObject itemHolder;

    private void Awake()
    {
        timeSinceLastFire = 1 / fireRate;
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();

        sensitivity = PlayerPrefs.GetFloat(sensitivityKey, defaultSensitivity);
        aimSensitivityMultiplier = PlayerPrefs.GetFloat(aimSensitivityMultiplierKey, defaultAimSensitivityMultiplier);

        UpdateSensitivitySliders();
        UpdateSensitivityTexts();
    }

    void Start()
    {
        UpdateSensitivitySliders();
        UpdateSensitivityTexts();
    }

    void Update()
    {
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(sensitivity * aimSensitivityMultiplier);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(sensitivity);
        }

        // Raycast from the center of the screen to the world
        mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        // Shooting
        timeSinceLastFire += Time.deltaTime;
        float fireRateInSeconds = 1 / fireRate;

        if (raycastDebug)
        {
            if (redBall == null)
            {
                redBall = Instantiate(redBallPrefab, Vector3.zero, Quaternion.identity);
            }
            redBall.transform.position = mouseWorldPosition;
        }

        if (timeSinceLastFire >= fireRateInSeconds && Input.GetButtonDown("Fire1"))
        {
            // Start cooldown
            timeSinceLastFire = 0;

            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            GameObject bullet = Instantiate(pfBullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            bullet.GetComponent<Bullet>().player = gameObject.transform;
            bullet.GetComponent<Bullet>().itemHolder = itemHolder;
            Destroy(bullet, 5);
            revolverAudioSource.PlayOneShot(revolverShotSound);
        }
    }

    void UpdateSensitivitySliders()
    {
        sensitivitySlider.value = sensitivitySliderSettings.value = sensitivity;
        aimSensitivityMultiplierSlider.value = aimSensitivityMultiplierSliderSettings.value = aimSensitivityMultiplier;
    }

    void UpdateSensitivityTexts()
    {
        sensitivityText.text = sensitivityTextSettings.text = sensitivitySliderSettings.value.ToString("0.00");
        aimSensitivityMultiplierText.text = aimSensitivityMultiplierTextSettings.text = aimSensitivityMultiplierSlider.value.ToString("0.00");
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        PlayerPrefs.SetFloat(sensitivityKey, sensitivity);
        sensitivityText.text = sensitivityTextSettings.text = sensitivity.ToString("0.00");
    }

    public void SetAimSensitivityMultiplier(float newMultiplier)
    {
        aimSensitivityMultiplier = newMultiplier;
        PlayerPrefs.SetFloat(aimSensitivityMultiplierKey, aimSensitivityMultiplier);
        aimSensitivityMultiplierText.text = aimSensitivityMultiplierTextSettings.text = aimSensitivityMultiplier.ToString("0.00");
    }

    public Vector3 GetMousePosition()
    {
        return mouseWorldPosition;
    }

    public void AddItemToList(Item item)
    {
        if (item is OnHitItem)
        {
            Item newItem = (Item)itemHolder.AddComponent(item.GetType());
            newItem.description = item.description;
            newItem.sprite = item.sprite;
        }
    }
}
