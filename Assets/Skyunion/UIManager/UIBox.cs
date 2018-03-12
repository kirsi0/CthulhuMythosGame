using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Skyunion
{
    public class UIBox : UIPanel
    {
        // Use this for initialization
        public virtual void Start()
        {
            AddClickEvent("Mask", OnMaskClick);
            AddButtonClick("Content/Close", Close);
            AddButtonClick("Content/Cancel", Close);
        }

        public void OnMaskClick(BaseEventData eventData)
        {
            Close();
        }

        public void Close()
        {
            UIManager.Instance().CloseBox();
        }
    }
}
