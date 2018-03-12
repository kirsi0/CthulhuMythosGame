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

        AudioDemo audioDemo = GameObject.Find("Audio").transform.GetComponent<AudioDemo>();
        audioDemo.playVoice(AudioType.Walk);
        VoxelBlocks map = GameObject.Find("Voxel Map").transform.GetComponent<VoxelBlocks>();
        map.ChangePos(GetComponent<BlockInfoComponent>().m_logicPosition, logPos);
    }
	public void MoveEnd(Vector3 logPos)

	{
		VoxelBlocks map = GameObject.Find ("Voxel Map").transform.GetComponent<VoxelBlocks> ();
        AudioDemo audioDemo = GameObject.Find("Audio").transform.GetComponent<AudioDemo>();

        audioDemo.pauseVoice(AudioType.Walk);
		map.ChangePos (GetComponent<BlockInfoComponent> ().m_logicPosition, logPos);


	}
    public void ChangePos(Vector3 logPos)
    {
        VoxelBlocks map = GameObject.Find("Voxel Map").transform.GetComponent<VoxelBlocks>();
        map.ChangePos(GetComponent<BlockInfoComponent>().m_logicPosition, logPos);
    }
    public void FindEnemy()
    {
        Invoke("StopMove", GetComponent<PropertyComponent>().moveSpd);
    }
    private void StopMove()
    {
        iTween.Stop(gameObject);
    }
	public void AnimationEnd ()
	{
        //总的行走的步伐除以每AP可移动的速度再+1获得消耗的AP点数

        if (GetComponent<BlockInfoComponent>().m_blockType == BlockType.Enemy)
            GetComponent<AIComponent>().m_enemyState = EnemyState.Wait;
        else
            StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Wait;
	}
    public void AnimationStart()
    {
        if (GetComponent<BlockInfoComponent>().m_blockType == BlockType.Enemy)
        {
            GetComponent<AIComponent>().m_enemyState = EnemyState.Action;
        }
        else
            StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
    }

}