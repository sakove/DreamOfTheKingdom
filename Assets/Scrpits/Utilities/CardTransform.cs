using UnityEngine;

public class CardTransform
{
    public Vector3 pos;
    public Quaternion rot;

    public CardTransform(Vector3 pos, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
    }
}
