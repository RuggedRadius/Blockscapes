using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private Material matCurrent;
    public Material matBase;

    private MeshRenderer meshRend;
    public Color blockColour;
    public bool lastOfColour;

    private void Awake()
    {
        meshRend = this.GetComponent<MeshRenderer>();        
    }

    void Start()
    {
        matCurrent = new Material(matBase);        
        matCurrent.color = blockColour;
        meshRend.material = matCurrent;
    }

    //private void Update()
    //{
    //    if (lastOfColour)
    //    {
    //        Debug.Log("Destroying last block of " + blockColour.ToString());
    //        GameController.blocks.DestroyBlock(this.gameObject);
    //    }
    //}
}
