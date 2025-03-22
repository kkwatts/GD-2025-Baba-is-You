using UnityEngine;

public class ConnectorBehavior : MonoBehaviour {
    public bool isHorizontalRule, isVerticalRule;

    public GameObject[,] rules;

    private GameObject gameManager;
    private LayerMask ruleLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        isHorizontalRule = false;
        isVerticalRule = false;

        rules = new GameObject[2, 2];

        gameManager = GameObject.FindGameObjectWithTag("GameController");
        ruleLayer = LayerMask.GetMask("Rules");
    }

    // Update is called once per frame
    void Update() {
        rules = GetRules();
    }

    private GameObject[,] GetRules() {
        GameObject[,] temp = new GameObject[2, 2];

        if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit leftHit, 0.9f, ruleLayer)) {
            if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit rightHit, 0.9f, ruleLayer)) {
                temp[0, 0] = leftHit.transform.gameObject;
                temp[0, 1] = rightHit.transform.gameObject;
                isHorizontalRule = true;
            }
            else {
                temp[0, 0] = null;
                temp[0, 1] = null;
                isHorizontalRule = false;
            }
        }
        else {
            temp[0, 0] = null;
            temp[0, 1] = null;
            isHorizontalRule = false;
        }

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit upHit, 0.9f, ruleLayer)) {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit downHit, 0.9f, ruleLayer)) {
                temp[1, 0] = upHit.transform.gameObject;
                temp[1, 1] = downHit.transform.gameObject;
                isVerticalRule = true;
            }
            else {
                temp[1, 0] = null;
                temp[1, 1] = null;
                isVerticalRule = false;
            }
        }
        else {
            temp[1, 0] = null;
            temp[1, 1] = null;
            isVerticalRule = false;
        }

        return temp;
    }
}
