using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe para buscar dispositivos
/// </summary>
public class SearchDeviceScreen : MonoBehaviour
{
    public Transform contentButtonsDevice;
    public RawImage rawImgshowCamera;

    public void OnButtonBackClicked()
    {
        AppManager.Instance.OnButtonBackScreenClicked(gameObject);
        AppManager.Instance.StopSearchDevices();
        AppManager.Instance.OnButtonStopQRCodeReaderClicked();
    }

    public void OnButtonQRCodeClicked()
    {
        AppManager.Instance.OnButtonStartQRCodeReaderClicked();
    }
}