using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo
{
    public BasicEntity entity;
    public Vector3 logPos;
    public BlockInfo(Vector3 _logPos,BasicEntity _entity=null){
        logPos = _logPos;
        entity = _entity;
    }
}

public class VoxelBlocks : MonoBehaviour
{

	[SerializeField]
	public Vector3 m_mapSize;
	[SerializeField]
	public Vector3 m_blockSize;
    
	//blcoks position and block id
	private Dictionary<Vector3, BlockInfo> m_mapData;

    private void Awake()
    {
        //加载地图信息
        m_mapData = new Dictionary<Vector3, BlockInfo>();
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            for (int j = 0; j < child.childCount; j++)
            {
                BlockInfoComponent block = child.GetChild(j).GetComponent<BlockInfoComponent>();
                BasicEntity basicEntity = child.GetChild(j).GetComponent<BasicEntity>();
                BlockInfo blockInfo = new BlockInfo(block.m_logicPosition, basicEntity);
                if (!m_mapData.ContainsKey(block.m_logicPosition))
                {
                    m_mapData.Add(block.m_logicPosition, blockInfo);
                }
            }
        }
    }

    public Vector3 MapSize {
		set {
			m_mapSize = value;
		}
	}

	public Vector3 BlockSize {
		set {
			m_blockSize = value;
		}
	}

    public BlockInfo GetBlockByLogicPos(Vector3 logicPos)
    {
        if(m_mapData.ContainsKey(logicPos))
            return m_mapData[logicPos];
        if (logicPos.x < 0 || logicPos.y < 0 || logicPos.x > m_mapSize.x || logicPos.y > m_mapSize.y)
            return null;
        return new BlockInfo(logicPos);
    }
    //逻辑位置转换世界坐标
    public Vector3 LogToWorld(Vector3 logPos)
    {
        float x = logPos.y * m_blockSize.x+transform.position.x+m_blockSize.x/2;
        float z = -(logPos.x + 1) * m_blockSize.y+transform.position.z+m_blockSize.z/2;

        return new Vector3(x, 0, z);
    }
    //通过逻辑位置获取所有邻居位置
    public List<BlockInfo> GetNeibor(Vector3 logicPos)
    {
        if (GetBlockByLogicPos(logicPos) == null)
            return null;
        List<BlockInfo> list = new List<BlockInfo>();
        List<Vector3> key = new List<Vector3>();
        key.Add(new Vector3((int)logicPos.x+1, (int)logicPos.y, logicPos.z - 1));
        key.Add(new Vector3((int)logicPos.x , (int)logicPos.y+1, logicPos.z - 1));
        key.Add(new Vector3((int)logicPos.x - 1, (int)logicPos.y, logicPos.z - 1));
        key.Add(new Vector3((int)logicPos.x , (int)logicPos.y-1, logicPos.z - 1));
        key.Add(new Vector3((int)logicPos.x + 1, (int)logicPos.y+1, logicPos.z - 1));
        key.Add(new Vector3((int)logicPos.x - 1, (int)logicPos.y+1, logicPos.z - 1));
        key.Add(new Vector3((int)logicPos.x - 1, (int)logicPos.y-1, logicPos.z - 1));
        key.Add(new Vector3((int)logicPos.x + 1, (int)logicPos.y-1, logicPos.z - 1));
        for (int i = 0; i < 8; i++)
        {
            // 判断是否越界，如果没有，加到列表中
            if (key[i].x < m_mapSize.x && key[i].y < m_mapSize.y && key[i].x >= 0 && key[i].y >= 0)
            {
                if (m_mapData.ContainsKey(key[i]))
                {
                    Vector3 n = key[i];
                    n.z += 1;
                    list.Add(GetBlockByLogicPos(n));
                }
            }
        }
        return list;

    }
    //移除方块
    public bool RemoveBlock(Vector3 logPos)
    {
        return m_mapData.Remove(logPos);
    }
    //变换方块位置
    public bool ChangePos(Vector3 s,Vector3 e)
    {
        if (m_mapData.ContainsKey(e)||!m_mapData.ContainsKey(s))
            return false;
        m_mapData.Add(e, m_mapData[s]);
        m_mapData.Remove(s);
        if (m_mapData[e].entity != null)
        {
            m_mapData[e].logPos = e;
            m_mapData[e].entity.GetComponent<BlockInfoComponent>().m_logicPosition = e;
        }
        return true;
    }
	//Vector3 FromLogicToLocal ()
	//{
	//	return new Vector3 (0, 0, 0);
	//}

	//Vector3 FromLocalToLogic ()
	//{
	//	return new Vector3 (0, 0, 0);
	//}
}
