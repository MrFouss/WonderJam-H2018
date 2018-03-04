using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

    private bool _flickerEnabled = false;
    public bool FlickerEnabled
    {
        set
        {
            // toggle true
            if (!_flickerEnabled && value)
            {
                _flickerEnabled = true;
                FlickerOn();
            } else
            {
                _flickerEnabled = value;
            }
        }

        get
        {
            return _flickerEnabled;
        }
    }

    public Material FlickerMaterial;
    public float FlickerDuration = 0.5f;
    public AnimationCurve RandomDistribution;
    public float MaxFlickerDelayInSeconds = 3;

    private Material baseMaterial;
    private MeshRenderer meshRenderer;
    
	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        baseMaterial = meshRenderer.material;
	}

    private void FlickerOn()
    {
        if (FlickerEnabled)
        {
            FlickerObjectMaterial(true);
            Invoke("FlickerOff", FlickerDuration);
        }
    }

    private void FlickerOff()
    {
        FlickerObjectMaterial(false);
        float rand = Random.Range(0.0f, 1.0f);
        float nextFlickerDelay = RandomDistribution.Evaluate(rand) * MaxFlickerDelayInSeconds;
        Invoke("FlickerOn", nextFlickerDelay);
    }

    private void FlickerObjectMaterial(bool on)
    {
        if (on)
        {
            meshRenderer.material = FlickerMaterial;
        } else
        {
            meshRenderer.material = baseMaterial;
        }
    }
}
