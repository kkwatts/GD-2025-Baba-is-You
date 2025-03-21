using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    private LayerMask objectLayer;

    private int level = 0;
    private string levelName;

    public GameObject[] VFX;
    public GameObject[] levels;

    private GameObject[] connectors;
    private GameObject[] objects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        objectLayer = LayerMask.GetMask("Objects");

        connectors = GetConnectors(level).ToArray();
        objects = GetObjects(level).ToArray();
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < connectors.Count; i++) {
            //if (connectors[i].GetComponent<BlockBehavior>().isConnected) {
            //Debug.Log(connectors[i].GetComponent<ConnectorBehavior>().objectBlock.tag);
            //}
            Debug.Log(connectors[i]);
        }
    }

    private ArrayList GetConnectors(int level) {
        ArrayList temp = new ArrayList();
        for (int i = 0; i < levels[level].transform.childCount; i++) {
            if (levels[level].transform.GetChild(i).gameObject.tag == "Connector Block") {
                temp.Add(levels[level].transform.GetChild(i).gameObject);
            }
        }
        return temp;
    }

    private ArrayList GetObjects(int level) {
        ArrayList temp = new ArrayList();
        for (int i = 0; i < levels[level].transform.childCount; i++) { 
            if (levels[level].transform.GetChild(i).gameObject.layer == objectLayer) {
                temp.Add(levels[level].transform.GetChild(i).gameObject);
            }
        }
        return temp;
    }
}
