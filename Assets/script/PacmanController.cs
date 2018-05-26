using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pacman
{
	public class PacmanController : MonoBehaviour
    {

        int direction;
        public float velocity;

        private SpriteRenderer _renderer;
        //pacman move
        float currentRatio;
        [SerializeField] Vector3Int pacManMoveTarget;
        [SerializeField] Vector3Int pacManMoveFrom;
        Vector3Int[] directionVector = { Vector3Int.zero, Vector3Int.left, Vector3Int.down, Vector3Int.up, Vector3Int.right };

        public float moveTime;
        //farmer enhance
        public bool IsSpeedUp { get { return speedUpTime > 0; } }
        public bool IsBig { get { return BigTime > 0; } }
        GameObject trail;
        public float speedUpTime;
        public float BigTime;

        Quaternion clockWise;
        Quaternion counterClockWise;

        private void Awake()
        {
            _renderer = this.GetComponentInChildren<SpriteRenderer>();
            moveTime = 0.5f;
            clockWise = Quaternion.FromToRotation(Vector3.up, Vector3.right);
            counterClockWise = Quaternion.FromToRotation(Vector3.up, Vector3.left);
        }
        // Use this for initialization
        void Start()
        {
        }

        public void EatDot(int value)
        {
            Debug.Log("eat dot " + value);
			UIManager.Instance.AddScore(value);
        }

		public void SetEffect(DotEffect effect)
		{
			switch (effect)
			{
				case DotEffect.BigSize:
					this.BigTime += Constant.BigSizeTime;
					break;
				case DotEffect.SpeedUp:
					this.speedUpTime += Constant.SpeedUpTime;
					break;
			}
		}


    void PacManMove()
        {
            float currentMoveTime = this.IsSpeedUp ? (moveTime / 1.5f) : moveTime;
            currentRatio += (Time.deltaTime / currentMoveTime);
            transform.position = Vector3.Lerp(pacManMoveFrom, pacManMoveTarget, currentRatio);
            _renderer.flipX = (pacManMoveTarget.x - pacManMoveFrom.x < 0) && (pacManMoveTarget.y == pacManMoveFrom.y);

            if ((pacManMoveTarget.y - pacManMoveFrom.y) > 0)
            {
                _renderer.transform.rotation = counterClockWise;
                _renderer.flipX = false;
            }
            else if ((pacManMoveTarget.y - pacManMoveFrom.y) < 0)
            {
                _renderer.transform.rotation = clockWise;
                _renderer.flipX = false;
            }
            else if (pacManMoveTarget.x < pacManMoveFrom.x)
            {
                _renderer.flipX = true;
                _renderer.transform.rotation = Quaternion.identity;
            }
            else if (pacManMoveTarget.x > pacManMoveFrom.x)
            {
                _renderer.flipX = false;
                _renderer.transform.rotation = Quaternion.identity;
            }
            if (currentRatio >= 1f)
            {
                currentRatio -= 1f;
                FindNextTarget();
            }
        }

        void FindNextTarget()
        {
            pacManMoveFrom = pacManMoveTarget;
            if (direction < 5)
                pacManMoveTarget = pacManMoveFrom + directionVector[direction];

        }

        void ChangeDirection()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    this.direction = 1;
                else if (Input.GetKey(KeyCode.DownArrow))
                    this.direction = 2;
                else if (Input.GetKey(KeyCode.UpArrow))
                    this.direction = 3;
                else if (Input.GetKey(KeyCode.RightArrow))
                    this.direction = 4;
            }
        }
        // Update is called once per frame
        void Update()
        {
            ChangeDirection();
            PacManMove();
        }
    }

}

