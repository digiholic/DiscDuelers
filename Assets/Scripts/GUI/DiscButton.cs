using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscButton : MonoBehaviour {
    private Text buttonText;
    private Image buttonImage;
    private Button button;

	// Use this for initialization
	void Start () {
        buttonText = GetComponentInChildren<Text>();
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeText(string s)
    {
        buttonText.text = s;
    }

    public void SetCancel()
    {
        buttonImage.color = Color.gray;
        ChangeText("x");
    }
}
