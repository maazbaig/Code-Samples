using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LFG.GlobalModifier
{
    [CreateAssetMenu(fileName = "GlobalModifierSet", menuName = "LFG/Global Modifier/Modifier Set")]
    public class GlobalModifierSet : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public List<GlobalModifierLevel> Levels { get; private set; }

        public GlobalModifier GetLevelModifier(int level)
        {
            var gmLevel = Levels.Find(l => l.Level == level);
            return gmLevel?.GlobalModifier;
        }
    }
    
    [Serializable]
    public class GlobalModifierLevel
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public GlobalModifier GlobalModifier { get; private set; }
    }
    
    [Serializable]
    public class GlobalModifierSetInstance
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public int PlayerID { get; private set; }
        [field: SerializeField] public List<GlobalModifierLevelInstance> Levels { get; private set; }
        [field: SerializeField] public GlobalModifier ActiveGlobalModifier { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }



        public static GlobalModifierSetInstance CreateInstance(GlobalModifierSet modifierSet, int playerId,  int level = 1)
        {
            GlobalModifierSetInstance instance = new GlobalModifierSetInstance
            {
                ID = modifierSet.ID,
                PlayerID = playerId,
                Levels = new List<GlobalModifierLevelInstance>()
            };
            
            foreach (GlobalModifierLevel globalModifierLevel in modifierSet.Levels)
            {
                instance.Levels.Add(new GlobalModifierLevelInstance(
                    globalModifierLevel.Level,
                    globalModifierLevel.GlobalModifier));
            }
            
            instance.SetActiveLevel(level);

            return instance;
        }

        public static GlobalModifierSetInstance CreateCopy(GlobalModifierSetInstance instance)
        {
            GlobalModifierSetInstance copiedInstance = new GlobalModifierSetInstance
            {
                ID = instance.ID,
                Levels = new List<GlobalModifierLevelInstance>()
            };

            foreach (GlobalModifierLevelInstance level in instance.Levels)
            {
                copiedInstance.Levels.Add(new GlobalModifierLevelInstance(level.Level, level.GlobalModifier));
            }
            
            copiedInstance.SetActiveLevel(instance.GetActiveLevel().Level);

            return copiedInstance;
        }

        public GlobalModifierLevelInstance GetActiveLevel()
        {
            return Levels.FirstOrDefault(level => level.Active);
        }

        public void SetActiveLevel(int activeLevel)
        {
            int topLevel = Levels.Max(level => level.Level);
            foreach (GlobalModifierLevelInstance modifierLevel in Levels)
            {
                modifierLevel.Active = Mathf.Clamp(activeLevel, 1, topLevel) == modifierLevel.Level; // TODO: Remove clamp when we have more levels
            }

            ActiveGlobalModifier = GetActiveLevel().GlobalModifier;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            GlobalModifierSetInstance comparingInstance = (GlobalModifierSetInstance)obj;
            return ID == comparingInstance.ID;
        }
    }
    
            
    [Serializable]
    public class GlobalModifierLevelInstance
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public GlobalModifier GlobalModifier { get; private set; }
        [field: SerializeField] public bool Active { get; set; }
        public GlobalModifierLevelInstance(int level, GlobalModifier globalModifier)
        {
            Level = level;
            GlobalModifier = globalModifier;
        }
    }

 
}