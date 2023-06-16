using System;
using System.Collections.Generic;
using System.Linq;
using LFG.Network;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LFG.GlobalModifier
{
    public abstract class GlobalModifierApplier : NetworkBehaviour
    {
        [SerializeField] private List<TargetCategory> _targetCategories;
        
        #region State

        [field:SerializeField, ReadOnly] public bool IsInitialized = false;
        [field: SerializeField] public List<GlobalModifierSetInstance> ActiveModifierSets { get; private set; }
        
        [field: SerializeField] public List<GlobalModifierSetInstance> RelevantModifierSets { get; private set; }
        #endregion

        protected Entity.Entity _entity;
        
        public virtual void Awake()
        {
            _entity = GetComponent<Entity.Entity>();
            GlobalModifierEvent.AddListener(OnGlobalModifierEvent);
            Initialize();
        }

        public virtual void Initialize()
        {
            IsInitialized = true;
        }

        protected void ApplyModifiers(List<GlobalModifierSetInstance> modifierSetInstances)
        {
            HandleModifiers(modifierSetInstances);
        }
    
        protected void OnGlobalModifierEvent(GlobalModifierEvent obj)
        {
        }
    
        private void HandleModifiers(List<GlobalModifierSetInstance> modifiers)
        {
            //Create Groups of Instance & targetCategory, and then Call Apply Modifier for each instance
            modifiers
                .SelectMany(instance => instance.GetActiveLevel().GlobalModifier.TargetCategories
                    .Where(targetCategory => _targetCategories.Any(myCategory => targetCategory.TypeID == myCategory.TypeID)), 
                    (instance, targetCategory) => (instance, targetCategory))
                .ToList()
                .ForEach(pair => ApplyModifier(pair.instance));
            
        }

        /// <summary>
        /// Applies the active Modifier from the setInstance
        /// Will compare Stackable and Player Linkage
        /// Stacking is grouped by player if playerLink is set to true on the Modifier
        /// </summary>
        /// <param name="setInstance"></param>
        /// <param name="objPlayerId"></param>
        public void ApplyModifier(GlobalModifierSetInstance setInstance)
        {
            try
            {
                GlobalModifier modifier = setInstance.ActiveGlobalModifier;

                bool localOnly = modifier.localOnly;

                int playerID = Runner && Runner.LocalPlayer ? Runner.LocalPlayer.PlayerId : -1;
                bool isLocalPlayer = setInstance.PlayerID == playerID;
            
                if (localOnly && !isLocalPlayer) return;

                bool isStackable = modifier.Stackable;
                bool isModifierAlreadyActive = ActiveModifierSets.Any(set =>
                    set.ActiveGlobalModifier == modifier &&
                    (localOnly ? set.PlayerID == setInstance.PlayerID : true)
                );

                if (!isStackable && isModifierAlreadyActive) return;

                if (modifier.linkedToPlayer && Object.InputAuthority.PlayerId != setInstance.PlayerID) return;
            
                modifier.Apply(gameObject);
                ActiveModifierSets.Add(setInstance);

                new Event(EventType.Applied, setInstance).Invoke();
            }
            catch (Exception e)
            {
                //Ignore
            }
         
        }
    
        private void RemoveModifier(GlobalModifierSetInstance setInstance)
        {
            if (ActiveModifierSets.Contains(setInstance))
            {
                GlobalModifier modifier = setInstance.ActiveGlobalModifier;
                ActiveModifierSets.Remove(setInstance);
                modifier.Remove(gameObject);
                                
            }
        }
        
        #region Events

        public enum EventType
        {
            Initialized,
            Applied,
            Removed,
        }
        
        public class Event : Aether.Event<Event>
        {
            public readonly EventType Type;

            public GlobalModifierSetInstance Instance;

            public Event(EventType type, GlobalModifierSetInstance instance)
            {
                Type = type;
                Instance = instance;
            }
        }
        #endregion
        
        //TODO: QUESTION: Do we want to remove application of the modifiers if player leaves during match?
        //This would depend on the reward system alteration. What if the player leaves right before the end, do rewards shrink?
        //Leaving their application in for now
        
    }

    public interface IGlobalModifier
    {
        public void Initialized(GlobalModifierApplier applier);

        public void Applied(GlobalModifier mod);

        public void Removed(GlobalModifier mod);
    }

}
