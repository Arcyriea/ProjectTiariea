using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public interface EffectsManager { 
    protected class StatusEffect
    {
        public Enums.StatusEffectType type;
        public int stackCount;
        public int maxStack;
        public float duration;

        public StatusEffect(Enums.StatusEffectType type, int stackCount, float duration)
        {
            this.type = type;
            this.stackCount = stackCount;
            this.duration = duration;
        }
    }
}
