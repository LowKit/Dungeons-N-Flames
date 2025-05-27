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
        mouseInfoText.gameObject.SetActive(true);
        InfoText.text = info;

        Vector2 mousePos = Input.mousePosition;
        mouseInfoText.transform.position = mousePos;
    }

    private void MouseNoFocus()
    {
        mouseInfoText.gameObject.SetActive(false);
    }
}
