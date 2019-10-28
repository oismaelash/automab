using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe para gerenciar todas as saidas que um ambiente possui
/// </summary>
public class OutputEnvironmentScreen : MonoBehaviour
{
    public int idEnvironment;
    public Text txtNameEnvironment;
    public GameObject goAllDevices;
    [HideInInspector] public int countOutput;


    private void Start()
    {
        if (countOutput > 0)
            goAllDevices.SetActive(true);
    }

    public void SetNameEnvironment(string value)
    {
        txtNameEnvironment.text = value;
    }

    public void OnButtonSearchDevicesClicked()
    {
        AppManager.Instance.OnButtonSearchDevicesClicked();
    }

    public void OnButtonBackClicked()
    {
        AppManager.Instance.OnButtonBackScreenClicked(gameObject);
    }
}