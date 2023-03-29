using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmissionChange : MonoBehaviour
{
    [SerializeField] private int _band;
    [SerializeField] private float _startScale, _scaleMultiplier;
    [SerializeField] private bool _useBuffer = true;

    [SerializeField] private AudioPeer audioPeer;

    [SerializeField] private Color _color;

    private Material _objectMaterial;


    void Start()
    {
        _objectMaterial = transform.GetComponent<Renderer>().material;
    }


    void Update()
    {
        if (_useBuffer)
        {
            _objectMaterial.SetColor("_EmissionColor", _color * (audioPeer._audioBandBuffer[_band] * _scaleMultiplier + _startScale));
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer._freqBands[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
        }

    }
}
