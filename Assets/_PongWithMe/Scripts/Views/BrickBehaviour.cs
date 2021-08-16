using Shapes;
using UnityEngine;

namespace PongWithMe
{
    public class BrickBehaviour : MonoBehaviour
    {
        [SerializeField] private Disc _shape = null;

        private Brick _brick = null;
        
        public void Initialize(Brick brick)
        {
            _brick = brick;
            _brick.OnColorSet += HandleOnColorSet;
            _brick.OnPositionSet += HandleOnPositionSet;
            transform.position = _brick.Position;
        }

        private void OnDisable()
        {
            if (_brick != null)
            {
                _brick.OnColorSet -= HandleOnColorSet;
                _brick.OnPositionSet -= HandleOnPositionSet;
                _brick = null;
            }
        }

        #region Delegate

        private void HandleOnColorSet(Color color)
        {
            _shape.Color = color;
        }

        private void HandleOnPositionSet(Vector3 position)
        {
            transform.position = position;
        }
        #endregion
    }
}

