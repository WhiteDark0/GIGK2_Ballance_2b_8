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
        private bool _hasCheckpoint;

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
                player.RestoreFromCheckpoint(_lastCheckpointPositon, _lastScore);
            }
        }

        public void SetCheckpoint(Vector3 checkpointPosition, int score)
        {
            _lastCheckpointPositon = checkpointPosition;
            _lastScore = score;
            _hasCheckpoint = true;
            Debug.Log($"Checkpoint set at position: {_lastCheckpointPositon}, score: {_lastScore}");
        }
    }

}
