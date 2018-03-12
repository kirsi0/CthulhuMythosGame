using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skyunion
{
    public class NetDispatcher : MessageDispatcher
    {
        private static NetDispatcher _instance = null;
        public static NetDispatcher Instance()
        {
            if (_instance == null)
            {
                _instance = new NetDispatcher();
            }
            return _instance;
        }
    };
}