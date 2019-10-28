using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que gerencia todo o ciclo de vida do aplicativo
/// </summary>
public class AppManager : MonoBehaviour
{
    #region Variables

    public static AppManager Instance;

    [Header("Prefabs")]
    [SerializeField] private DeviceModel btnDevice;
    [SerializeField] private GameObject screenNewEnvironment;
    [SerializeField] private EnvironmentModel btnEnvironment;
    [SerializeField] private OutputEnvironmentScreen screenOutputsEnvironment;
    [SerializeField] private SearchDeviceScreen screenSearchDevices;
    [SerializeField] private AddDeviceType1Screen screenAddDeviceType1;
    [SerializeField] private AddDeviceType2Screen screenAddDeviceType2;
    [SerializeField] private AddDeviceType3Screen screenAddDeviceType3;

    [Space(10)]
    [Header("Contents Scroll View")]
    private Transform contentButtonsDevice;
    [SerializeField] private Transform contentButtonsEnvironment;

    private List<DeviceModel> devicesTemp = new List<DeviceModel>();
    private List<DeviceModel> buttonsDeviceTemp = new List<DeviceModel>();
    private int indexDeviceGetSetupAux = 0;
    private string lastButtonDeviceAdd;
    private bool sendNewSetupDevice = false;

    [Header("Screen Environment")]
    [SerializeField] private GameObject bgNothingEnvironments;
    [SerializeField] private GameObject bgEnvironments;

    private bool sendMessageGetSetup = false;

    private CodeReader codeReader;
    private string lastDataReceveidCodeReader = "";

    private Transform canvasMain;

    #endregion

