using System;
using UnityEngine;

namespace GAG.ObjectPooling
{
    public class Events
    {
        public static Action OnObjectSpawned;
        public static Action<GameObject> OnObjectArrived;
    }
}