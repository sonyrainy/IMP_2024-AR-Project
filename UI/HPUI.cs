using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public Slider HpSlider;

    public int Max_HP = 60;
    int hp = 0;
    // Start is called before the first frame update
    public void Damage()
    {
        //hp -= ;
        HpSlider.value = hp;
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}
