using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheerUpSystem : BasicSystem
{

	private ComponentType m_linkedType = ComponentType.CheerUp;

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
        foreach(BasicEntity entity in entities)
        {
            AbilityComponent ab = entity.GetComponent<AbilityComponent>();
            InputComponent input = entity.GetComponent<InputComponent>();
            StateComponent ap = entity.GetComponent<StateComponent>();
            CheerUpComponent cu = entity.GetComponent<CheerUpComponent>();

            int i = AiToInput.GetAbilityCount(entity, M_LinkedType);
            if (i >= ab.m_temporaryAbility.Count || i != input.currentKey)
                continue;
            StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
            ap.m_actionPoint += cu.m_addAp ;
            if(!ab.m_coldDown.ContainsKey(M_LinkedType))
                ab.m_coldDown.Add(M_LinkedType, cu.m_coldDown);
            ap.Invoke("AnimationEnd", 1);
        }
	}
}
