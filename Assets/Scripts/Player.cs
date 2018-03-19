using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour {

	public List<AudioClip> footStepFiles;
	public AudioMixerGroup footstepChannel;
	public AudioMixer MainMixer;
	[Range(0.1f, 5f)]
	public float walkingRate = 1f;
	private AudioSource[] aSources = new AudioSource[2];

	private Coroutine[] footstepRoutines = new Coroutine[2];

	private int currentAudioSource = 0;
	private int currentClipPlaying = 0;

	void Awake() {
		int aSourcesIndex = 0;
		foreach(AudioSource aSouce in aSources) {
			aSources[aSourcesIndex] =  gameObject.AddComponent<AudioSource>();
			aSources[aSourcesIndex].clip = footStepFiles[aSourcesIndex];
			aSources[aSourcesIndex].outputAudioMixerGroup = footstepChannel;
			aSources[aSourcesIndex].playOnAwake = false;
			aSources[aSourcesIndex].spatialBlend = 1;
			aSourcesIndex++;
			currentClipPlaying++;
		}
	}


	// Use this for initialization
	void Start () {
		InvokeRepeating("triggerFootstep", 0f, walkingRate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void triggerFootstep() {
		footstepRoutines[currentAudioSource] = StartCoroutine(playFootstep());
	}

	IEnumerator playFootstep() {
		// if (aSources[lastAudioSourceModified].isPlaying) {
		// 	FadeOut(aSources[lastAudioSourceModified], 0.05f);
		// 	yield return new WaitForSeconds(0.05f);			
		// }
		aSources[currentAudioSource].clip = footStepFiles[currentClipPlaying];
		varySound();
		aSources[currentAudioSource].Play();
		yield return new WaitForSeconds(footStepFiles[currentClipPlaying].length);			
		incrementAudioVariables();
		yield return null;
	}

	void incrementAudioVariables() {			
		currentClipPlaying = (currentClipPlaying + 1) % (footStepFiles.Count - 1);
		Debug.Log("clip playing is " + currentClipPlaying);
		currentAudioSource = (currentAudioSource + 1) % (aSources.Length);
		Debug.Log("audio source playing is " + currentAudioSource);
	}

	void varySound() {
		aSources[currentAudioSource].volume = Random.Range(0.9f, 1f);
		aSources[currentAudioSource].pitch = Random.Range(0.95f, 1.05f);
		MainMixer.SetFloat("FootFilter1", Random.Range(1000f, 1500f));
		MainMixer.SetFloat("FootFilter2", Random.Range(1500f, 2000f));
	}


	private IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }
}
