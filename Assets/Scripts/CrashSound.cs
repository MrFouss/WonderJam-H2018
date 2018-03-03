using UnityEngine;
using System.Collections;

public class CrashSound : MonoBehaviour {

	[Tooltip("liste des sons lors de la collision du plus faible au plus fort")]
    public AudioClip[] crashSounds;
    public Transform particle;


    private AudioSource source;
    private float lowPitchRange = .75F;
    private float highPitchRange = 1.5F;
    private float velToVol = .2F;
    private float maxVelocity = 10F;


    void Awake () {
    
        source = GetComponent<AudioSource>();
    }


    void OnCollisionEnter (Collision coll)
    {
        source.pitch = Random.Range (lowPitchRange,highPitchRange);
        float hitVol = coll.relativeVelocity.magnitude * velToVol;

		int i = (int)Mathf.Min(Mathf.Floor(coll.relativeVelocity.magnitude / maxVelocity * crashSounds.Length), crashSounds.Length-1);
		Debug.Log(i);
        source.PlayOneShot(crashSounds[i],hitVol);
        Instantiate(particle, coll.contacts[0].point, Quaternion.identity);
    }

}