using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialWizardry : MonoBehaviour
{
    public Color Color;
 
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
 
    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _propBlock.SetColor("_Color", Color);
        _renderer.SetPropertyBlock(_propBlock);
    }
}