using System.Collections.Generic;
using Blocks;
using UnityEngine;

namespace UI.blocksUi
{
    
    [System.Serializable]
    public class BlockPrefabConfig
    {
        public GameObject prefab;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale = Vector3.one;
    }
    
    public class BlocksUiGenerator : MonoBehaviour
    {

        [Header("Bloques a mostrar (los mismos que usa BlockSpawner)")]
        [SerializeField] public List<BlockPrefabConfig> blocks = new List<BlockPrefabConfig>();



        //[Header("Parámetros de visualización")] [SerializeField]
        //private Vector3 rotationEuler = new Vector3(10f, 130f, 45f);
//
        //[SerializeField] private Vector3 normalScale = Vector3.one;

        private readonly List<GameObject> _spawnedBlocks = new List<GameObject>();
        private int _currentSelected = 0;

        void Start()
        {
            //if (blockSpawner == null)
            //    return;
//
            //_blockPrefabs = blockSpawner.blockPrefabs;

            SpawnAllBlocks();
            HighlightSelected(_currentSelected);
        }

        private void SpawnAllBlocks()
        {
            // Instanciar todos los bloques en la misma posición (0,0,0)
            foreach (var block in blocks)
            {
                Quaternion rot = Quaternion.Euler(block.rotation);
                GameObject instance = Instantiate(block.prefab, transform);
                instance.transform.localPosition = block.position;
                instance.transform.localRotation = rot;
                instance.transform.localScale = block.scale;
                
                _spawnedBlocks.Add(instance);
            }
        }

        /// <summary>
        /// Muestra solo el bloque seleccionado, ocultando los demás.
        /// </summary>
        private void UpdateBlockVisibility()
        {
            if (_spawnedBlocks.Count == 0)
                return;

            for (int i = 0; i < _spawnedBlocks.Count; i++)
            {
                bool isSelected = (i == _currentSelected);
                _spawnedBlocks[i].SetActive(isSelected);
            }
        }

        public void HighlightSelected(int newIndex)
        {
            if (_spawnedBlocks.Count == 0)
                return;

            newIndex = Mathf.Clamp(newIndex, 0, _spawnedBlocks.Count - 1);
            _currentSelected = newIndex;

            // Actualiza visibilidad
            UpdateBlockVisibility();
        }

        public void SetSelectedFromSpawner(int index)
        {
            HighlightSelected(index);
        }
    }
}
