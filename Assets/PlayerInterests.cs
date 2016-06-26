using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class PlayerInterests : IEnumerable<Vector3>
    {
        private Vector3 _player;
        private IList<Vector3> _otherPoints;

        public PlayerInterests(Vector3 player)
        {
            _player = player;
            _otherPoints = new List<Vector3>();
        }

        public Vector3 GetPlayerPosition()
        {
            return _player;
        }

        public void Add(Vector3 point)
        {
            _otherPoints.Add(point);
        }

        public IEnumerator<Vector3> GetEnumerator()
        {
            return new[] { _player }.Concat(_otherPoints).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (!(obj is PlayerInterests)) return false;
            return Equals(obj as PlayerInterests);
        }

        public bool Equals(PlayerInterests p)
        {
            if (!_player.Equals(p._player)) return false;
            if (_otherPoints.Count != p._otherPoints.Count) return false;

            for (var i = 0; i < _otherPoints.Count; i++)
            {
                if (_otherPoints[i].Equals(p._otherPoints[i])) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _player.GetHashCode();
        }
    }
}
