using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[ExecuteInEditMode]
public class ObstacleSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public int numberOfObjects = 5;
    private Vector3 spaceSize;
    [SerializeField] private int _length = 40;
    public bool Created;
    private Vector3[] positions;

    //when you turn on and turn on this object, the obstacles change, 
    //if you click on "Created" it will stop changing
    void OnEnable()
    {
        spaceSize = new Vector3(_length, 0.5f, 4);
        if (!Created)
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
                positions = null;
            }

            SpawnObjects();
        }
    }

    void SpawnObjects()
    {
        positions = GenerateNonOverlappingPositions(numberOfObjects, spaceSize);

        foreach (Vector3 position in positions)
        {
            Instantiate(GetRandomScale(objectToSpawn), position, Quaternion.identity, transform);
        }
    }

    private Vector3[] GenerateNonOverlappingPositions(int count, Vector3 areaSize)
    {
        Vector3[] positions = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            Vector3 newPosition = Vector3.zero;
            bool positionFound = false;

            while (!positionFound)
            {
                positionFound = true;
                newPosition = new Vector3(
                    Random.Range(-areaSize.x / 2, areaSize.x / 2),
                    Random.Range(0.66f, areaSize.y) - 1,
                    Random.Range(-areaSize.z / 2, areaSize.z / 2)
                );

                for (int j = 0; j < i; j++)
                {
                    if (Vector3.Distance(newPosition, positions[j]) < 0.2f /* objectToSpawn.transform.localScale.x*/)
                    {
                        positionFound = false;
                        break;
                    }
                }
            }

            positions[i] = newPosition;
        }

        return positions;
    }

    private GameObject GetRandomScale(GameObject newObject)
    {
        newObject.transform.localScale =
            new Vector3(Random.Range(0.2f, 1f), 1f, Random.Range(0.2f, 1f));
        return newObject;
    }
}