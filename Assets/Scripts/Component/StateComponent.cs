using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateComponent : BasicComponent
{


	//敏捷


	public int m_actionPoint;

	public override void Init (ComponentType componentType, BasicEntity basicEntity)
	{
		base.Init (componentType, basicEntity);

		m_actionPoint = 5;
	}

    public void MoveStart(Vector3 logPos)
    {
        VoxelBlocks map = GameObject.Find("Voxel Map").transform.GetComponent<VoxelBlocks>();
        AudioDemo audioDemo = GameObject.Find("Audio").transform.GetComponent<AudioDemo>();
        audioDemo.playVoice(AudioType.Walk);
        map.ChangePos(GetComponent<BlockInfoComponent>().m_logicPosition, logPos);
    }
	public void ChangePos(Vector3 logPos)
	{
		VoxelBlocks map = GameObject.Find ("Voxel Map").transform.GetComponent<VoxelBlocks> ();
        AudioDemo audioDemo = GameObject.Find("Audio").transform.GetComponent<AudioDemo>();

        audioDemo.pauseVoice(AudioType.Walk);
		map.ChangePos (GetComponent<BlockInfoComponent> ().m_logicPosition, logPos);
        BasicEntity entity  = map.GetBlockByLogicPos(logPos).entity;
        entity.transform.position = map.LogToWorld(logPos); 
	}
	public void AnimationEnd ()
	{
		//总的行走的步伐除以每AP可移动的速度再+1获得消耗的AP点数


		StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Wait;
	}

}