using UnityEngine;
using System.Collections;

public class LevelSelectScript : MonoBehaviour {
    private GameObject manager;
    private GameObject soundFX;

    private bool canSelect;
    private int buttonNum;

    public GameObject[] buttons;
    public float offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        manager = GameObject.FindGameObjectWithTag("GameController");
        soundFX = GameObject.FindGameObjectWithTag("Sound FX");

        if (gameObject.name == "Selector") {
            buttonNum = 1;
            canSelect = true;
            transform.position = new Vector3(transform.position.x, buttons[0].transform.position.y + offset, transform.position.z);
        }
    }

    public void LoadLevelSelect() {
        if (gameObject.name == "Selector") {
            buttonNum = 1;
            canSelect = true;
            transform.position = new Vector3(transform.position.x, buttons[0].transform.position.y + offset, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update() {
        if (gameObject.name == "Selector") {
            if (buttonNum == 1) {
                transform.position = new Vector3(transform.position.x, buttons[0].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    manager.GetComponent<GameManager>().GoToLevel(0);
                    soundFX.GetComponent<AudioScript>().PlaySound("Goal");
                }
            }
            else if (buttonNum == 2) {
                transform.position = new Vector3(transform.position.x, buttons[1].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    manager.GetComponent<GameManager>().GoToLevel(1);
                    soundFX.GetComponent<AudioScript>().PlaySound("Goal");
                }
            }
            else if (buttonNum == 3) {
                transform.position = new Vector3(transform.position.x, buttons[2].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    manager.GetComponent<GameManager>().GoToLevel(2);
                    soundFX.GetComponent<AudioScript>().PlaySound("Goal");
                }
            }
            else if (buttonNum == 4) {
                transform.position = new Vector3(transform.position.x, buttons[3].transform.position.y + offset, transform.position.z);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                    canSelect = false;
                    manager.GetComponent<GameManager>().GoToMenu();
                    soundFX.GetComponent<AudioScript>().PlaySound("Goal");
                }
            }

            if (buttonNum > 1 && canSelect && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))) {
                buttonNum--;
                soundFX.GetComponent<AudioScript>().PlaySound("Step");
            }
            else if (buttonNum < 4 && canSelect && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))) {
                buttonNum++;
                soundFX.GetComponent<AudioScript>().PlaySound("Step");
            }
        }
    }
}
