using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    [SerializeField] private int _band;
    [SerializeField] private float _startScale, _scaleMultiplier;
    [SerializeField] private bool _useBuffer = true;

    [SerializeField] private AudioPeer audioPeer;


    void Start()
    {
        
    }


    void Update()
    {
        if(_useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer._bandBuffer[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, (audioPeer._freqBands[_band] * _scaleMultiplier) + _startScale, transform.localScale.z);
        }
 
    }
}
