using System;
using System.Collections.Generic;

// Extension Methods 
public static class Utility
{
    public static byte[] GetSerialDevice(this byte[] data)
    {
        List<byte> idDevice = new List<byte>();

        for (int i = 2; i < 8; i++)
            idDevice.Add(data[i]);

        return idDevice.ToArray();
    }

    public static int GetTypeDevice(this byte[] data)
    {
        return Convert.ToInt32(data[8]);
    }

    public static byte[] GetSetupDevice(this byte[] data, int typeDevice)
    {
        //C122 (2 bytes) + ID dele mesmo (6 bytes) + (bytes de configuração - variam conforme modelo do dispositivo) + EEEEDDAA (4 bytes)
        List<byte> bytesSetup = new List<byte>();

        switch (typeDevice)
        {
            case 1:
                var countBytesCounter1 = 25 - 4; 
                for (int i = 8; i < countBytesCounter1; i++)
                    bytesSetup.Add(data[i]);
                break;
            case 2:
                var countBytesCounter2 = 29 - 4;
                for (int i = 8; i < countBytesCounter2; i++)
                    bytesSetup.Add(data[i]);
                break;
            case 3:
                var countBytesCounter3 = 29 - 4;
                for (int i = 8; i < countBytesCounter3; i++)
                    bytesSetup.Add(data[i]);
                break;
        }

        return bytesSetup.ToArray();
    }

    public static string GetModelDevice(int type)
    {
        var model = "";

        switch (type)
        {
            case 1:
                model = "Módulo Dimmer";
                break;
            case 2:
                model = "Módulo Relé Duplo";
                break;
            case 3:
                model= "Módulo Emissor IR e RF";
                break;
        }

        return model;
    }

    public static int GeneratePortRandom()
    {
        return UnityEngine.Random.Range(1000, 9999);
    }

    public static string DecimalToHexadecimal(this int valueDecimal)
    {
        return valueDecimal.ToString("X");
    }

    public static byte[] IntToByteArray(this int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return bytes;
    }
}