using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    GameObject skill;
    GameObject bag;
    GameObject condition;
    GameObject propety;
    GameObject tips;
    GameObject turn;
<<<<<<< HEAD
    private BasicEntity entity ;
=======

>>>>>>> temp
    public static UiManager uiManager; 

	// Use this for initialization
	void Start () {
<<<<<<< HEAD
        entity = null;
=======

>>>>>>> temp
        skill = null;
        bag = null;
        UiManager.uiManager = this;
        ShowCondition();
        ShowPropety();
        ShowTips();
        ShowTurn();
        tips.SetActive(false);
        propety.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD
        if (entity==StateStaticComponent.m_currentEntity)
        {
            if (entity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Player)
            {
                if (skill == null)
                    ShowSkill(StateStaticComponent.m_currentEntity);

                if (Input.GetButtonDown("Bag"))
                {
                    if (bag != null)
                    {
                        CloseBag();
                    }
                    else
                    {
                        ShowBag(StateStaticComponent.m_currentEntity);
                    }
                }
                if (Input.GetButtonDown("ShowPropety"))
                {
                    if (propety.activeSelf)
                        propety.SetActive(false);
                    else
                    {
                        propety.SetActive(true);
                    }
                }
            }
        }
        else
        {
            entity = StateStaticComponent.m_currentEntity;
            CloseSkill();
            CloseBag();
        }
        
=======
        if (StateStaticComponent.m_currentEntity != null
    && StateStaticComponent.m_currentEntity.GetComponent<BlockInfoComponent>().m_blockType == BlockType.Player &&
    StateStaticComponent.m_currentEntity.GetComponent<InputComponent>() != null)
        {
            if(skill==null)
                ShowSkill(StateStaticComponent.m_currentEntity);

            if (Input.GetButtonDown("Bag"))
            {
                if (bag != null)
                {
                    CloseBag();
                }
                else
                {
                    ShowBag(StateStaticComponent.m_currentEntity);
                }
            }
            //if (Input.GetButtonDown("Propety"))
            //{
            //    if (propety != null)
            //    {
            //        ;
            //    }
            //    else
            //    {
            //        ShowPropety(StateStaticComponent.m_currentEntity);
            //    }
            //}
        }
        else
        {
            CloseSkill();
            CloseBag();
        }
        if (Input.GetButtonDown("ShowPropety"))
        {
            if (propety.activeSelf)
                propety.SetActive(false);
            else
            {
                propety.SetActive(true);
            }
        }
>>>>>>> temp
        if (Input.GetButtonDown("ShowTips"))
        {
            if (tips.activeSelf)
                tips.SetActive(false);
            else
                tips.SetActive(true);
        }
    }
    public void CloseSkill()
    {
        if (skill == null)
            return;
        skill.GetComponentInChildren<UiSkill>().Clear();
        
        DestroyImmediate(skill);
        skill = null;
    }
    public void ShowSkill(BasicEntity entity)
    {
        if (skill != null)
        {
            return;
        }
        skill = (GameObject)Resources.Load("Prefabs/UI/Panel/UISkill");
        skill = Instantiate(skill);
        skill.transform.SetParent(gameObject.transform);
        skill.transform.localPosition = skill.transform.position;
        skill.transform.localScale = Vector3.one;
        UiSkill uiSkill = skill.transform.GetChild(1).gameObject.AddComponent<UiSkill>();
        uiSkill.AddClick(entity);
    }

    public void CloseBag()
    {
        if (bag == null)
        {
            return;
        }
        bag.GetComponentInChildren<UiBag>().Clear();
        DestroyImmediate(bag);
        bag = null;
    }

    public void ShowBag(BasicEntity entity)
    {
        if (bag != null)
        {
            bag.GetComponent<UiBag>().Clear();
            DestroyImmediate(bag);
            bag = null;
        }
        bag = (GameObject)Resources.Load("Prefabs/UI/Panel/UIBag");
        bag = Instantiate(bag);
        bag.transform.SetParent(gameObject.transform);
        bag.transform.localPosition = bag.transform.position;
        bag.transform.localScale = Vector3.one;
        UiBag uiBag = bag.transform.GetChild(1).gameObject.AddComponent<UiBag>();
        uiBag.AddClick(entity);
    }
    public void ShowPropety()
    {
        if (propety != null)
        {
            propety.GetComponent<UiPropety>().Clear();
            DestroyImmediate(propety);
            propety = null;
        }
        propety = (GameObject)Resources.Load("Prefabs/UI/Panel/UIPropety");
        propety = Instantiate(propety);
        propety.transform.parent = gameObject.transform;
        propety.transform.localPosition = propety.transform.position;
        propety.transform.localScale = Vector3.one;
        UiPropety uipropety = propety.AddComponent<UiPropety>();
        
    }

    public void ShowCondition()
    {
        if (condition != null)
        {
            condition.GetComponent<UiCondition>().Clear();
            DestroyImmediate(condition);
            condition = null;
        }
        condition = (GameObject)Resources.Load("Prefabs/UI/Panel/UICondition");
        condition = Instantiate(condition);
        condition.transform.parent = gameObject.transform;
        condition.transform.localPosition = condition.transform.position;
        condition.transform.localScale = Vector3.one;
        UiCondition uicondition = condition.AddComponent<UiCondition>();
    }

    void ShowTips()
    {

        tips = (GameObject)Resources.Load("Prefabs/UI/Panel/UITips");
        tips = Instantiate(tips);
        tips.transform.parent = gameObject.transform;
        tips.transform.localPosition = tips.transform.position;
        tips.transform.localScale = Vector3.one;
        UiTips uitips = tips.AddComponent<UiTips>();
    }

    void ShowTurn()
    {

        turn = (GameObject)Resources.Load("Prefabs/UI/Panel/UITurn");
        turn = Instantiate(turn);
        turn.transform.parent = gameObject.transform;
        turn.transform.localPosition = turn.transform.position;
        turn.transform.localScale = Vector3.one;
        UiTurn uiturn = turn.AddComponent<UiTurn>();
    }
}
