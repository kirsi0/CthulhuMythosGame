using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackComponent : BasicComponent
{
	public int STR;
    public List<BasicEntity> enemy=null;
    public int current=0;
}