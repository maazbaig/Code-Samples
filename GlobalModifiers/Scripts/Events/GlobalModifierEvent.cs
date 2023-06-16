using System.Collections.Generic;
using Aether;

namespace LFG.GlobalModifier
{
    public class GlobalModifierEvent : Event<GlobalModifierEvent>
    {
        public readonly List<GlobalModifierSetInstance> ModifierSets;

        public GlobalModifierEvent(List<GlobalModifierSetInstance> modifierSets)
        {
            ModifierSets = modifierSets;
        }
    }

    public enum EventType
    {
        Activate,
        Deactivate
    }
}