using System.Collections.Generic;
using UnityEngine;

namespace LFG.GlobalModifier
{
    public abstract class GlobalModifier : ScriptableObject
    {
        [field: SerializeField] public List<TargetCategory> TargetCategories { get; private set; }
        [field: SerializeField] public bool Stackable { get; private set; }
        [field: SerializeField] public bool localOnly { get; private set; }
        [field: SerializeField] public bool linkedToPlayer { get; private set; }
        
        [field: SerializeField]
        private string _description;
        public string Description 
        { 
            get 
            { 
                if (string.IsNullOrEmpty(_description))
                {
                    return GetDescription();
                }
                
                return _description;
            } 
            private set { _description = value; }
        }

        public abstract void Apply(GameObject appliedObject);
        public abstract void Remove(GameObject appliedObject);

        protected virtual string GetDescription()
        {
            return "";
        }
    }
}