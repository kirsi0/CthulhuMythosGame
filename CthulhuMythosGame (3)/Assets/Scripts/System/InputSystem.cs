using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem :BasicSystem {
    private ComponentType m_linkedType = ComponentType.Input;
    private int keySize=6;

    //覆盖父类的m_linkedType
    public override ComponentType M_LinkedType
    {
        set
        {
            m_linkedType = value;
        }
        get
        {
            return m_linkedType;
        }
    }

    public override void Execute(List<BasicEntity> entities)
    {
        int ck = getCurrentKey(keySize);
        Vector3 mousePos = getMousePos();

        foreach(var entity in entities)
        {
            //过滤所有AI控制的角色
            
            InputComponent input = entity.GetComponent<InputComponent>();
            if (entity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Enemy)
            {
                if (input.afkTime > StateStaticComponent.afkLimiteTime)
                {
                    StateStaticComponent.m_currentSystemState = StateStaticComponent.SystemState.Action;
                    StateComponent state = entity.GetComponent<StateComponent>();
                    state.m_actionPoint -= 1;
                    state.Invoke("AnimationEnd", 0.5f);
                }
                input.afkTime += Time.deltaTime;
                continue;
            }
            if (ck != 0)
            {
                input.currentKey = ck;
            }
            if (mousePos != Vector3.down)
            {
                input.currentPos = mousePos;
            }
            input.leftButtonDown = Input.GetMouseButtonDown(0);
            input.rightButtonDown = Input.GetMouseButtonDown(1);   
        }
    }

    private int getCurrentKey(int keySize)
    {
        int key=0;
        string s = "key";
        for(int i = 1; i < keySize; i++)
        {
            if (Input.GetButton(s+i.ToString()))
                key = i;
        }
        if (Input.GetButtonDown(InputType.Bag.ToString()))
            key = (int)InputType.Bag;
        if (Input.GetButtonDown(InputType.StopTurn.ToString()))
            key = (int)InputType.StopTurn;
        if (Input.GetButtonDown(InputType.UseItem.ToString()))
            key = (int)InputType.UseItem;
        if (Input.GetButtonDown(InputType.NextOne.ToString()))
            key = (int)InputType.NextOne;
        return key;
    }

    //获取鼠标点击位置，若在图外则返回Vector3.down
    private Vector3 getMousePos()
    {
        Transform map = GameObject.Find("Voxel Map").transform;
        if (map == null)
            return Vector3.down;
        //新建平面作为被射击的物体
        Plane p = new Plane(map.TransformDirection(Vector3.up), Vector3.zero);
        //从鼠标所在的屏幕位置发射射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 hit = Vector3.zero;
        float dist = 0f;

        //记录鼠标射线起点到击中点的位置
        if (p.Raycast(ray, out dist))
        {
            hit = ray.origin + ray.direction.normalized * dist;
        }
        Vector3 blockSize = map.GetComponent<VoxelMap>().GetBlockSize();
        //鼠标击中的位置需要重新转化为父对象的坐标，防止父对象的移动导致的错位
        Vector3 mouseHit= map.InverseTransformPoint(hit);

        float x = Mathf.Floor(mouseHit.x / blockSize.x) * blockSize.x;
        float z = Mathf.Floor(mouseHit.z / blockSize.z) * blockSize.z;

        //计算逻辑上的位置
        float row = x / blockSize.x;
        float column = Mathf.Abs(z / blockSize.z) - 1;

 
        //检查位置是否越界
        if (column < map.GetComponent<VoxelMap>().mapSize.x && row < map.GetComponent<VoxelMap>().mapSize.y)
            return new Vector3(column, row, 1);
        else
            return Vector3.down;
    }

}
