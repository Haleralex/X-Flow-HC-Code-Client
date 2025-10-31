using UnityEngine;

namespace XFlow.Core
{
    public interface IGameAction
    {
        bool CanApply();
        void Apply();
    }
}
