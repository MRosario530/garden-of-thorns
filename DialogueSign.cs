using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSign : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private float radius = 1f;
    [SerializeField] private string tooltipText = "Press F to read";

    public GameObject dialogueBoxPrefab;
    private GameObject dialogueBox;

    private GameObject dialogueCanvas;
    private GameObject uiCanvas;
    private GameObject tooltipObject;
    [SerializeField] private GameObject tooltipPrefab;

    public string dialogueText;
    public bool isOneTimeUse;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        tooltipObject = Instantiate(tooltipPrefab, new Vector3(Screen.width / 2, 340, 0), Quaternion.identity);
        uiCanvas = GameObject.Find("Other Canvas");
        tooltipObject.transform.SetParent(uiCanvas.transform);
        tooltipObject.transform.SetParent(uiCanvas.transform);

        Tooltip tooltip = tooltipObject.GetComponent<Tooltip>();
        tooltip.SetMessage(tooltipText);
        tooltip.SetDescription("");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F) && dialogueBox == null && dialogueBoxPrefab != null)
            {
                dialogueBox = Instantiate(dialogueBoxPrefab, new Vector3(Screen.width / 2, Screen.height - 30, 0), Quaternion.identity);
                dialogueBox.transform.SetParent(uiCanvas.transform);
                dialogueBox.GetComponent<DialogBox>().SetText(dialogueText);
                dialogueBox.GetComponent<DialogBox>().StartDisplay();
                if (isOneTimeUse)
                    radius = 0;
            }
            tooltipObject.SetActive(true);
        }
        else
        {
            tooltipObject.SetActive(false);
        }
    }
}
