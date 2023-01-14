using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBeam 
{
    Vector3 pos, dir;

    GameObject beamObj;
    LineRenderer beam;
    List<Vector3> beamIndices = new List<Vector3>();
    public SoundBeam(Vector3 pos, Vector3 dir, Material material)
    {
        this.beam = new LineRenderer();
        this.beamObj = new GameObject();
        this.beamObj.name = "Aim Beam";
        this.pos = pos;
        this.dir = dir;

        this.beam = this.beamObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        this.beam.startWidth = 0.1f;
        this.beam.endWidth = 0.1f;
        this.beam.material = material;
        this.beam.startColor = Color.white;
        this.beam.endColor = Color.white;

        CastBeam(pos, dir, beam);
    }

    void CastBeam(Vector3 pos, Vector3 dir, LineRenderer beam)
    {
        beamIndices.Add(pos);
        Ray beamCast = new Ray(pos, dir);
        RaycastHit hit;

        if(Physics.Raycast(beamCast, out hit, 30, 1))
        {
            /*beamIndices.Add(hit.point);
            UpdateBeam();*/
            ReflectBeam(hit, dir, beam);
        } else {
            beamIndices.Add(beamCast.GetPoint(30));
            UpdateBeam();
        }
    }

    void UpdateBeam()
    {
        int count = 0;
        beam.positionCount = beamIndices.Count;

        foreach (Vector3 idx in beamIndices)
        {
            beam.SetPosition(count, idx);
        }
    }
    
    void ReflectBeam(RaycastHit hitInfo, Vector3 direction, LineRenderer beam)
    {
        Vector3 pos = hitInfo.point;
        Vector3 dir = Vector3.Reflect(direction, hitInfo.normal);

        CastBeam(pos, dir, beam);
    }
}
