using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GunScript[] scrs;
    public GameObject[] model;
    public AudioClip[] clip;
    public AudioSource audiosrc;
    // Start is called before the first frame update
    void Start()
    {
        scrs[0].enabled = true;
        model[0].SetActive(true);
        audiosrc.clip = clip[0];
    }
    public void SwitchGuns(int gun) {
        for(int i = 0; i < scrs.Length; i++) {
            scrs[i].enabled = false;
            model[i].SetActive(false);
            audiosrc.clip = clip[i];
        }        
        scrs[gun].enabled = true;
        model[gun].SetActive(true);  
        audiosrc.clip = clip[gun];
    }
    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            SwitchGuns(3);
        }
    }
}
