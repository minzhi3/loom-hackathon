using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pacman
{
	public class DotController : MonoBehaviour
    {

        public int DotValue;
        public DotEffect Effect;
		[SerializeField] DotState state;
		//0 : initial
		//1 : ready for click
		//2 : fly
		//3 : need remove
		//4 : removed

        private SpriteRenderer _renderer;
		private void Awake()
		{

            _renderer = this.GetComponentInChildren<SpriteRenderer>();
		}

		private void OnTriggerEnter2D(Collider2D other)
        {
            if (state == DotState.ReadyForClick)
            {
                PacmanController pacman = other.GetComponent<PacmanController>();
                BeEaten(DotValue);
				pacman.EatDot(DotValue);
				if (Effect != DotEffect.Normal) pacman.SetEffect(Effect);
            }
        }
        void BeEaten(int value)
        {
			this._renderer.gameObject.SetActive(false);
			this.state = DotState.Removed;
			StartCoroutine(Respawn(5));
        }

		IEnumerator Respawn(float time)
		{
			yield return new WaitForSeconds(time);
            this.state = DotState.ReadyForClick;
			this._renderer.gameObject.SetActive(true);
		}

        // Use this for initialization
        void Start()
        {
			this.state = DotState.ReadyForClick;
			this.DotValue = Constant.DefaultDotValue;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