    #region Methods MonoBehaviour

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CheckEnvironments();
        CodeReader.OnCodeFinished += GetContentQRCode;
        codeReader = GetComponent<CodeReader>();
        canvasMain = GameObject.FindGameObjectWithTag("MainCanvas").transform;
    }

    #endregion

    #region Methods Created

    // TODO: Divide method below, for methods small
    public void AddDeviceFind(string ipDevice, byte[] messageDevice)
    {
        if (sendNewSetupDevice)
        {
            var cmdReceived = new byte[] { messageDevice[0], messageDevice[1] };
            var cmdReceivedString = cmdReceived.ByteArrayToString();

            if (cmdReceivedString == Constants.cmdReceivedDeviceSetupChangeSuccess)
            {
                print(string.Format("Device with IP:: {0} setuped with success", ipDevice));
                sendNewSetupDevice = false;
                return;
            }
            else
            {
                print(string.Format("Device with IP:: {0} setuped with error. Try again now...", ipDevice));
                // TODO Get Infos SendNewSetupDevice(string numberSerial, string ipTemp, byte[] responseSetupDevice)
                // Send again
                return;
            }
        }

        if (sendMessageGetSetup)
        {
            if(messageDevice == null || devicesTemp[indexDeviceGetSetupAux].ipTemp != ipDevice)
            {
                GetDevice(ipDevice, messageDevice);
                return;
            }

            var typeDevice = devicesTemp[indexDeviceGetSetupAux].type;
            devicesTemp[indexDeviceGetSetupAux].responseSetupDevice = messageDevice.GetSetupDevice(typeDevice);
            sendMessageGetSetup = false;
            //print("Add setup device(numberSerial - setup) info:: " + devicesTemp[indexDeviceGetSetupAux].numberSerial + " - " + devicesTemp[indexDeviceGetSetupAux].responseSetupDevice.ByteArrayToString() + " - " + messageDevice.ByteArrayToString());
            UDPController.Instance.StartSearchDevices();
            return;
        }

        if(!string.IsNullOrEmpty(ipDevice))
            GetDevice(ipDevice, messageDevice);
    }

    private void GetDevice(string ipDevice, byte[] messageDevice)
    {
        if (sendMessageGetSetup)
        {
            var messageGetSetup = Constants.cmdGetSetupDeviceBegin + messageDevice.GetSerialDevice().ByteArrayToString() + Constants.cmdGetSetupDeviceEnd;
            UDPController.Instance.SendDataUDP(messageGetSetup, devicesTemp[indexDeviceGetSetupAux].ipTemp, Constants.portMain);
            return;
        }

        var deviceModel = new DeviceModel()
        {
            ipTemp = ipDevice,
            numberSerial = messageDevice.GetSerialDevice().ByteArrayToString(),
            type = messageDevice.GetTypeDevice()
        };

        if (devicesTemp.Count > 0)
        {
            for (var index = 0; index < devicesTemp.Count; index++)
            {
                if (ipDevice == devicesTemp[index].ipTemp)
                {
                    if (devicesTemp[index].responseSetupDevice == null)
                    {
                        indexDeviceGetSetupAux = index;
                        UDPController.Instance.StopSearchDevices();
                        var messageGetSetup = Constants.cmdGetSetupDeviceBegin + messageDevice.GetSerialDevice().ByteArrayToString() + Constants.cmdGetSetupDeviceEnd;
                        UDPController.Instance.SendDataUDP(messageGetSetup, ipDevice, Constants.portMain);
                        sendMessageGetSetup = true;
                        //print("Send Message Get Setup of IP:: " + ipDevice);
                    }

                    return;
                }
            }

            devicesTemp.Add(deviceModel);
            lastButtonDeviceAdd = ipDevice;
        }
        else
        {
            devicesTemp.Add(deviceModel);
            lastButtonDeviceAdd = ipDevice;
        }
    }

    // TODO: Optimize this method below
    private void AddNewButtonDevice() 
    {
        foreach (var button in buttonsDeviceTemp)
            Destroy(button.gameObject);

        buttonsDeviceTemp.Clear();

        foreach (var device in devicesTemp)
        {
            var btnNewDevice = Instantiate(btnDevice, contentButtonsDevice) as DeviceModel;
            btnNewDevice.numberSerial = device.numberSerial;
            btnNewDevice.ipTemp = device.ipTemp;
            btnNewDevice.txtNameButton.text = device.numberSerial;
            btnNewDevice.type = device.type;
            btnNewDevice.model = Utility.GetModelDevice(device.type);
            buttonsDeviceTemp.Add(btnNewDevice);
        }
    }

    public void CheckEnvironments()
    {
        if (EnviromentController.Instance.allEnvironments.Count > 0)
        {
            ShowButtonEnvironments(contentButtonsEnvironment, OnButtonSelectEnvironmentClicked);
            bgNothingEnvironments.SetActive(false);
            bgEnvironments.SetActive(true);
        }
        else
        {
            print("Nothing Environments");
            bgNothingEnvironments.SetActive(true);
            bgEnvironments.SetActive(false);
        }
    }

    public void ShowButtonEnvironments(Transform parent, Action OnButtonClicked)
    {
        for (int i = 0; i < parent.childCount; i++) // Optimize this
            Destroy(parent.GetChild(i).gameObject);

        foreach (var environment in EnviromentController.Instance.allEnvironments)
        {
            var newButton = Instantiate(btnEnvironment, parent);
            newButton.id = environment.id;
            newButton.nameEnvironment = environment.nameEnvironment;
            newButton.devices = environment.devices;
            newButton.idIcon = environment.idIcon;
            newButton.OnButtonClicked = OnButtonClicked;
        }
    }

    public void ShowButtonEnvironments(Transform parent, Action<EnvironmentModel> OnButtonClickedWithParameter)
    {
        for (int i = 0; i < parent.childCount; i++) // Optimize this
            Destroy(parent.GetChild(i).gameObject);

        foreach (var environment in EnviromentController.Instance.allEnvironments)
        {
            var newButton = Instantiate(btnEnvironment, parent);
            newButton.id = environment.id;
            newButton.nameEnvironment = environment.nameEnvironment;
            newButton.devices = environment.devices;
            newButton.idIcon = environment.idIcon;
            newButton.OnButtonClickedWithParameter = OnButtonClickedWithParameter;
        }
    }

    private void GetContentQRCode(string dataQRCode)
    {
        if(lastDataReceveidCodeReader != dataQRCode)
        {
            print("Data QRCode:: " + dataQRCode);
            lastDataReceveidCodeReader = dataQRCode;
        }
    }

    public void StartSearchDevices()
    {
        UDPController.Instance.StartSearchDevices();
        InvokeRepeating("AddNewButtonDevice", 1f, 1f);
    }

    public void StopSearchDevices()
    {
        UDPController.Instance.StopSearchDevices();
        CancelInvoke("AddNewButtonDevice");
        devicesTemp.Clear();
        buttonsDeviceTemp.Clear();

        for (int i = 0; i < contentButtonsDevice.childCount; i++)
            Destroy(contentButtonsDevice.GetChild(i).gameObject);
    }

    public void OnButtonDeviceClicked(int type, DeviceModel deviceModel)
    {
        switch (type)
        {
            case 1:
                var screenSetup1 = Instantiate(screenAddDeviceType1, canvasMain);
                screenSetup1.txtSerial.text = deviceModel.numberSerial;
                screenSetup1.txtModel.text = deviceModel.model;
                screenSetup1.txtNameDevice.placeholder.GetComponent<Text>().text = string.Format("{0} {1}", deviceModel.model, deviceModel.numberSerial);
                screenSetup1.txtNameDevice.text = string.Format("{0} {1}", deviceModel.model, deviceModel.numberSerial);
                screenSetup1.deviceModel = deviceModel;

                foreach (var device in devicesTemp)
                {
                    if (device.ipTemp == screenSetup1.deviceModel.ipTemp)
                        screenSetup1.deviceModel.responseSetupDevice = device.responseSetupDevice;
                }

                print("Instantiate View Setup Device Type 1");
                break;
            case 2:
                var screenSetup2 = Instantiate(screenAddDeviceType2, canvasMain);
                screenSetup2.txtSerial.text = deviceModel.numberSerial;
                screenSetup2.txtModel.text = deviceModel.model;
                screenSetup2.txtNameDevice.placeholder.GetComponent<Text>().text = string.Format("{0} {1}", deviceModel.model, deviceModel.numberSerial);
                screenSetup2.txtNameDevice.text = string.Format("{0} {1}", deviceModel.model, deviceModel.numberSerial);
                screenSetup2.deviceModel = deviceModel;

                foreach (var device in devicesTemp)
                {
                    if (device.ipTemp == screenSetup2.deviceModel.ipTemp)
                        screenSetup2.deviceModel.responseSetupDevice = device.responseSetupDevice;
                }
                
                print("Instantiate View Setup Device Type 2");
                break;
            case 3:
                Instantiate(screenAddDeviceType3, canvasMain);
                print("Instantiate View Setup Device Type 3");
                break;
        }
    }

    public void SendNewSetupDevice(string numberSerial, string ipTemp, byte[] responseSetupDevice)
    {
        print("Send new setup for device with IP:: " + ipTemp);
        var newSetup = Constants.cmdSendSetupDeviceBegin + numberSerial + responseSetupDevice.ByteArrayToString();
        UDPController.Instance.SendDataUDP(newSetup, ipTemp, Constants.portMain);
        sendNewSetupDevice = true;
        print("SendNewSetupDevice Response:: " + newSetup);
    }

    #endregion

    #region Methods UI

    public void OnButtonNewEnvironment()
    {
        Instantiate(screenNewEnvironment, canvasMain);
    }

    public void OnButtonStartQRCodeReaderClicked()
    {
        codeReader.StartWork();
    }

    public void OnButtonStopQRCodeReaderClicked()
    {
        codeReader.StopWork();
    }

    public void OnButtonSelectEnvironmentClicked(EnvironmentModel environment)
    {
        print("Open Environment:: " + environment.nameEnvironment);
        OutputEnvironmentScreen outputEnvironmentScreen = Instantiate(screenOutputsEnvironment, canvasMain);
        outputEnvironmentScreen.SetNameEnvironment(environment.nameEnvironment);
        outputEnvironmentScreen.idEnvironment = environment.id;
    }

    public void OnButtonBackScreenClicked(GameObject screen)
    {
        Destroy(screen);
    }

    public void OnButtonSearchDevicesClicked()
    {
        var screenSearchDevice = Instantiate(screenSearchDevices, canvasMain);
        PreviewController.Instance.rawImg = screenSearchDevice.rawImgshowCamera;
        contentButtonsDevice = screenSearchDevice.contentButtonsDevice;
        StartSearchDevices();
    }

    #endregion
}