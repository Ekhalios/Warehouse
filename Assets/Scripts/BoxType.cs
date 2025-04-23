using UnityEngine;

public enum BoxTypes
{
    A,B,C,Plastic,
}
public class BoxType : MonoBehaviour
{
    public BoxTypes boxType;
    
    public bool sorted = false;
}
