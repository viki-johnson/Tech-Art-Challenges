using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    
    public GameObject objectToFollow;
    public float zOffset;
    
    public float speed = 2.0f;
    
    void Update () {
        float interpolation = speed * Time.deltaTime;
        
        Vector3 position = this.transform.position;
        position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);
        position.z = Mathf.Lerp(this.transform.position.z, objectToFollow.transform.position.z+zOffset, interpolation);
        
        this.transform.position = position;
    }
}