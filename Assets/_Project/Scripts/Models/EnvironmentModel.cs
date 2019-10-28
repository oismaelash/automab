using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe para guardar informacoes de um ambiente
/// </summary>
public class EnvironmentModel : MonoBehaviour
{
    public int id;
    public int idIcon;
    public string nameEnvironment;
    [SerializeField] private Image imageIcon;
    [SerializeField] private Text txtNameEnvironment;
    public List<object> devices = new List<object>();
    public Action OnButtonClicked;
    public Action<EnvironmentModel> OnButtonClickedWithParameter;

    private void Start()
    {
        EnviromentController.Instance.SetIcon(idIcon, imageIcon);
        txtNameEnvironment.text = nameEnvironment;
        GetComponent<Button>().onClick.AddListener(OnButtonSelectEnvironmentClicked);
    }

    public void OnButtonSelectEnvironmentClicked()
    {
        if (OnButtonClicked != null)
            OnButtonClicked();

        if (OnButtonClickedWithParameter != null)
            OnButtonClickedWithParameter(this);
    }
}