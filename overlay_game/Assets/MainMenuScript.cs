using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Slider nrOfRectSlider;
    public TextMeshProUGUI nrOfRectText;
    public TextMeshProUGUI rectLeftText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI resultText;
    public Button startButton;
    public GameObject centerPanel;
    public GameObject endPanel;
    public GameObject buttonPrefab ;
    public static float nrOfRect = 5.0f;
    private static float rectLeft = nrOfRect;
    private bool gameStarted = false;
    private bool gameEnded = false;
    private bool panelClear = false;
    private static float timer = 0.0f;
    private Button[] buttons;


    void Start(){
        nrOfRectSlider.value = nrOfRect;
        ShowRectLeft();
        ShowSliderValue();
    }


    void Update(){
        if (rectLeft == 0.0f ) gameEnded = true;
        if (panelClear) endPanel.SetActive(true);
        if (gameStarted) {
            if (!gameEnded) timer += Time.deltaTime;
            else ShowResult();
        }
        ShowTimeValue();
    }

    public void ShowSliderValue(){
        string message = "number of rectangles: " + nrOfRectSlider.value;
        nrOfRectText.text = message;
    }

    public void OnSliderValueChanged(float value) {
        nrOfRect = value;
        ShowSliderValue();
        if (!gameStarted) rectLeft = value;
        ShowRectLeft();
    }

    public void ShowTimeValue(){
        string message = "time: " + FormatTime(timer);
        timeText.text = message;
    }

    public void ShowRectLeft(){
        string message = "rectangles left: " + rectLeft;
        rectLeftText.text = message;
    }

    public void ShowResult(){
        string message = "result: " + FormatTime(timer);
        resultText.text = message;
    }

    public void onButtonStartClicked(){
        gameStarted = !gameStarted;
        centerPanel.SetActive(false);
        buttons = new Button[(int)nrOfRect];
        for (int i=0; i<nrOfRect; i++){
            buttons[i] = MakeButton();
        }
    }

    public void onButtonGameClicked(GameObject button){
        StartCoroutine( ChangeColor(button));
    }

    public void onButtonRestartClicked(){
        timer = 0.0f;
        gameStarted = false;
        gameEnded = false;
        panelClear = false;
        rectLeft = nrOfRect;
        SceneManager.LoadScene("GameScene");
    }

    private string FormatTime( float time ) {
         int minutes = (int) time / 60 ;
         int seconds = (int) time - 60 * minutes;
         int milliseconds = (int) (1000 * (time - minutes * 60 - seconds));
         return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds );
    }

    public Button MakeButton() {  
        GameObject button = (GameObject) Instantiate( buttonPrefab ) ;
        Canvas canvas = FindObjectOfType<Canvas>();
        float h = canvas.GetComponent<RectTransform>().rect.height;
        float w = canvas.GetComponent<RectTransform>().rect.width;
        float width = Random.Range(40.0f, w/4.0f);
        float height = Random.Range(40.0f, h/2.0f);
        var panel = GameObject.Find("Panel Game Area");
        button.GetComponent<RectTransform>().SetParent(panel.transform);
        button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Random.Range(0.0f, w-width), width);
        button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, Random.Range(0.0f, h*0.9f-height), height); 
        button.GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value);
		button.GetComponent<Button>().onClick.AddListener(delegate { onButtonGameClicked(button); });
        return button.GetComponent<Button>();
    }

    IEnumerator ChangeColor(GameObject button)  {
        rectLeft -=1;
        ShowRectLeft();
        button.GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value);
        yield return new WaitForSeconds(.3f);
        button.SetActive(false);
        if (gameEnded) panelClear = true;
    }
}
