using UnityEngine;

namespace GAG.ObjectPooling
{
    public class ObjectPoolObject : MonoBehaviour
    {
        public int Speed;

        // Update is called once per frame
        void Update()
        {
            Move();
            SetEnqueue();
        }

        void Move()
        {
            gameObject.transform.Translate(0, Speed * Time.deltaTime, 0);
        }

        void SetEnqueue()
        {
            if (gameObject.transform.position.y >= 2.25f)
            {
                ObjectPoolEvents.OnObjectArrived?.Invoke(gameObject);
            }
        }
    }
}
