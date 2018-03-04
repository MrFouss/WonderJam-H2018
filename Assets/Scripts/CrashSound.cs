using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class CrashSound : MonoBehaviour {

	[Tooltip("liste des sons lors de la collision du plus faible au plus fort")]
    public AudioClip[] crashSounds;
    public Transform particle;
    public UnityEngine.PostProcessing.PostProcessingProfile profile;

    private AudioSource source;
    private float lowPitchRange = .75F;
    private float highPitchRange = 1.5F;
    private float velToVol = .2F;
    private float maxVelocity = 10F;

    private float lastCrashTime;
    private float chromaticAberrationDuration = 0.4f;

    void Awake () {
        source = GetComponent<AudioSource>();
        lastCrashTime = Time.time;
    }

    void Update() {
        if (Time.time <= lastCrashTime + chromaticAberrationDuration) {
            UnityEngine.PostProcessing.ChromaticAberrationModel.Settings chromaticAberrationSettings = profile.chromaticAberration.settings;
            chromaticAberrationSettings.intensity = Mathf.Sin((Time.time - lastCrashTime) * Mathf.PI / chromaticAberrationDuration);
            profile.chromaticAberration.settings = chromaticAberrationSettings;
        }
    }

    void OnCollisionEnter (Collision coll)
    {
		if (coll.transform.tag == "Balle") {
			source.pitch = Random.Range (lowPitchRange, highPitchRange);
			float hitVol = coll.relativeVelocity.magnitude * velToVol;

			lastCrashTime = Time.time;

			int i = (int) Mathf.Min(Mathf.Floor(coll.relativeVelocity.magnitude / maxVelocity * crashSounds.Length), crashSounds.Length-1);
			source.PlayOneShot(crashSounds[i],hitVol);
			Instantiate(particle, coll.contacts[0].point, Quaternion.identity);
		}
    }

}