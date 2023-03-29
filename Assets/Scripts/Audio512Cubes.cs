using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio512Cubes : MonoBehaviour
{
    [SerializeField] private GameObject _sampleCubePrefab;
    private GameObject[] _sampleCubes = new GameObject[512];
    [SerializeField] private float _maxScale = 1000f;

    [SerializeField] private AudioPeer audioPeer;

    [SerializeField] private float _rotationAfterInstantiate = -45f;

    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject _instanceSampleCube = Instantiate(_sampleCubePrefab);
            _instanceSampleCube.transform.position = this.transform.position;
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "sampleCube " + i;
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            _instanceSampleCube.transform.position = Vector3.forward * 200;
            _sampleCubes[i] = _instanceSampleCube;
        }
        this.transform.eulerAngles = new Vector3(0, this.transform.rotation.y, _rotationAfterInstantiate);
    }

    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (_sampleCubes != null)
            {
                _sampleCubes[i].transform.localScale = new Vector3(0.75f, (audioPeer._samplesLeft[i] * _maxScale) + 0.5f, 0.75f);
            }
        }
    }
}
