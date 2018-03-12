using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using Msg;
using UnityEngine;
using Skyunion;

public class LoginLogic : LogicBase
{
    private static LoginLogic _instance = null;
    public static LoginLogic Instance()
    {
        return _instance;
    }
    public LoginLogic()
    {
        _instance = this;
    }

    public enum Event : int
    {
        LoginSuccess,
        LoginFailure,
        WorldList,
        ServerList,
        SelectServerSuccess,
    };

    public void Start()
    {
        //AddReceiveCallBack(Msg.EGameMsgID.EGMI_ACK_LOGIN, OnLoginProcess);
    }

    // 请求消息
    public void LoginPB(string strAccount, string strPwd, string strKey)
    {
        //Debug.Log("LoginPB:" + strAccount);
        //Msg.ReqAccountLogin xData = new Msg.ReqAccountLogin();
        //xData.account = System.Text.Encoding.Default.GetBytes(strAccount);
        //xData.password = System.Text.Encoding.Default.GetBytes(strPwd);
        //xData.security_code = System.Text.Encoding.Default.GetBytes(strKey);
        //xData.signBuff = System.Text.Encoding.Default.GetBytes("");
        //xData.clientVersion = 1;
        //xData.loginMode = 0;
        //xData.clientIP = 0;
        //xData.clientMAC = 0;
        //xData.device_info = System.Text.Encoding.Default.GetBytes("");
        //xData.extra_info = System.Text.Encoding.Default.GetBytes("");

        //mAccount = strAccount;

        //MemoryStream stream = new MemoryStream();
        //Serializer.Serialize<Msg.ReqAccountLogin>(stream, xData);

        //SendToServerByPB(Msg.EGameMsgID.EGMI_REQ_LOGIN, stream);
    }
    // 接收消息
    private void OnLoginProcess(UInt16 id, MemoryStream stream)
    {
        Debug.Log("OnLoginProcess1");
        //Msg.MsgBase xMsg = new Msg.MsgBase();
        //xMsg = Serializer.Deserialize<Msg.MsgBase>(stream);

        //Debug.Log("OnLoginProcess2");
        //Msg.AckEventResult xData = new Msg.AckEventResult();
        //xData = Serializer.Deserialize<Msg.AckEventResult>(new MemoryStream(xMsg.msg_data));

        //Debug.Log("OnLoginProcess3");
        //if (EGameEventCode.EGEC_ACCOUNT_SUCCESS == xData.event_code)
        //{
        //    Debug.Log("Login  SUCCESS");
        //    DoEvent((int)Event.LoginSuccess);
        //}
        //else
        //{
        //    Debug.Log("Login Faild,Code: " + xData.event_code);
        //    DataList varList = new NFCDataList();
        //    varList.AddInt((Int64)xData.event_code);
        //    DoEvent((int)Event.LoginFailure);
        //}
    }
};