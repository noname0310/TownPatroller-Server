using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TownPatroller.Client;

public class ClientsManagerObj : MonoBehaviour
{
    public ClientsManager clientsManager;

    public void Init(PacketReceiverObj packetReceiverObj)
    {
        clientsManager = new ClientsManager(packetReceiverObj);
    }
}
