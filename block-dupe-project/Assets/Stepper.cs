using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[ExecuteInEditMode]
public class Stepper : MonoBehaviour
{
    public bool interactable;
    public int optionSelected;
    public string[] options;
    public UnityEvent<int> OnValueChanged;
    public Button leftButton;
    public Button rightButton;
    public TMPro.TMP_Text labelText;
    public void Awake()
    {
        leftButton.onClick.AddListener(OnLeftPressed);
        rightButton.onClick.AddListener(OnRightPressed);
    }
    public void Update()
    {
        if(options.GetUpperBound(0) < 0) //invalid state
        {
            leftButton.interactable = rightButton.interactable = interactable = false;
            optionSelected = 0;
            labelText.text = "";
            return;
        }
        leftButton.interactable = rightButton.interactable = interactable;
        optionSelected = Mathf.Clamp(optionSelected, 0, options.GetUpperBound(0));
        labelText.text = options[optionSelected];
        
    }
    public void OnLeftPressed()
    {
        optionSelected -= 1;
        if(optionSelected <= -1) 
            optionSelected = options.GetUpperBound(0);
        OnValueChanged.Invoke(optionSelected);
    }
    public void OnRightPressed()
    {
        optionSelected += 1;
        optionSelected %= options.Length; //wrap between 0 and options.Length-1
        OnValueChanged.Invoke(optionSelected);
    }
}
