using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Skyunion
{
    // 此类只用来编写逻辑和数据，不用来操作非Logic的代码。
    // 需要和外部交互的话，需要生成事件交给外部来自己注册感兴趣的时间
    // 这边暂时使用 MonoBehaviour 方便调试 后续会把他删除掉的
    public class LogicBase : MonoBehaviour
    {
        public delegate bool EventHandler(Dictionary<string, object> vars);

        public LogicBase()
        {
            mhtEvent = new Dictionary<int, ArrayList>();
		}

        public void RegisterCallback(int nEventID, EventHandler handler)
        {
            ArrayList events;
            if (!mhtEvent.TryGetValue(nEventID, out events))
            {
                events = new ArrayList();
                mhtEvent.Add(nEventID, events);
            }
            events.Add(handler);
		}

        public void DoEvent(int nEventID, Dictionary<string, object> vars = null)
        {
            ArrayList events;
            if (!mhtEvent.TryGetValue(nEventID, out events))
            {
                return;
            }

            foreach (EventHandler handle in events)
            {
                if (!handle(vars))
                    break;
            }
        }

        public void AddReceiveCallBack(Msg.EGameMsgID id, MessageDispatcher.MessageHandler netHandler)
        {
            NetService.Instance().AddReceiveCallBack(id, netHandler);
        }

        public void SendToServerByPB(Msg.EGameMsgID unMsgID, MemoryStream stream)
        {
            NetService.Instance().SendToServerByPB(unMsgID, stream);
        }

        Dictionary<int, ArrayList> mhtEvent;
    }
}