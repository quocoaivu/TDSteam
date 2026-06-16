using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickyRoad : MonoBehaviour
{
    // Start is called before the first frame update
    public static int dataRoad;
    public PolygonCollider2D coll;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dataRoad == 1)
        {
            coll.isTrigger = false;
            coll.enabled = false;
            //Invoke("dataBack", 2);
        }
        else if (dataRoad == 0)
        {
            coll.enabled = true;
            coll.isTrigger = true;
        }
    }
}
