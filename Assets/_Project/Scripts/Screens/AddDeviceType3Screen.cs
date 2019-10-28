using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que controla um cadastro de um dipositivo tipo 3
/// </summary>
public class AddDeviceType3Screen : MonoBehaviour
{
    #region  Variables

    [HideInInspector] public DeviceModel deviceModel;

    [Header("About Device")]
    public Text txtSerial;
    public Text txtModel;
    public InputField txtNameDevice;
    public Text txtNameEnvironmentOutput;

    [Header("References Controllers")]
    [SerializeField] private ControllerModel prefabControllerModel;
    [SerializeField] private Transform groupControllers;
    public List<ControllerModel> controllers = new List<ControllerModel>();


    [Header("Others")]
    public ChooseEnvironmentScreen prefabChooseEnvironmentScreen;

    #endregion

    #region Methods MonoBeahaviour

    private void Start()
    {
        ChooseEnvironmentScreen.OnChoosedEnvironment += ChoosedEnvironment;
        SaveControllerScreen.OnNewControllerSave += NewController;
    }

    #endregion

    #region Methods Created

    public void OnButtonAddDeviceClicked()
    {
        //switch (dropdownTypeActionOutput1.value)
        //{
        //    case 0: // Retentivo
        //        deviceModel.responseSetupDevice[0] = 0;
        //        deviceModel.responseSetupDevice[1] = 0;
        //        deviceModel.responseSetupDevice[2] = 0;
        //        deviceModel.responseSetupDevice[3] = 0;
        //        deviceModel.responseSetupDevice[4] = 0;
        //        break;
        //    case 1: // Pulsante
        //        deviceModel.responseSetupDevice[0] = 1;

        //        if (toggleTimingOutput1.isOn)
        //        {
        //            int timeUser = int.Parse(inputFieldTimingOutput1.text) * 1000;
        //            byte[] timeUserBytes = timeUser.IntToByteArray();

        //            deviceModel.responseSetupDevice[1] = timeUserBytes[0];
        //            deviceModel.responseSetupDevice[2] = timeUserBytes[1];
        //            deviceModel.responseSetupDevice[3] = timeUserBytes[2];
        //            deviceModel.responseSetupDevice[4] = timeUserBytes[3];
        //        }
        //        else
        //        {
        //            deviceModel.responseSetupDevice[1] = 0;
        //            deviceModel.responseSetupDevice[2] = 0;
        //            deviceModel.responseSetupDevice[3] = 0;
        //            deviceModel.responseSetupDevice[4] = 0;
        //        }

        //        break;
        //}

        //switch (dropdownTypeActionOutput2.value)
        //{
        //    case 0: // Retentivo
        //        deviceModel.responseSetupDevice[5] = 0;
        //        deviceModel.responseSetupDevice[6] = 0;
        //        deviceModel.responseSetupDevice[7] = 0;
        //        deviceModel.responseSetupDevice[8] = 0;
        //        deviceModel.responseSetupDevice[9] = 0;
        //        break;
        //    case 1: // Pulsante
        //        deviceModel.responseSetupDevice[5] = 1;

        //        if (toggleTimingOutput2.isOn)
        //        {
        //            int timeUser = int.Parse(inputFieldTimingOutput2.text) * 1000;
        //            byte[] timeUserBytes = timeUser.IntToByteArray();

        //            deviceModel.responseSetupDevice[6] = timeUserBytes[0];
        //            deviceModel.responseSetupDevice[7] = timeUserBytes[1];
        //            deviceModel.responseSetupDevice[8] = timeUserBytes[2];
        //            deviceModel.responseSetupDevice[9] = timeUserBytes[3];
        //        }
        //        else
        //        {
        //            deviceModel.responseSetupDevice[6] = 0;
        //            deviceModel.responseSetupDevice[7] = 0;
        //            deviceModel.responseSetupDevice[8] = 0;
        //            deviceModel.responseSetupDevice[9] = 0;
        //        }

        //        break;
        //}

        AppManager.Instance.SendNewSetupDevice(deviceModel.numberSerial, deviceModel.ipTemp, deviceModel.responseSetupDevice);
        //AppManager.Instance.StartSearchDevices();
    }

    public void OnButtonChooseEnvironmetClicked(int output)
    {
        Instantiate(prefabChooseEnvironmentScreen, GameObject.FindGameObjectWithTag("MainCanvas").transform);
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

        print(string.Format("Choosed id environment {1} and name environment {2}", environmentModel.id, environmentModel.nameEnvironment));
        txtNameEnvironmentOutput.text = environmentModel.nameEnvironment;
    }

    private void NewController(ControllerModel controllerModel)
    {
        if (controllerModel == null)
            return;

        // CONTINUE HERE: STOP IN 13/06 - 01:04
        // Instantiate:: prefabControllerModel in group in group.Count-2
        print(string.Format("New Controller, id {0} and name {1}", controllerModel.id, controllerModel.Name));
        controllers.Add(controllerModel);
    }

    #endregion
}