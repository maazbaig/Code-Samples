using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace LFG.GlobalModifier
{
    public class GlobalModifierManager : Singleton<GlobalModifierManager>
    {
        [field: SerializeField] public List<GlobalModifierSet> GlobalModifierSetCollection { get; private set; }
        [SerializeField] private List<GlobalModifier> _registeredGlobalModifiers;
        
        [field: SerializeField, ReadOnly] public List<GlobalModifier> ActiveModifiers { get; private set; }
        [field: SerializeField, ReadOnly] public List<GlobalModifierSetInstance> ActiveModifierSets { get; private set; }
        [field: SerializeField, ReadOnly] public List<GlobalModifierSetInstance> RuntimeGeneratedSets { get;
            private set;
        }

        #region Public Functions
        
        void Awake()
        {
            RegisterGlobalModifiers();
        }

        void RegisterGlobalModifiers()
        {
            GlobalModifierSetCollection.Select(c => c.Levels)
                //Cycle through each Collection's Levels
                .ForEach(levels => levels.Select(level => level.GlobalModifier)
                    //Cycle through each Level's GM and add if it doesn't already exist
                    .ForEach(gm =>
                    {
                        if (!_registeredGlobalModifiers.Contains(gm))
                            _registeredGlobalModifiers.Add(gm);
                    }));
        }

        public static void AddModifierSets(List<GlobalModifierSetInstance> modifierSets)
        {
            foreach (GlobalModifierSetInstance modifierSet in modifierSets)
            {
                AddModifierSet(modifierSet);
            }

            ResetActiveModifiers();
            
            new GlobalModifierEvent(instance.ActiveModifierSets).Invoke();
        }

        public static void RemoveModifierSets(List<GlobalModifierSetInstance> modifierSets)
        {
            instance.ActiveModifierSets = instance.ActiveModifierSets
                .Where(activeSet => modifierSets.All(modifierSet => modifierSet.ID != activeSet.ID))
                .ToList();
            
            ResetActiveModifiers();
            
            new GlobalModifierEvent(instance.ActiveModifierSets).Invoke();
        }

        public static void RemoveModifiersByPlayerID(int playerId)
        {
            instance.ActiveModifierSets.RemoveAll(set => set.PlayerID == playerId);
        }

        public static void ClearAllModifierSets()
        {
            instance.ActiveModifierSets.Clear();
            ResetActiveModifiers();
        }

        #region Utility
        #endregion
        
        #endregion

        #region Private Functions

        private static void ResetActiveModifiers()
        {
            instance.ActiveModifiers = instance.ActiveModifierSets
                .Select(set => set.ActiveGlobalModifier)
                .Where(modifier => modifier != null)
                .GroupBy(modifier => modifier.name)
                .SelectMany(group => group.First().Stackable ? group : group.Take(1))
                .ToList();
        }

        private static void AddModifierSet(GlobalModifierSetInstance modifierSet)
        {
            GlobalModifier modifier = modifierSet.GetActiveLevel()?.GlobalModifier;

            if (!modifier) return;
            
            instance.ActiveModifierSets.Add(modifierSet);
        }
        
        #endregion
    }
}

