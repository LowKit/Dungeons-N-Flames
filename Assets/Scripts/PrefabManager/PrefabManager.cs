using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; private set;}
    [SerializeField] private List<PrefabList> prefabLists = new List<PrefabList>();

    private void Awake()
    {
        Instance = this;
    }
    public GameObject GetObjectPrefab(string listType, string objectKey)
    {
        List<PrefabReference> list = GetReferenceList(listType);

        if (list == null)
        {
            Debug.LogError("[PrefabManager] Prefab list not found!");
            return null;
        }

        GameObject prefab = list.FirstOrDefault(i => i.referenceKey == objectKey).prefab;

        if (prefab == null)
        {
            Debug.LogError("[PrefabManager] Prefab not found!");
            return null;
        }

        return prefab;
    }
    public GameObject[] GetRandomPrefabs(string listType, int count)
    {
        List<PrefabReference> list = GetReferenceList(listType);
        List<GameObject> prefabList = new();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            prefabList.Add(list[randomIndex].prefab);
        }

        return prefabList.ToArray();
    }

    private List<PrefabReference> GetReferenceList(string listType)
    {
        return prefabLists.FirstOrDefault(i => i.type == listType).references.prefabReferences;
    }
}

