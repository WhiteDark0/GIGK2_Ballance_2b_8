using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class CheckpointController : MonoBehaviour
    {
        public static CheckpointController Instance { get; private set; }
        public Transform PlayerTransform { get; private set; }
        private Vector3 _lastCheckpointPositon;

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

        public void SetPlayerTransform(Transform playerTransform)
        {
            PlayerTransform = playerTransform;

            // If a checkpoint has already been reached, move the player to that position
            if(_lastCheckpointPositon != Vector3.zero)
            {
                PlayerTransform.position = _lastCheckpointPositon;
            }
        }

        public void SetCheckpoint(Vector3 checkpointPosition)
        {
            _lastCheckpointPositon = checkpointPosition;
        }
    }

}
