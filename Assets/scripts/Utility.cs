using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public static class Utility 
{
    public static string GetAddressIP()
    {
        string AddressIP = string.Empty;
        foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            Debug.Log(_IPAddress.AddressFamily.ToString());
            if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
            {
                AddressIP = _IPAddress.ToString();
            }
        }

        return AddressIP;
    }

    public static bool CheckIP(string ipStr)
    {
        IPAddress ip;

        if(IPAddress.TryParse(ipStr,out ip))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
