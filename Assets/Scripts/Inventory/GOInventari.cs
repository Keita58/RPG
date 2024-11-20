using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GOInventari", menuName = "Scriptable Objects/GOInventari")]
public class GOInventari : ScriptableObject
{
    [SerializeField] GridLayoutGroup Inventari;
}
