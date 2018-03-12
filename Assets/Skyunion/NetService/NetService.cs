using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ProtoBuf;

namespace Skyunion
{
    public class NetService : GameModule<NetService>
    {
        [SerializeField]
        public string ip;
        [SerializeField]
        public UInt16 port;
        [SerializeField]
        public bool connected;

        void Start()
        {
            Debug.Log("NetService");
            ConnectServer("10.0.2.207", 9300);

            NetClient.Instance().OnConnected += OnConnected;
            NetClient.Instance().OnDisConnected += OnDisConnected;
        }
        void Update()
        {
            NetClient.Instance().doUpdate();
        }

        void OnConnected(object sender, ConnectedEventArgs e)
        {
            connected = true;
        }

        void OnDisConnected(object sender, DisConnectedEventArgs e)
        {
            connected = false;
        }


        public void ConnectServer(string ip, UInt16 port)
        {
            if (NetClient.Instance().isConnected())
                NetClient.Instance().shutDown();

            if (ip == "127.0.0.1" && port != 14001)
            {
                ip = NetClient.Instance().ip;
            }

            NetClient.Instance().ready(ip, port);
            NetClient.Instance().connect();
            this.ip = ip;
            this.port = port;
            this.connected = false;
        }

        public void AddReceiveCallBack(Msg.EGameMsgID id, MessageDispatcher.MessageHandler netHandler)
        {
            NetDispatcher.Instance().AddReceiveCallBack((UInt16)id, netHandler);
        }

        public void SendToServerByPB(Msg.EGameMsgID unMsgID, MemoryStream stream)
        {
            Msg.MsgBase xData = new Msg.MsgBase();
            //xData.player_id = NFToPB(mOwnerID);
            xData.msg_data = stream.ToArray();

            MemoryStream body = new MemoryStream();
            Serializer.Serialize<Msg.MsgBase>(body, xData);

            MemoryStream pack = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(pack);
            UInt32 msgLen = (UInt32)body.Length + (UInt32)ConstDefine.NF_PACKET_HEAD_SIZE;
            writer.Write(NetClient.ConvertUint16((UInt16)unMsgID));
            writer.Write(NetClient.ConvertUint32((UInt32)msgLen));
            body.WriteTo(pack);
            NetClient.Instance().sendMsg(pack);
        }
    }
}
