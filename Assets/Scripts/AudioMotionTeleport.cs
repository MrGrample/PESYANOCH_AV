using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMotionTeleport : MonoBehaviour
{

    [SerializeField] private Transform[] _positions;
    [SerializeField] private AudioPeer _audioPeer;

    void Start()
    {
    }

    void Update()
    {
        transform.position = _positions[_audioPeer.GetMaximumAmplitudeBandIndexBuffer()].position;    
        transform.rotation = _positions[_audioPeer.GetMaximumAmplitudeBandIndexBuffer()].rotation;
    }
}
