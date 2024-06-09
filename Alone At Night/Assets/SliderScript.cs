using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderScript : MonoBehaviour
{
    public TMP_Text text;
    public Image fill;
    void Start() {
        text.text = GetComponent<Slider>().value + "/" + GetComponent<Slider>().maxValue;
    }
    public void ChangeColor() {
        text.text = GetComponent<Slider>().value + "/" + GetComponent<Slider>().maxValue;
        if(GetComponent<Slider>().value <= 40) {
            fill.color = Color.yellow;
        }
        if(GetComponent<Slider>().value <= 20) {
            fill.color = Color.red;
        }
        if(GetComponent<Slider>().value >= 41) {
            fill.color = Color.green;
        }
    }
}
