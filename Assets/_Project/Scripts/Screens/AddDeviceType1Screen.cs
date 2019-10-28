using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que controla um cadastro de um dipositivo tipo 1
/// </summary>
public class AddDeviceType1Screen : MonoBehaviour
{
    [HideInInspector] public DeviceModel deviceModel;

    [Header("About Device")]
    public Text txtSerial;
    public Text txtModel;
    public InputField txtNameDevice;

    [Header("References Output 1")]
    public Text txtDescriptionOutput1;
    public Text txtNameEnvironmentOutput1;
    public Toggle toggleKeepBright;
    public Toggle toggleVisibleOutput1;
    public Dropdown dropdownTypeActionOutput1;
    public Toggle toggleTimingOutput1;
    public InputField inputFieldTimingOutput1;
    public GameObject goTimingOutput1;
    public GameObject goTimeCloseAutomaticOutput1;

    [Header("Others")]
    public ChooseEnvironmentScreen prefabChooseEnvironmentScreen;

    private int outputEnvironmentClicked;

    private void Start()
    {
        ChooseEnvironmentScreen.OnChoosedEnvironment += ChoosedEnvironment;
    }

    public void OnButtonAddDeviceClicked()
    {
        if (toggleKeepBright.isOn)
            deviceModel.responseSetupDevice[1] = 1;
        else
            deviceModel.responseSetupDevice[1] = 0;

        switch (dropdownTypeActionOutput1.value)
        {
            case 0: // Retentivo
                deviceModel.responseSetupDevice[0] = 0;
                deviceModel.responseSetupDevice[2] = 0;
                deviceModel.responseSetupDevice[3] = 0;
                deviceModel.responseSetupDevice[4] = 0;
                deviceModel.responseSetupDevice[5] = 0;

                break;
            case 1: // Pulsante
                deviceModel.responseSetupDevice[0] = 1;

                if (toggleTimingOutput1.isOn)
                {
                    int timeUser = int.Parse(inputFieldTimingOutput1.text) * 1000;
                    byte[] timeUserBytes = timeUser.IntToByteArray();

                    deviceModel.responseSetupDevice[2] = timeUserBytes[0];
                    deviceModel.responseSetupDevice[3] = timeUserBytes[1];
                    deviceModel.responseSetupDevice[4] = timeUserBytes[2];
                    deviceModel.responseSetupDevice[5] = timeUserBytes[3];
                }
                else
                {
                    deviceModel.responseSetupDevice[2] = 0;
                    deviceModel.responseSetupDevice[3] = 0;
                    deviceModel.responseSetupDevice[4] = 0;
                    deviceModel.responseSetupDevice[5] = 0;
                }

                break;
        }

        AppManager.Instance.SendNewSetupDevice(deviceModel.numberSerial, deviceModel.ipTemp, deviceModel.responseSetupDevice);
        //AppManager.Instance.StartSearchDevices();
    }

    public void OnButtonChooseEnvironmetClicked(int output)
    {
        outputEnvironmentClicked = output;
        Instantiate(prefabChooseEnvironmentScreen, GameObject.FindGameObjectWithTag("MainCanvas").transform);
    }

    public void OnValueChangeDropdownOutput1()
    {
        if (dropdownTypeActionOutput1.value == 1)
            goTimingOutput1.SetActive(true);
        else
            OnValueChangeDropdownRetentiveOutput1();
    }

    public void OnValueChangeToggleOutput1(GameObject go)
    {
        if (toggleTimingOutput1.isOn == true)
            go.gameObject.SetActive(true);
        else
            go.gameObject.SetActive(false);
    }

    private void OnValueChangeDropdownRetentiveOutput1()
    {
        goTimingOutput1.gameObject.SetActive(false);
        goTimeCloseAutomaticOutput1.gameObject.SetActive(false);
        toggleTimingOutput1.isOn = false;
        inputFieldTimingOutput1.text = "";
    }

    public void OnButtonBackClicked(GameObject go)
    {
        AppManager.Instance.OnButtonBackScreenClicked(go);
        AppManager.Instance.StartSearchDevices();
    }

    private void ChoosedEnvironment(EnvironmentModel environmentModel)
    {
        if (environmentModel == null)
            return;

        print(string.Format("Button of output {0}, choosed id environment {1} and name environment {2}", outputEnvironmentClicked, environmentModel.id, environmentModel.nameEnvironment));
        txtNameEnvironmentOutput1.text = environmentModel.nameEnvironment;
    }
}