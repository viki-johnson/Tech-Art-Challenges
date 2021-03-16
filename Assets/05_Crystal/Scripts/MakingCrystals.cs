using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fiveCrystal {

    public class MakingCrystals : MonoBehaviour
    {
    public GameObject crystal;
    public int along, down;
    public float width, depth;
        void Start()
        {
            for(int x=0; x<along; x++)
            {
                for(int z=0; z<down; z++)
                {
                    float xx = x*width;
                    if(z%2 == 0)
                    {
                        xx += width/2;
                    }
                    Vector3 pos = new Vector3(xx, 0, z*depth);
                    GameObject c = Instantiate(crystal, pos, Quaternion.identity); 
                }
            }
        }
    }
}
