using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Skyunion
{
    public class BaseScene : MonoBehaviour
    {
        // Use this for initialization
        public Dictionary<string, object> mUserData = null;
        void Start()
        {
        }

        public virtual void AddGameObject(GameObject go)
        {
            go.transform.SetParent(transform);
        }
    }
}
