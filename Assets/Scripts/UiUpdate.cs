using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiUpdate : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image mouseInfoText;
    [SerializeField] private TextMeshProUGUI InfoText;

    [Header("Interact Message Settings")]
    [SerializeField] private TextMeshProUGUI interactMessageText;
    [SerializeField] private float interactMessageDuration = 2f; // Duration to show interaction message

    void Start()
    {
        if (interactMessageText != null)
        {
            interactMessageText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (mouseInfoText.gameObject.activeSelf)
        {
            mouseInfoText.transform.position = Input.mousePosition;
        }
    }

    private void OnEnable()
    {
        PlayerController.OnHealthChange += HealthChage;
        Interactable.OnFocusEvent += MouseFocus;
        Interactable.OnLoseFocusEvent += MouseNoFocus;
        Interactable.OnInteractEvent += ShowInteractMessage;
    }

    private void OnDisable()
    {
        PlayerController.OnHealthChange -= HealthChage;
        Interactable.OnFocusEvent -= MouseFocus;
        Interactable.OnLoseFocusEvent -= MouseNoFocus;
        Interactable.OnInteractEvent -= ShowInteractMessage;
    }

    private void HealthChage(float currenthp, float maxhp)
    {
        float fill = currenthp / maxhp;
        healthBarFill.fillAmount = fill;
    }

    private void MouseFocus(string info)
    {
        if (Time.timeScale == 0f) return; // Bloqueia a exibição se estiver pausado

        mouseInfoText.gameObject.SetActive(true);
        InfoText.text = info;
        mouseInfoText.transform.position = Input.mousePosition;
    }

    private void MouseNoFocus()
    {
        if (Time.timeScale == 0f) return; // Impede desativar/ativar durante pausa

        mouseInfoText.gameObject.SetActive(false);
    }

    private void ShowInteractMessage(string message)
    {
        StartCoroutine(ShowMessageCoroutine(message));
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        if (interactMessageText == null)
            yield break;

        interactMessageText.text = message;
        interactMessageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(interactMessageDuration);

        interactMessageText.gameObject.SetActive(false);
    }
    public void OcultarInteracao()
    {
        mouseInfoText.gameObject.SetActive(false);
    }


}
