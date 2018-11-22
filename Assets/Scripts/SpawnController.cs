using UnityEngine;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{
    private float sphereTimeElapsed;
    private float colorZoneTimeElapsed;
    private bool spawnPowerup = true;
    private float spawnZ;
    private readonly float safeZone = 5.0f;
    private readonly float tileLength = 10.0f;
    private readonly float noOfTilesOnScreen = 15;
    private PlayerController playerController;
    private GameObject currentRoad;
    public GameObject colorChangingPrefab;
    public GameObject roadPrefab;
    public List<GameObject> spherePrefabs;
    public List<GameObject> instantiatedRoads;
    public List<GameObject> instantiatedObjects;
    public GameObject player;
    public float sphereCycle;
    public float colorChangeCycle;

    private void Start()
    {
        sphereCycle = 2f;
        colorChangeCycle = 15f;
        playerController = player.GetComponent<PlayerController>();
        instantiatedObjects = new List<GameObject>();
        instantiatedRoads = new List<GameObject>();

        for (int i = 0; i < noOfTilesOnScreen; i++)
        {
            SpawnRoad();
        }
    }

    private void Update()
    {
        if (!(GameController.Instance.isPaused || GameController.Instance.isGameOver))
        {
            if (player.transform.position.z > (spawnZ - noOfTilesOnScreen * tileLength))
            {
                SpawnRoad();
            }
            sphereTimeElapsed += Time.deltaTime;
            colorZoneTimeElapsed += Time.deltaTime;
            if (colorZoneTimeElapsed > colorChangeCycle)
            {
                SpawnColorChangingZone();
                colorZoneTimeElapsed -= colorChangeCycle;
                sphereTimeElapsed -= sphereCycle;
            }
            else if (sphereTimeElapsed > sphereCycle)
            {
                SpawnSphere();
                sphereTimeElapsed -= sphereCycle;
                spawnPowerup = !spawnPowerup;
            }
            CleanMemory();
        }
    }

    private void SpawnRoad()
    {
        currentRoad = Instantiate(roadPrefab);
        currentRoad.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        instantiatedRoads.Add(currentRoad);
    }

    private void SpawnColorChangingZone()
    {
        GameObject area = Instantiate(colorChangingPrefab);
        area.transform.position = new Vector3(area.transform.position.x, area.transform.position.y, player.transform.position.z + tileLength * noOfTilesOnScreen - tileLength);
        int randomInt = Random.Range(0, 3);
        area.GetComponent<Light>().color = randomInt == 0 ? Color.red : randomInt == 1 ? Color.green : Color.blue;
        instantiatedObjects.Add(area);
    }

    private void SpawnSphere()
    {
        GameObject sphere;
        if (spawnPowerup)
        {
            int index = spherePrefabs.FindIndex(current => current.GetComponent<Renderer>().sharedMaterial.color == playerController.currentColor);
            sphere = Instantiate(spherePrefabs[index]);
            Vector3 pos = sphere.transform.position;
            sphere.transform.position = new Vector3(Random.Range(-4, 4), pos.y, Random.Range(0, 4) + player.transform.position.z + tileLength * noOfTilesOnScreen - tileLength);
        }
        else
        {
            List<GameObject> obstacles = new List<GameObject>(spherePrefabs);
            int index = obstacles.FindIndex(current => current.GetComponent<Renderer>().sharedMaterial.color == playerController.currentColor);
            obstacles.RemoveAt(index);
            sphere = Instantiate(obstacles[Random.Range(0, obstacles.Count)]);
            Vector3 pos = sphere.transform.position;
            sphere.transform.position = new Vector3(Random.Range(-4, 4), pos.y, Random.Range(0, 4) + player.transform.position.z + tileLength * noOfTilesOnScreen - tileLength);
        }
        instantiatedObjects.Add(sphere);
    }

    private void CleanMemory()
    {
        if (instantiatedObjects.Count > 0)
        {
            GameObject oldestObject = instantiatedObjects[0];
            if (oldestObject.transform.position.z + safeZone < player.transform.position.z)
            {
                Destroy(oldestObject);
                instantiatedObjects.RemoveAt(0);
            }

        }

        if (instantiatedRoads.Count > 0)
        {
            GameObject oldestRoad = instantiatedRoads[0];
            if (oldestRoad.transform.position.z + tileLength + safeZone < player.transform.position.z)
            {
                Destroy(oldestRoad);
                instantiatedRoads.RemoveAt(0);
            }
        }
    }
}