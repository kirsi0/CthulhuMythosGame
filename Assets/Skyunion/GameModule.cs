using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Skyunion
{
    public class GameModule<T> : MonoSingleton<T> where T : GameModule<T>
    {
        public override void Init()
        {
            transform.SetParent(GameRoot.Instance().transform);
        }
    }
}
