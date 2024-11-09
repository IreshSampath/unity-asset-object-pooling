using System;
using UnityEngine;

namespace GAG.ObjectPooling
{
    public class ObjectPoolEvents
    {
        public static Action OnObjectSpawned;
        public static Action<GameObject> OnObjectArrived;
    }
}