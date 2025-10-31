using System;
using System.Collections.Generic;
using UnityEngine;

namespace XFlow.Core
{
    public class PlayerData
    {
        private static PlayerData _instance;
        public static PlayerData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerData();
                }
                return _instance;
            }
        }

        private Dictionary<Type, object> _data = new Dictionary<Type, object>();
        private Dictionary<Type, Action> _changeCallbacks = new Dictionary<Type, Action>();
        public event Action OnAnyChanged;
        private PlayerData() { }

        public T Get<T>() where T : new()
        {
            Type type = typeof(T);
            if (!_data.ContainsKey(type))
            {
                _data[type] = new T();
            }
            return (T)_data[type];
        }

        public void Set<T>(T value)
        {
            Type type = typeof(T);
            _data[type] = value;
            NotifyChange<T>();
        }

        public void Subscribe<T>(Action callback)
        {
            Type type = typeof(T);
            if (!_changeCallbacks.ContainsKey(type))
            {
                _changeCallbacks[type] = callback;
            }
            else
            {
                _changeCallbacks[type] += callback;
            }
        }

        public void Unsubscribe<T>(Action callback)
        {
            Type type = typeof(T);
            if (_changeCallbacks.ContainsKey(type))
            {
                _changeCallbacks[type] -= callback;
            }
        }

        public void NotifyChange<T>()
        {
            Type type = typeof(T);
            if (_changeCallbacks.ContainsKey(type))
            {
                _changeCallbacks[type]?.Invoke();
            }
            OnAnyChanged?.Invoke();
        }
    }
}
