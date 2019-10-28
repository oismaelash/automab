using System;
using System.Timers;

/// <summary>
/// Classe de controle sobre comunicacao UDP
/// </summary>
public class UDPController : UDPManager
{
    #region Variables

    public static UDPController Instance;
    private byte[] lastMessageReceveid = null;

    private Timer timerSearchDevice;

    #endregion

    #region Methods MonoBehaviour

    private void Awake()
    {
        Instance = this;
    }

    public override void Start ()
    {
        base.Start();
        
    }

    #endregion

    public override void MessageReceived(byte[] data, string ipHost, int portHost)
    {
        if (lastMessageReceveid != data)
        {
            //print(string.Format("Received data:: {0} of IP:: {1} and Port:: {2}", data.ByteArrayToString(), ipHost, portHost));
            lastMessageReceveid = data;
        }

        AppManager.Instance.AddDeviceFind(ipHost, data);
    }

    #region Methods Acess Commands

    public void StartSearchDevices()
    {
        timerSearchDevice = new Timer(1000);
        timerSearchDevice.Elapsed += OnTimedEvent;
        timerSearchDevice.AutoReset = true;
        timerSearchDevice.Start();
    }

    public void StopSearchDevices()
    {
        timerSearchDevice.Stop();
        timerSearchDevice.Dispose();
        //print("StopSearchDevices");
    }

    #endregion

    #region Methods Privates

    private void SendData_SearchDevices()
    {
        
    }

    private void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        SendDataUDP(Constants.cmdSearchDevices, Constants.lanHost, Constants.portMain);
    }

    #endregion
}