using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAG.ObjectPooling
{
    public class GameManager : MonoBehaviour
    {
        #region Objects pooling

        [SerializeField] int _objectPoolCount = 5;

        [SerializeField] Transform _objectParent;
        [SerializeField] List<GameObject> _objectPrefabs;

        Dictionary<string, Queue<GameObject>> _objectPools = new Dictionary<string, Queue<GameObject>>();

        #endregion

        [Range(1, 5)][SerializeField] int _speed;
        [Range(0.1f, 2)][SerializeField] float _spawnInterval = 1;
        [SerializeField] bool _isStartGame = true;

        float _currentTime = 0;

        void OnEnable()
        {
            Events.OnObjectArrived += DespawanObject;
        }

        void Start()
        {
            foreach (GameObject objectPrefab in _objectPrefabs)
            {
                _objectPools[objectPrefab.tag] = new Queue<GameObject>();
            }

            InstantiateObject();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) && !_isStartGame)
            {
                _currentTime = 0;
                _isStartGame = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && _isStartGame)
            {
                _isStartGame = false;
            }

            SpawnObjects();
        }

        void InstantiateObject()
        {
            for (int i = 0; i < _objectPoolCount; i++)
            {
                foreach (GameObject objectPrefab in _objectPrefabs)
                {
                    GameObject createdObject = Instantiate(objectPrefab, _objectParent);
                    objectPrefab.SetActive(false);

                    EnqueueObjectPool(createdObject);
                }
            }
        }

        void SpawnObjects()
        {
            if (_isStartGame)
            {
                _currentTime += Time.deltaTime;

                if (_spawnInterval <= _currentTime)
                {
                    SpawnObject();
                    _currentTime = 0;
                }
            }
        }

        void SpawnObject()
        {
            int selectedObject = Random.Range(0, _objectPrefabs.Count);
            string objectKey = _objectPrefabs[selectedObject].tag;

            GameObject newObject = DequeueObjectPool(objectKey);

            if (newObject != null)
            {
                newObject.transform.position = _objectPrefabs[selectedObject].transform.position;
                newObject.gameObject.GetComponent<Obj>().Speed = _speed;
                newObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No object available in the queue for selectedObject: " + selectedObject);
            }
        }

        void DespawanObject(GameObject go)
        {
            if (go == null)
            {
                Debug.LogWarning("Attempted to despawn a null go.");
                return;
            }

            go.SetActive(false);
            EnqueueObjectPool(go);
        }

        void EnqueueObjectPool(GameObject go)
        {
            // Check if the object has a tag and if it exists in the dictionary
            if (go != null && _objectPools.TryGetValue(go.tag, out Queue<GameObject> objectQueue))
            {
                objectQueue.Enqueue(go);
            }
            else
            {
                Debug.LogWarning($"No pool found for object with tag: {go.tag}");
            }
        }

        GameObject DequeueObjectPool(string objectKey)
        {
            if (_objectPools.TryGetValue(objectKey, out Queue<GameObject> objectQueue) && objectQueue.Count > 0)
            {
                return objectQueue.Dequeue();
            }
            else
            {
                Debug.LogWarning($"No object available in the pool for id: {objectKey}");
                return null;
            }
        }
    }
}