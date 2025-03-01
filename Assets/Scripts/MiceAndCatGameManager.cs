using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class MiceAndCatGameManager : MonoBehaviour
{
    // Singleton instance
    public static MiceAndCatGameManager Instance { get; private set; }
    public Transform groundPlane;

    public GameObject MicePrefab;
    public GameObject CatPrefab;
    public int initialMiceCount = 20;
    public int initialCatCount = 3;

    // Chase parameters
    public float catMaxSight;
    public float catMaxAngle; // degrees
    public float catMaxChaseTime; // seconds

    [Header("Chase Parameters")]
    public AnimationCurve distanceCurve;
    public AnimationCurve angleCurve;
    public AnimationCurve chaseContinuityCurve;

    [Header("Flee Parameters")]
    public AnimationCurve fleeDistanceCurve;

    public List<GameObject> miceList = new List<GameObject>();
    public List<GameObject> catList = new List<GameObject>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SpawnInitialPopulation();
    }

    void Update()
    {

    }

    public void SpawnInitialPopulation()
    {
        SpawnObjects(MicePrefab, initialMiceCount);
        SpawnObjects(CatPrefab, initialCatCount);
    }

    void SpawnObjects(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Bounds groundBounds = groundPlane.GetComponent<Renderer>().bounds;

            float randomX = Random.Range(groundBounds.min.x, groundBounds.max.x);
            float randomZ = Random.Range(groundBounds.min.z, groundBounds.max.z);

            Vector3 spawnPosition = new Vector3(randomX, groundBounds.max.y + prefab.transform.localScale.y / 2, randomZ);

            GameObject newObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
            if (prefab.name == "Mice")
            {
                miceList.Add(newObject);
            }
            else if (prefab.name == "Cat")
            {
                catList.Add(newObject);
            }
        }
    }
}


