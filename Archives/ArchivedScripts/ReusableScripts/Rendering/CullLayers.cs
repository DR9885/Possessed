using UnityEngine;


[System.Serializable]
public class CullLayer
{
	[SerializeField] private int _Layer;
	[SerializeField] private float _Distance;
	
	public int Layer
	{
		get { return _Layer; }	
		set { _Layer = value; }
	}

	public float Distance
	{
		get { return _Distance; }	
		set { _Distance = value; }
	}
}

public class CullLayers : MonoBehaviour
{
	[SerializeField] private CullLayer[] _CullLayers;
	[SerializeField] private Camera[] _Cameras;
	
	private CullLayer[] _TrackedCullLayers;

	void Start () 
	{
		UpdateCullLayers();
	}
	
	void Update()
	{
		if(_TrackedCullLayers == null || !_TrackedCullLayers.Equals(_CullLayers))
		{
			_TrackedCullLayers = new CullLayer[_CullLayers.Length];
			for(int i = 0; i < _CullLayers.Length; i++)
			{
				_TrackedCullLayers[i] = new CullLayer();
				_TrackedCullLayers[i].Distance = _CullLayers[i].Distance;
				_TrackedCullLayers[i].Layer = _CullLayers[i].Layer;
			}
			UpdateCullLayers();	
		}
	}
	
	void UpdateCullLayers()
	{
		//Debug.Log("Update");
		
		
		float[] distances = new float[32];
		
		for(int i = 0; i < _CullLayers.Length; i++)
		{
			CullLayer cullLayer = _CullLayers[i];
			if(cullLayer.Layer < 32)
				distances[cullLayer.Layer] = Mathf.Max(0.0f, cullLayer.Distance);
		}
		
		if(_Cameras != null)
			for(int j = 0; j < _Cameras.Length; j++)
				if (_Cameras[j] != null)
					_Cameras[j].layerCullDistances = distances;
	}
}