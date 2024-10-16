using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageFlash : MonoBehaviour
{
    [SerializeField]
    private float _flashDuration;
    [SerializeField]
    private Color _flashColor;
    [SerializeField]
    private int _numberOffFlashes;

    private SpriteFlash _spriteFlash;


    private void Awake()
    {
        _spriteFlash = GetComponent<SpriteFlash>();

    }

    public void StartFlash() 
    {
        _spriteFlash.StartFlash(_flashDuration,_flashColor, _numberOffFlashes);
    }
}
