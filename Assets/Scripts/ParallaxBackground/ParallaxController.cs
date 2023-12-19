using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxController : MonoBehaviour
{

    Transform camera;
    Vector3 cameraStartPosition;
    float distance;

    GameObject[] backgrounds;
    Material[] material;
    float[] backSpeed;
    float farthestBack;

    [Range(0f, 0.5f)]
    public float parallaxSpeed;

    void Start()
    {
        camera = Camera.main.transform;
        cameraStartPosition = camera.position;

        int backCount = transform.childCount;
        material = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            material[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++) //find the farthest background
        {
            if ((backgrounds[i].transform.position.z - camera.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - camera.position.z;
            }
        }

        for (int i = 0; i < backCount; i++) //set the speed of background
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - camera.position.z) / farthestBack;
        }
    }

    private void LateUpdate()
    {
        distance = camera.position.x - cameraStartPosition.x;
        transform.position = new Vector3(camera.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            material[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
