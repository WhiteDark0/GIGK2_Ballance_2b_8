using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Scripts
{
    public class CheckpointController : MonoBehaviour
    {
        public static CheckpointController Instance { get; private set; }
        private Vector3 _lastCheckpointPositon;
        private int _lastScore;
        private TransformationManager.PlayerMaterialType _lastForm;
        private bool _hasCheckpoint;
        private readonly List<SaveableObject> _registeredObjects = new List<SaveableObject>();
        private readonly Dictionary<string, ObjectState> _savedObjectStates = new Dictionary<string, ObjectState>();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public void SetPlayer(PlayerController player)
        {
            if(_hasCheckpoint)
            {
                player.RestoreFromCheckpoint(_lastCheckpointPositon, _lastScore, _lastForm);
            }
        }

        public void RegisterSaveable(SaveableObject saveable)
        {
            if (!_registeredObjects.Contains(saveable))
            {
                _registeredObjects.Add(saveable);
            }
        }

        public bool TryGetSavedState(string uniqueId, out ObjectState state)
        {
            return _savedObjectStates.TryGetValue(uniqueId, out state);
        }

        public void SetCheckpoint(Vector3 checkpointPosition, int score, TransformationManager.PlayerMaterialType lastForm)
        {
            _lastCheckpointPositon = checkpointPosition;
            _lastScore = score;
            _lastForm = lastForm;
            Debug.Log($"Checkpoint set at position: {_lastCheckpointPositon}, score: {_lastScore}, form: {_lastForm}");

            _savedObjectStates.Clear();
            foreach (var saveable in _registeredObjects)
            {
                if (saveable != null)
                {
                    _savedObjectStates[saveable.UniqueID] = saveable.CaptureState();
                }
            }
            _lastForm = lastForm;
            _hasCheckpoint = true;
            Debug.Log($"Saved states for {_savedObjectStates.Count} objects at checkpoint.");
        }

        public void RestoreObjects()
        {
            foreach (var saveable in _registeredObjects)
            {
                if (saveable == null) continue;

                if (_hasCheckpoint && _savedObjectStates.TryGetValue(saveable.UniqueID, out var state))
                {
                    saveable.RestoreState(state);
                }
                else
                {
                    // If dying before hitting any checkpoint, reset to default active state
                    saveable.ResetToDefault();
                }
            }
        }

        public void ClearCheckpoints()
        {
            _lastCheckpointPositon = Vector3.zero;
            _lastScore = 0;
            _hasCheckpoint = false;
            _lastForm = TransformationManager.PlayerMaterialType.Planks;
            _registeredObjects.Clear();
            _savedObjectStates.Clear();
        }
    }

}
