using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiUpdate : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image mouseInfoText;
    [SerializeField] private TextMeshProUGUI InfoText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
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
    }

    private void OnDisable()
    {
        PlayerController.OnHealthChange -= HealthChage;
        Interactable.OnFocusEvent -= MouseFocus;
        Interactable.OnLoseFocusEvent -= MouseNoFocus;
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
    public void OcultarInteracao()
    {
        mouseInfoText.gameObject.SetActive(false);
    }

}
