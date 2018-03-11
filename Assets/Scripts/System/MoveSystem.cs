using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MoveSystem : BasicSystem
{
	private ComponentType m_linkedType = ComponentType.Move;

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
		foreach (var entity in entities) {
			MoveComponent move = entity.gameObject.GetComponent<MoveComponent> ();
			AbilityComponent ab = entity.GetComponent<AbilityComponent> ();
			InputComponent input = entity.GetComponent<InputComponent> ();
			StateComponent ap = entity.GetComponent<StateComponent> ();
            BlockType blockType = entity.GetComponent<BlockInfoComponent>().m_blockType;

			int i = AiToInput.GetAbilityCount(entity, M_LinkedType);
            if (i >= ab.m_temporaryAbility.Count || i != input.currentKey)
				continue;
            UISimple ui = GameObject.Find("UI").GetComponent<UISimple>();
            
            VoxelBlocks map = GameObject.Find ("Voxel Map").transform.GetComponent<VoxelBlocks> ();

			if (move.pathList == null) {
				move.pathList = new List<Vector3> ();
				move.pathList.Add (entity.GetComponent<BlockInfoComponent> ().m_logicPosition);
			}
			if (move.path == null) {
				move.path = FindPath.FindPathInStep (move.pathList [move.pathList.Count - 1], ap.m_actionPoint * move.SPD - move.pathList.Count + 1);
			}
			List<Vector3> a = move.path;
            if(blockType!=BlockType.Enemy)
                ui.ShowUI(a, 3);
			if (input.currentPos != null) {
                if (a.Count == 0 && input.leftButtonDown)
                {
                    SetPath(move);
                    move.pathList = null;
                    move.path = null;
                    input.leftButtonDown = false;
                    return;
                }
                foreach (var bi in a) {
					//判断鼠标停靠位置是否位于可移动范围之内
					if (bi == input.currentPos) {
						List<Vector3> path = new List<Vector3> ();
						path.AddRange (move.pathList);
						List<Vector3> newPath = FindPath.GetPath (move.pathList [move.pathList.Count - 1], input.currentPos);
						if (newPath != null)
							path.AddRange (newPath);
                        //调用UI显示路径
                        if (blockType != BlockType.Enemy)
                            ui.ShowUI(path, 2);
						if (input.midButtonDown) {
							move.pathList = path;
							move.path = null;
						}

                        if (input.rightButtonDown)
                        {
                            move.pathList.Clear();
                            move.path = null;
                        }

						if (input.leftButtonDown && ap.m_actionPoint != 0) {
							move.pathList = path;

							SetPath (move);
							move.pathList = null;
							move.path = null;
							input.leftButtonDown = false;
							return;
						}
                    }
				}
                
            }

		}

	}

	private void SetPath (MoveComponent move)
	{
		float time = 0;
		VoxelBlocks map = GameObject.Find ("Voxel Map").transform.GetComponent<VoxelBlocks> ();
		float oa = move.transform.rotation.eulerAngles.y * 3.1415f / 180;

		for (int i = 1; i < move.pathList.Count; i++) {
			Vector3 pos;
			pos = map.LogToWorld (move.pathList [i]) - map.LogToWorld (move.pathList [i - 1]);

			Hashtable args = new Hashtable ();
			float angle = GetAngle (pos);
			if (Mathf.Abs (angle - oa) > 0.1) {
				iTween.RotateTo (move.gameObject, iTween.Hash ("rotation", new Vector3 (0, angle * 180 / 3.1415f, 0), "time", 0.25f, "easeType", "easeInOutBack", "loopType", "none", "delay", time));
				time += 0.25f;
				//获取转向后的前进坐标
				oa = angle;
			}
			pos = RotatePos (pos, -oa);
			args.Add ("easeType", iTween.EaseType.linear);
			//移动的整体时间。如果与speed共存那么优先speed
			args.Add ("time", move.moveSpeed);
			args.Add ("loopType", "none");
			args.Add ("delay", time);
			time += move.moveSpeed;
			// x y z 标示移动的位置。
			args.Add ("x", pos.x);
			args.Add ("z", pos.z);
            args.Add("onstart", "MoveStart");
            args.Add("onstartparams", move.pathList[i]);
            args.Add("onstarttarget", move.gameObject);

			if (move.pathList.Count - 1 == i) {
				int ap = (i - 1) / move.SPD + 1;
                move.GetComponent<StateComponent>().m_actionPoint -= ap;
                move.GetComponent<StateComponent>().Invoke("AnimationEnd", time+0.2f);
                args.Add("oncomplete", "ChangePos");
                args.Add("oncompleteparams", move.pathList[i]);
                args.Add("oncompletetarget", move.gameObject);
            }
			iTween.MoveBy (move.gameObject, args);
		}
		StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
	}

	private float GetAngle (Vector3 pos)
	{
		float angle = Mathf.Atan2 (pos.x, pos.z);
		return angle;
	}

	private Vector3 RotatePos (Vector3 pos, float angle)
	{
		Vector3 newPos = pos;
		newPos.z = pos.z * Mathf.Cos (angle) - pos.x * Mathf.Sin (angle);
		newPos.x = pos.z * Mathf.Sin (angle) + pos.x * Mathf.Cos (angle);
		return newPos;
	}
}
