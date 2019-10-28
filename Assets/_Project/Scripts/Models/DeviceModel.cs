using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que guarda informacoes genericas de qualquer tipo de dispositivo
/// </summary>
[RequireComponent(typeof(Button))]
public class DeviceModel : MonoBehaviour
{
    public string numberSerial;
    public string model;
    public string ipTemp;
    public Text txtNameButton;
    public int type;
    [HideInInspector] public byte[] responseSetupDevice;
    //[HideInInspector] public List<ControllerModel> controllers;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonDeviceClicked);
    }

    public void OnButtonDeviceClicked()
    {
        AppManager.Instance.OnButtonDeviceClicked(type, GetComponent<DeviceModel>());
        AppManager.Instance.StopSearchDevices();
    }
}