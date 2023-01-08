using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public float speed;
    public bool left;
    public bool started;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(wee());
    }

    // Update is called once per frame
    void Update()
    {
        if (left)
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
        else if(started)
        {
            transform.Translate(Vector3.back * (speed * Time.deltaTime));
        }
        
    }

    public IEnumerator wee()
    {
        while (true)
        {
            started = true;
            left = true;
            yield return new WaitForSeconds(0.1f);
            left = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
