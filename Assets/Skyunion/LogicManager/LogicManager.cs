using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Skyunion
{
    public class LogicManager : GameModule<LogicManager>
    {
        void Start()
        {
        }

        public void AddLogic<T>() where T : LogicBase
        {
            var go = new GameObject();
            go.name = typeof(T).ToString();
            go.AddComponent<T>();
            go.transform.SetParent(transform);
        }
    }
}