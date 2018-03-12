using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Skyunion
{
    public class UIPanel : MonoBehaviour
    {
        // Use this for initialization
        public Dictionary<string, object> mUserData = null;
        void Start()
        {
        }

        public T GetControl<T>(string name)
        {
            var tran = transform.Find(name);
            if (tran)
            {
                return tran.GetComponent<T>();
            }

            return default(T);
        }

        public void AddButtonClick(string name, UnityAction callback)
        {
            Button button = GetControl<Button>(name);
            if(button)
            {
                button.onClick.AddListener(callback);
            }
            else
            {
                Debug.LogWarning(name + " Not Found");
            }
        }

        public void AddClickEvent(string name, System.Action<BaseEventData> callback)
        {
            EventTrigger trigger = GetControl<EventTrigger>(name);
            if (trigger == null)
            {
                Transform tran = transform.Find(name);
                if(tran)
                    trigger = tran.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
            trigger.triggers.Add(entry);
        }

        public void AddEventTriggerListener(GameObject go, EventTriggerType eventType, System.Action<BaseEventData> callback)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if(trigger == null)
                trigger = go.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
            trigger.triggers.Add(entry);
        }
    }
}
