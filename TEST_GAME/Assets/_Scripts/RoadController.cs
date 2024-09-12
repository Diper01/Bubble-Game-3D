using System;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x,_player.localScale.y,transform.localScale.z);
    }
}