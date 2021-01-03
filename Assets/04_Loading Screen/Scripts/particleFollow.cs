using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleFollow : MonoBehaviour
{
    public float lerp, speed;
    public GameObject particles;
    public genBase generator;
    public int currentPoint;
    Vector3 start, end;

    public float y;
    private void Start() {
        currentPoint = 0;
        StartCoroutine(SlightDelay());
    }

    IEnumerator SlightDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MoveTo());
    }
    IEnumerator MoveTo()
    {
        int p = currentPoint%generator.points.Count;

        lerp = 0;

        start = new Vector3(generator.points[p].transform.position.x, y, generator.points[p].transform.position.z);

        if (p < generator.points.Count-1){
            end = new Vector3(generator.points[p+1].transform.position.x, y, generator.points[p+1].transform.position.z);
        } else{
            end = new Vector3(generator.points[0].transform.position.x, y, generator.points[0].transform.position.z);
        }

        while(Vector3.Distance(particles.transform.position, end) > y)
        {
            lerp += Time.deltaTime * speed;
            particles.transform.position = Vector3.Lerp(start, end, lerp);
            yield return null;
        }
        AddOne();
    }

    public void AddOne()
    {
        currentPoint++;
        StartCoroutine(MoveTo());
    }
}
