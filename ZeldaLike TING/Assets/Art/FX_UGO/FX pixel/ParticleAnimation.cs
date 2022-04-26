using System.Collections;
using UnityEngine;



	[ExecuteInEditMode]
	public class ParticleAnimation : MonoBehaviour
	{
#pragma warning disable CS0649

		[SerializeField] private Animation m_animation;
		[SerializeField] private ParticleSystem m_particleSystem;

#pragma warning restore CS0649

		private void OnEnable()
		{
#if UNITY_EDITOR
			if(!Application.isPlaying)
				return;
#endif

			if(m_animation.GetClipCount() == 0)
			{
				return;
			}

			IEnumerator enumerator = m_animation.GetEnumerator();
			if(enumerator.MoveNext())
			{
				var state = (AnimationState) enumerator.Current;
				m_animation.Play(state.name);
			}
		}

		private void OnDisable()
		{
#if UNITY_EDITOR
			if(!Application.isPlaying)
				return;
#endif

			m_animation.Stop();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if(Application.isPlaying)
				return;

			m_animation.playAutomatically = false;

			if(m_animation.GetClipCount() == 0)
			{
				return;
			}

			IEnumerator enumerator = m_animation.GetEnumerator();
			if(enumerator.MoveNext())
			{
				var state = (AnimationState) enumerator.Current;
				m_animation.Play(state.name);
				state.time = m_particleSystem.time;
			
				m_animation.Sample();
			}
		}

		private void Reset()
		{
			m_animation = GetComponent<Animation>();
			m_particleSystem = GetComponent<ParticleSystem>();
		}
#endif
	}

