using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickyTrigger : MonoBehaviour
{
    //Start is called before the first frame update
    public static int dataTricky;
    public BoxCollider2D coll;

    void Start()
    {
        UnityEngine.Debug.Log("trigger masuk");
    }

    // Update is called once per frame
    void Update()
    {
        if (dataTricky == 1)
        {
            coll.isTrigger = true;
            coll.enabled = true;
            UnityEngine.Debug.Log("trigger true");
            //Invoke("dataBack", 2);
        }else if(dataTricky == 0)
        {
            coll.enabled = false;
            coll.isTrigger = false;
        }
    }

    public void dataBack()
    {
        coll.enabled = false;
        coll.isTrigger = false;
    }

    public void dataTrick()
    {
        dataTricky = 0;
    }
}
