using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    private LayerMask objectLayer;

    private int level = 0;
    private string levelName;
    private string objectName, attributeName;

    public GameObject[] VFX;
    public GameObject[] levels;

    private List<GameObject> connectors;
    private List<GameObject> objects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        objectLayer = LayerMask.GetMask("Objects");

        objectName = "";
        attributeName = "";
    }

    // Update is called once per frame
    void Update() {
        connectors = GetConnectors(level);
        objects = GetObjects(level);

        for (int i = 0; i < connectors.Count; i++) {
            if (connectors[i].GetComponent<BlockBehavior>().isConnected) {
                objectName = connectors[i].GetComponent<ConnectorBehavior>().objectBlock.gameObject.name;
                attributeName = connectors[i].GetComponent<ConnectorBehavior>().attributeBlock.gameObject.name;
                AddTags();
            }
        }
    }

    private List<GameObject> GetConnectors(int level) {
        List<GameObject> temp = new();
        for (int i = 0; i < levels[level].transform.childCount; i++) {
            if (levels[level].transform.GetChild(i).gameObject.CompareTag("Connector Block")) {
                temp.Add(levels[level].transform.GetChild(i).gameObject);
            }
            if (levels[level].transform.GetChild(i).transform.childCount > 0) {
                for (int j = 0; j < levels[level].transform.GetChild(i).transform.childCount; j++) {
                    if (levels[level].transform.GetChild(i).transform.GetChild(j).gameObject.CompareTag("Connector Block")) {
                        temp.Add(levels[level].transform.GetChild(i).transform.GetChild(j).gameObject);
                    }
                }
            }
        }
        return temp;
    }

    private List<GameObject> GetObjects(int level) {
        List<GameObject> temp = new();
        for (int i = 0; i < levels[level].transform.childCount; i++) {
            if ((objectLayer & (1 << levels[level].transform.GetChild(i).gameObject.layer)) != 0) {
                temp.Add(levels[level].transform.GetChild(i).gameObject);
            }
            if (levels[level].transform.GetChild(i).transform.childCount > 0) {
                for (int j = 0; j < levels[level].transform.GetChild(i).transform.childCount; j++) {
                    if ((objectLayer & (1 << levels[level].transform.GetChild(i).transform.GetChild(j).gameObject.layer)) != 0) {
                        temp.Add(levels[level].transform.GetChild(i).transform.GetChild(j).gameObject);
                    }
                }
            }
        }
        return temp;
    }

    private void AddTags() {
        for (int i = 0; i < objects.Count; i++) {
            if (objects[i].name == objectName) {
                objects[i].tag = attributeName;
            }
        }
    }

    public void RemoveTags(string blockName, string blockTag) {
        for (int i = 0; i < objects.Count; i++) {
            if (objects[i].name == blockName) {
                objects[i].tag = "Untagged";
            }
        }
    }
}
