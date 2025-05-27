using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PrefabReferenceList", menuName = "References/PrefabReferenceList", order = 1)]
public class PrefabReferenceList : ScriptableObject
{
    public List<PrefabReference> prefabReferences;
}
