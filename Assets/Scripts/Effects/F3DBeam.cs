using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class F3DBeam : MonoBehaviour
{
    public F3DFXType fxType;

    public Texture[] BeamFrames;

    public bool AnimateUV;

    public float UVTime;

    private LineRenderer lineRenderer;


    private float initialBeamOffset;
    private void Awake()
	{
		lineRenderer = base.GetComponent<LineRenderer>();
		if (!AnimateUV && BeamFrames.Length > 0)
		{
			lineRenderer.material.mainTexture = BeamFrames[0];
		}
		initialBeamOffset = UnityEngine.Random.Range(0f, 5f);
	}

	private void Update()
	{
		if (AnimateUV)
		{
			lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * UVTime + initialBeamOffset, 0f));
		}
	}
}
