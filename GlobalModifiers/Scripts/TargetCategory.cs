using UnityEngine;

namespace LFG.GlobalModifier
{
    [CreateAssetMenu(fileName = "ModifierCategory", menuName = "LFG/Global Modifier/Category")]
    public class TargetCategory : ScriptableObject
    {
        [field: SerializeField] public string TypeID { get; private set; }
    }
}