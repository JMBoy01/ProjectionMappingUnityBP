using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq.Expressions;

public class UDPReceiver : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    [SerializeField] public int port;
    public bool startReceiving = true;
    public bool printToConsole = false;
    public string data;

    // Start is called before the first frame update
    public void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // Receive thread
    private void ReceiveData(){
        client = new UdpClient(port);
        while(startReceiving){
            try{
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                if(printToConsole){
                    print(data);
                }
            }
            catch(Exception e){
                print(e.ToString());
            }
        }
    }
}
