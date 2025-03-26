using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    private LayerMask ruleLayer;
    private SpriteRenderer render;

    private int level;
    public bool inLevel;
    private bool transitioning;

    public GameObject[] VFX;
    public GameObject[] levels;
    public GameObject[] menuSelectors;
    public GameObject mainMenu;
    public GameObject levelSelect;

    private List<GameObject> rules;
    private List<GameObject> objects;
    private List<GameObject> queue;
    private List<string> tags;
    private List<GameObject> you;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        render = GetComponent<SpriteRenderer>();

        mainMenu.SetActive(true);
        inLevel = false;
        transitioning = false;
        for (int i = 0; i < levels.Length; i++) {
            levels[i].SetActive(false);
        }
    }

    private void LevelStart() {
        ruleLayer = LayerMask.GetMask("Rules");

        rules = new List<GameObject>();
        objects = new List<GameObject>();
        queue = new List<GameObject>();
        tags = new List<string>();
        you = new List<GameObject>();

        GetLevel(level);
        GameUpdate();
        inLevel = true;
    }

    // Update is called once per frame
    void Update() {
        if (inLevel) {
            int count = 0;
            for (int i = 0; i < you.Count; i++) {
                if (you[i].transform.position == you[i].GetComponent<BlockBehavior>().targetPosition) {
                    count++;
                }
            }
            if (count == you.Count) {
                GameUpdate();
            }
        }
        if (transitioning) {
            render.color = new Color(render.color.r, render.color.g, render.color.b, Mathf.Lerp(render.color.a, 1f, 0.01f));
        }
        else {
            render.color = new Color(render.color.r, render.color.g, render.color.b, Mathf.Lerp(render.color.a, 0f, 0.01f));
        }
    }

    public void GameUpdate() {
        you = GetYou();
        rules = GetRuleBlocks(level);
        EvaluateRules(rules);
    }

    private List<GameObject> GetYou() {
        List<GameObject> temp1 = GetObjectBlocks(level);
        List<GameObject> temp2 = new();
        for (int i = 0; i < temp1.Count; i++) { 
            if (temp1[i].GetComponent<BlockBehavior>().tags.Contains("You")) {
                temp2.Add(temp1[i]);
            }
        }
        return temp2;
    }

    private void GetLevel(int levelNum) { 
        for (int i = 0; i < levels.Length; i++) {
            levels[i].SetActive(false);
        }
        levels[levelNum].SetActive(true);
    }

    private List<GameObject> GetRuleBlocks(int level) {
        List<GameObject> temp = new();
        for (int i = 0; i < levels[level].transform.childCount; i++) {
            if ((ruleLayer & (1 << levels[level].transform.GetChild(i).gameObject.layer)) != 0) {
                temp.Add(levels[level].transform.GetChild(i).gameObject);
            }
            if (levels[level].transform.GetChild(i).transform.childCount > 0) {
                for (int j = 0; j < levels[level].transform.GetChild(i).transform.childCount; j++) {
                    if ((ruleLayer & (1 << levels[level].transform.GetChild(i).transform.GetChild(j).gameObject.layer)) != 0) {
                        temp.Add(levels[level].transform.GetChild(i).transform.GetChild(j).gameObject);
                    }
                }
            }
        }
        return temp;
    }

    private List<GameObject> GetObjectBlocks(int level) {
        List<GameObject> temp = new();
        for (int i = 0; i < levels[level].transform.childCount; i++) { 
            if (levels[level].transform.GetChild(i).CompareTag("Object")) {
                temp.Add(levels[level].transform.GetChild(i).gameObject);
            }
            if (levels[level].transform.GetChild(i).transform.childCount > 0) { 
                for (int j = 0; j < levels[level].transform.GetChild(i).transform.childCount; j++) { 
                    if (levels[level].transform.GetChild(i).transform.GetChild(j).CompareTag("Object")) {
                        temp.Add(levels[level].transform.GetChild(i).transform.GetChild(j).gameObject);
                    }
                }
            }
        }
        return temp;
    }

    private List<GameObject> RemoveTags(List<GameObject> objectList) {
        for (int i = 0; i < objectList.Count; i++) {
            objectList[i].GetComponent<BlockBehavior>().tags.Clear();
        }

        for (int i = 0; i < rules.Count; i++) {
            rules[i].GetComponent<BlockBehavior>().isConnected = false;
        }

        if (objectList.Count == 0) {
            return new List<GameObject>();
        }

        return objectList;
    }

    private void EvaluateRules(List<GameObject> ruleList) {
        queue = ruleList;
        objects = RemoveTags(GetObjectBlocks(level));

        for (int i = 0; i < ruleList.Count; i++) {
            tags = new List<string>();

            if (Physics.Raycast(ruleList[i].transform.position, Vector3.left, out RaycastHit leftHit, 0.9f, ruleLayer)) { 
                if (!leftHit.transform.gameObject.CompareTag("Connector Block") || !leftHit.transform.gameObject.GetComponent<ConnectorBehavior>().isHorizontalRule) { 
                    if (Physics.Raycast(ruleList[i].transform.position, Vector3.right, out RaycastHit rightHit, 0.9f, ruleLayer)) { 
                        if (rightHit.transform.gameObject.CompareTag("Connector Block") && rightHit.transform.gameObject.GetComponent<ConnectorBehavior>().isHorizontalRule) {
                            ruleList[i].GetComponent<BlockBehavior>().isConnected = true;
                            rightHit.transform.gameObject.GetComponent<BlockBehavior>().isConnected = true;
                            GetRule(rightHit.transform.gameObject.GetComponent<ConnectorBehavior>().rules[0, 1], true);
                            ImplementRule(ruleList[i].name, tags);
                        }
                    }
                }
            }
            else {
                if (Physics.Raycast(ruleList[i].transform.position, Vector3.right, out RaycastHit rightHit, 0.9f, ruleLayer)) {
                    if (rightHit.transform.gameObject.CompareTag("Connector Block") && rightHit.transform.gameObject.GetComponent<ConnectorBehavior>().isHorizontalRule) {
                        ruleList[i].GetComponent<BlockBehavior>().isConnected = true;
                        rightHit.transform.gameObject.GetComponent<BlockBehavior>().isConnected = true;
                        GetRule(rightHit.transform.gameObject.GetComponent<ConnectorBehavior>().rules[0, 1], true);
                        ImplementRule(ruleList[i].name, tags);
                    }
                }
            }

            if (Physics.Raycast(ruleList[i].transform.position, Vector3.up, out RaycastHit upHit, 0.9f, ruleLayer)) {
                if (!upHit.transform.gameObject.CompareTag("Connector Block") || !upHit.transform.gameObject.GetComponent<ConnectorBehavior>().isVerticalRule) {
                    if (Physics.Raycast(ruleList[i].transform.position, Vector3.down, out RaycastHit downHit, 0.9f, ruleLayer)) {
                        if (downHit.transform.gameObject.CompareTag("Connector Block") && downHit.transform.gameObject.GetComponent<ConnectorBehavior>().isVerticalRule) {
                            ruleList[i].GetComponent<BlockBehavior>().isConnected = true;
                            downHit.transform.gameObject.GetComponent<BlockBehavior>().isConnected = true;
                            GetRule(downHit.transform.gameObject.GetComponent<ConnectorBehavior>().rules[1, 1], false);
                            ImplementRule(ruleList[i].name, tags);
                        }
                    }
                }
            }
            else {
                if (Physics.Raycast(ruleList[i].transform.position, Vector3.down, out RaycastHit downHit, 0.9f, ruleLayer)) {
                    if (downHit.transform.gameObject.CompareTag("Connector Block") && downHit.transform.gameObject.GetComponent<ConnectorBehavior>().isVerticalRule) {
                        ruleList[i].GetComponent<BlockBehavior>().isConnected = true;
                        downHit.transform.gameObject.GetComponent<BlockBehavior>().isConnected = true;
                        GetRule(downHit.transform.gameObject.GetComponent<ConnectorBehavior>().rules[1, 1], false);
                        ImplementRule(ruleList[i].name, tags);
                    }
                }
            }
        }
    }

    private void GetRule(GameObject block, bool isHorizontal) {
        tags.Add(block.name);
        block.GetComponent<BlockBehavior>().isConnected = true;

        if (isHorizontal) {
            if (Physics.Raycast(block.transform.position, Vector3.right, out RaycastHit rightHit, 0.9f, ruleLayer)) { 
                if (rightHit.transform.gameObject.CompareTag("Connector Block") && rightHit.transform.gameObject.GetComponent<ConnectorBehavior>().isHorizontalRule) {
                    GetRule(rightHit.transform.gameObject.GetComponent<ConnectorBehavior>().rules[0, 1], true);
                }
            }
        }
        else { 
            if (Physics.Raycast(block.transform.position, Vector3.down, out RaycastHit downHit, 0.9f, ruleLayer)) { 
                if (downHit.transform.gameObject.CompareTag("Connector Block") && downHit.transform.gameObject.GetComponent<ConnectorBehavior>().isVerticalRule) {
                    GetRule(downHit.transform.gameObject.GetComponent<ConnectorBehavior>().rules[1, 1], true);
                }
            }
        }
    }

    private void ImplementRule(string objectName, List<string> attributes) {
        for (int i = 0; i < objects.Count; i++) { 
            if (objects[i].name == objectName) { 
                for (int j = 0; j < attributes.Count; j++) {
                    objects[i].GetComponent<BlockBehavior>().tags.Add(attributes[j]);
                }
            }
        }
    }

    public void GoToLevelSelect() {
        StartCoroutine(Wait(2, 1, 0));
    }

    public void GoToMenu() {
        StartCoroutine(Wait(2, 2, 0));
    }

    public void GoToLevel(int num) {
        StartCoroutine(Wait(2, 3, num));
    }

    IEnumerator Wait(float seconds, int action, int subaction) {
        transitioning = true;
        yield return new WaitForSeconds(seconds);
        inLevel = false;
        transitioning = false;
        if (action == 1) {
            menuSelectors[1].GetComponent<LevelSelectScript>().LoadLevelSelect();
            levelSelect.SetActive(true);
            mainMenu.SetActive(false);
            levels[0].SetActive(false);
            levels[1].SetActive(false);
            levels[2].SetActive(false);
        }
        else if (action == 2) {
            menuSelectors[0].GetComponent<MenuScript>().LoadMenu();
            mainMenu.SetActive(true);
            levelSelect.SetActive(false);
            levels[0].SetActive(false);
            levels[1].SetActive(false);
            levels[2].SetActive(false);
        }
        else if (action == 3) {
            level = subaction;
            mainMenu.SetActive(false);
            levelSelect.SetActive(false);
            if (subaction == 0) {
                levels[0].SetActive(true);
            }
            else if (subaction == 1) {
                levels[0].SetActive(false);
                levels[1].SetActive(true);
            }
            else if (subaction == 2) {
                levels[1].SetActive(false);
                levels[2].SetActive(true);
            }
            LevelStart();
            ResetLevel(levels[subaction]);
        }
    }

    private void ResetLevel(GameObject levelObject) {
        for (int i = 0; i < levelObject.transform.childCount; i++) { 
            if (!levelObject.transform.GetChild(i).gameObject.CompareTag("Untagged")) {
                levelObject.transform.GetChild(i).gameObject.GetComponent<BlockBehavior>().ResetPos();
            }
            else if (levelObject.transform.GetChild(i).transform.childCount > 0) { 
                for (int j = 0; j < levelObject.transform.GetChild(i).transform.childCount; j++) { 
                    if (!levelObject.transform.GetChild(i).transform.GetChild(j).gameObject.CompareTag("Untagged")) {
                        levelObject.transform.GetChild(i).transform.GetChild(j).gameObject.GetComponent<BlockBehavior>().ResetPos();
                    }
                }
            }
        }
    }
}
