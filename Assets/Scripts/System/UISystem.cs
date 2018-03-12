using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : BasicSystem
{
    private GameObject UICanvas =null;
	private ComponentType m_linkedType = ComponentType.UI;

	//覆盖父类的m_linkedType
	public override ComponentType M_LinkedType {
		set {
			m_linkedType = value;
		}
		get {
			return m_linkedType;
		}
	}
	public override void Execute (List<BasicEntity> entities)
	{
        foreach(var entity in entities)
        {
            if (UICanvas == null)
            {
                UICanvas = new GameObject("UISystem");
                Canvas canvas = UICanvas.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
            }
            GameObject slider = entity.GetComponent<UIComponent>().m_hpSlider;
            VoxelBlocks map = GameObject.Find("Voxel Map").transform.GetComponent<VoxelBlocks>();
            if (slider == null)
            {
                if(AddHpUI(entity)==false)
                    return;
                slider = entity.GetComponent<UIComponent>().m_hpSlider;
            }
            slider.GetComponentInChildren<Slider>().value=entity.GetComponent<DeadComponent>().hp;
            if (slider.GetComponentInChildren<Slider>().value <= 0)
            {
                GameObject.Destroy(slider);
            }
            slider.transform.position = entity.gameObject.transform.position+new Vector3(0,map.GetComponent<VoxelMap>().GetBlockSize().y*1.2f,0);
            slider.transform.rotation = Camera.main.transform.rotation;
        }
	}

    private bool AddHpUI(BasicEntity entity)
    {
        if (UICanvas == null)
            return false;
        GameObject newSlider = (GameObject)Resources.Load("Prefabs/UI/Panel/BloodSlider");
        newSlider = GameObject.Instantiate(newSlider);
        newSlider.transform.parent = UICanvas.transform;
        entity.GetComponent<UIComponent>().m_hpSlider = newSlider;
        Slider slider = newSlider.GetComponentInChildren<Slider>();
        slider.maxValue = entity.GetComponent<PropertyComponent>().HP;
        slider.value = slider.maxValue;
        return true;
    }
}
