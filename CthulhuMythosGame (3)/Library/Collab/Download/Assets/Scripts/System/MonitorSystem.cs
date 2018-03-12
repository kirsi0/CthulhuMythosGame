using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorSystem : BasicSystem
{
	public override void Execute (List<BasicEntity> entities)
	{
		foreach (var e in entities) {
			MonitorComponent monitor = e.GetComponent<MonitorComponent> ();
			if (monitor == null)
				return;
			//若监视范围为空或者实体进行移动，重置监控范围
			monitor.m_enemy = new List<BasicEntity> ();
			if (monitor.m_view == null || monitor.m_view.Contains (e.GetComponent<BlockInfoComponent> ().m_logicPosition)) {
				List<Vector3> list = FindPath.GetArea (e.GetComponent<BlockInfoComponent> ().m_logicPosition, monitor.m_SightArea);
				//获取自身位置
				Vector3 s = e.GetComponent<BlockInfoComponent> ().m_logicPosition;
				//初始化视野列表
				monitor.m_view = new List<Vector3> ();
				//获取所有能看到的点
				foreach (var pos in list) {
					if (InView (pos, s))
						monitor.m_view.Add (pos);
				}
			}
			//UISimple ui = GameObject.Find("UI").GetComponent<UISimple>();
			//ui.ShowUI(monitor.m_view, 1);
			VoxelBlocks map = GameObject.Find ("Voxel Map").GetComponent<VoxelBlocks> ();
			//查找所有在视野里的玩家
			foreach (var pos in monitor.m_view) {
				BasicEntity entity = map.GetBlockByLogicPos (pos).entity;
				//判断是否为玩家
				if (entity != null) {
					if (entity.GetComponent<BlockInfoComponent> ().m_blockType == BlockType.Player && !monitor.m_enemy.Contains (entity)) {
						//添加到敌人列表里
						monitor.m_enemy.Add (entity);
					}
				}
			}



		}
	}
	//检查是否能看到该位置
	private bool InView (Vector3 s, Vector3 e)
	{
		VoxelBlocks map = GameObject.Find ("Voxel Map").GetComponent<VoxelBlocks> ();
		Vector3 next = s;
		int deltCol = (int)(e.x - s.x);
		int deltRow = (int)(e.y - s.y);
		int stepCol, stepRow;
		int fraction;
		//初始化路径数组

		stepCol = (int)Mathf.Sign (deltCol);     //获取x轴上的前进方向
		stepRow = (int)Mathf.Sign (deltRow);      //获取z轴上的前进方向
		deltRow = Mathf.Abs (deltRow * 2);
		deltCol = Mathf.Abs (deltCol * 2);

		if (deltCol > deltRow) {
			fraction = deltRow * 2 - deltCol;
			while (next.x != e.x) {
				if (fraction >= 0) {
					next.y += stepRow;
					fraction = fraction - deltCol;
				}
				next.x += stepCol;
				fraction = fraction + deltRow;
				BasicEntity entity = map.GetBlockByLogicPos (next).entity;
				if (entity != null && entity.GetComponent<BlockInfoComponent> ().m_blockType == BlockType.Wall)
					return false;

			}
		} else {
			fraction = deltCol * 2 - deltRow;
			while (next.y != e.y) {
				if (fraction >= 0) {
					next.x += stepCol;
					fraction = fraction - deltRow;
				}
				next.y += stepRow;
				fraction = fraction + deltCol;
				BasicEntity entity = map.GetBlockByLogicPos (next).entity;
				if (entity != null && entity.GetComponent<BlockInfoComponent> ().m_blockType == BlockType.Wall)
					return false;
			}
		}
		return true;
	}
}
