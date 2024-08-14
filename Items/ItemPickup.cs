using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using UnityEngine.Events;
using Unity.VisualScripting;

public class ItemPickup : MonoBehaviour
{
    private PlayerController playerController;
    private ThirdPersonShooterController thirdPersonShooterController;
    [SerializeField] private float radius = 1f;
    private Light lightComp;

    [SerializeField] private string tooltipText = "Press F to pick up";
    private GameObject tooltipObject;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private AudioClip itemPickupSoundClip;
    private Item item;

    void Start()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        thirdPersonShooterController = GameObject.FindAnyObjectByType<ThirdPersonShooterController>();
        item = gameObject.GetComponent<Item>();
        GameObject lightGameObject = new GameObject("The Light");
        lightGameObject.transform.parent = transform;
        lightGameObject.transform.localPosition = new Vector3(0, 0, 0);
        lightGameObject.transform.position += new Vector3(0, 1.5f, 0);
        lightGameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);

        lightComp = lightGameObject.AddComponent<Light>();
        lightComp.type = LightType.Spot;
        lightComp.range = 10;
        lightComp.color = Color.yellow;


        //tooltipObject = Instantiate(tooltipPrefab, new Vector3(-1000, -1000, 0), Quaternion.identity);

        tooltipObject = Instantiate(tooltipPrefab, new Vector3(Screen.width / 2, 340, 0), Quaternion.identity);

        tooltipObject.SetActive(false);
        //tooltipObject.transform.position = new Vector3(Screen.width / 2, 340, 0);
        GameObject uiCanvas = GameObject.Find("Other Canvas");
        tooltipObject.transform.SetParent(uiCanvas.transform);

        Tooltip tooltip = tooltipObject.GetComponent<Tooltip>();
        tooltip.SetMessage(tooltipText);
        tooltip.SetDescription(item.description);
    }

    void Update()
    {
        lightComp.intensity = Mathf.PingPong(Time.time * 10, 20);

        if (Vector3.Distance(transform.position, playerController.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                AudioSource.PlayClipAtPoint(itemPickupSoundClip, transform.position);
                if (item is OnPickupItem pickup)
                {
                    pickup.ApplyEffect();
                }
                thirdPersonShooterController.AddItemToList(item);
                playerController.AddItemToInventory(item);
                Destroy(gameObject);
            }

            tooltipObject.SetActive(true);
        }
        else
        {
            tooltipObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Destroy(tooltipObject);
    }
}