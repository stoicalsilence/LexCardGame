using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HiddenText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
