using UnityEngine;

public class InstantiateOnce : MonoBehaviour
{
    //public/inspector
    [SerializeField] public GameObject prefabToInstantiate;
    [SerializeField] public string tagName;

    //unity methods
    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);

        if (objects.Length == 0)
        {
            Instantiate(prefabToInstantiate, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
