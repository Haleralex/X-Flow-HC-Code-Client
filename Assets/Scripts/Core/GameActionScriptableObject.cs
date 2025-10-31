using UnityEngine;

namespace XFlow.Core
{
    public abstract class GameActionScriptableObject : ScriptableObject, IGameAction
    {
        public abstract bool CanApply();
        public abstract void Apply();
    }
}
