using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.XR.CoreUtils.Collections;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] GameObject moleHolePrefab;
    private List<GameObject> moleHoles = new List<GameObject>(); 

    [SerializeField] readonly int maxMoleHoles = 10; 
    private int moleHoleCount = 0;    

    public float areaSizeX = 0; 
    public float areaSizeZ = 0;
    [SerializeField] Transform rayOrigin; // Transform of the invisible object

    [SerializeField] bool areaDetectionEnabled = false;

   
    void Start()
    {
        
    }


    void Update()
    {
        if (areaDetectionEnabled)
            AreaDetection();    
    }

    private void Init()
    {

    }

    private IEnumerator GenerateGameBoard()
    {
        Debug.Log("Generating game board...");
        areaDetectionEnabled = true;    
        return new WaitUntil(() => moleHoleCount == maxMoleHoles);
        areaDetectionEnabled = false;



    }

    // Checks surface area of game board for empty places to spawn mole hole at.
    private void AreaDetection()
    {
        // Check if yOriginTransform is assigned
        if (rayOrigin == null)
        {
            Debug.LogError("rayOrigin is not assigned.");
            return;
        }

        if (moleHoleCount == maxMoleHoles)
        {
            Debug.Log("moleHoleCount reached maxed count");
            return;
        }

        // Generate random x and z positions within the defined area
        float randomX = Random.Range(rayOrigin.position.x - areaSizeX / 2, rayOrigin.position.x + areaSizeX / 2);
        float randomZ = Random.Range(rayOrigin.position.z - areaSizeZ / 2, rayOrigin.position.z + areaSizeZ / 2);

        // Create the random position vector
        Vector3 randomPosition = new Vector3(randomX, rayOrigin.position.y, randomZ);

        // Shoot a ray from the random position downwards
        Ray ray = new Ray(randomPosition, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Ray hit: " + hit.collider.name + " at position: " + hit.point);
            if (hit.transform.tag == "MoleHole")
            {
                Debug.Log($"hitting moleHole: {hit.transform.name}");
            }

            else if (hit.transform.tag == "GameBoard")
            {
                Debug.Log($"hitting gameboard!");
            }
        }
        else
        {
            Debug.Log("Ray did not hit anything.");
        }

        // Optional: Draw the ray in the scene view for debugging
        Debug.DrawRay(randomPosition, Vector3.down, Color.red, 1f);
    }

    private void SpawnHole(Vector3 targetPos) 
    {
        var moleHole = Instantiate(moleHolePrefab, targetPos, transform.rotation);
        moleHoles.Add(moleHole);    
        moleHoleCount++;
    }

    private void DeleteAllMoleHoles()
    {
        foreach (var moleHole in moleHoles)
            Destroy(moleHole);
        moleHoles.Clear();
        moleHoleCount = 0;  
    }
}
