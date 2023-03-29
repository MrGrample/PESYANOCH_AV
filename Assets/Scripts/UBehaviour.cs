using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UBehaviour : MonoBehaviour
{

    [SerializeField] private float shakeSpeed = 2f;
    [SerializeField] private float shakeAmount = 0.5f;


    [SerializeField] private AudioPeer _audioPeer;

    [SerializeField] private int _band;
    [SerializeField] private float _startScale = 1f, _scaleMultiplier = 5f;
    [SerializeField] private bool _useBuffer = true;

    void Start()
    {
        StartCoroutine(Shaking());
    }

    void Update()
    {
        shakeSpeed = _audioPeer._audioBandBuffer[_band] * _scaleMultiplier + _startScale;
        shakeAmount = _audioPeer._audioBandBuffer[_band] * _scaleMultiplier + _startScale;
    }

    protected IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;

        while (true)
        {

            float sin = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            float cos = Mathf.Cos(Time.time * shakeSpeed) * shakeAmount;
            transform.position = new Vector3(startPosition.x + sin,
                                             startPosition.y + sin,
                                             startPosition.z + cos);
            yield return null;

        }
    }
}
