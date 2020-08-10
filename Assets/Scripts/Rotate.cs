using System.Collections;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // public GameObject booster;
    // public int speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 170f) * Time.deltaTime);
    }
}
