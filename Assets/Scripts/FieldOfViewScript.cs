using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewScript : MonoBehaviour
{
  public float fieldOfView = 100f;
  public int amountOfRays = 250;
  //private static int amountOfRaysStatic;
  public float viewDistance = 4f;

  [SerializeField]
  private LayerMask layerMask;

  //private Mesh mesh;
  private Sprite sprite;
  //private Vector3 origin;
  private Vector2 spriteOrigin;
  private float startAngle;

  //private static Vector3[] vertices;
  private static Vector2[] spriteVertices;
  //private static Vector2[] uv;
  //private static int[] triangles;
  private static ushort[] spriteTriangles;

  private void Start()
  {
    //mesh = new Mesh();
    //amountOfRaysStatic = amountOfRays;
    sprite = GetComponent<SpriteMask>().sprite;
    //origin = Vector3.zero;
    spriteOrigin = Vector2.zero;

    //vertices = new Vector3[amountOfRays + 2];
    spriteVertices = new Vector2[amountOfRays + 2];
    //uv = new Vector2[vertices.Length];
    //triangles = new int[amountOfRays * 3];
    spriteTriangles = new ushort[amountOfRays * 3];
}

  private void Update()
  {
    //origin = Vector3.zero;

    float angle = startAngle;
    float angleIncrease = fieldOfView / amountOfRays;

    //vertices[0] = origin;
    spriteVertices[0] = spriteOrigin;
    for (int i = 0; i <= amountOfRays; ++i)
    {
      RaycastHit2D raycastHit = Physics2D.Raycast(new Vector3(spriteOrigin.x, spriteOrigin.y), GetVectorFromAngle(angle), viewDistance, layerMask);
      
      if (raycastHit.collider == null)
      {
        //vertices[i + 1] = origin + GetVectorFromAngle(angle) * viewDistance;
        spriteVertices[i + 1] = spriteOrigin + Vector3ToVector2(GetVectorFromAngle(angle)) * viewDistance;
      } else
      {
        //vertices[i + 1] = raycastHit.point;
        spriteVertices[i + 1] = Vector3ToVector2(raycastHit.point);
      }

      if (i > 0)
      {
        //triangles[(i-1) * 3] = 0;
        //triangles[(i-1) * 3 + 1] = i;
        //triangles[(i-1) * 3 + 2] = i + 1;
        spriteTriangles[(i - 1) * 3] = 0;
        spriteTriangles[(i - 1) * 3 + 1] = System.Convert.ToUInt16(i);
        spriteTriangles[(i - 1) * 3 + 2] = System.Convert.ToUInt16(i + 1);
      }

      angle -= angleIncrease;
    }

    //mesh.vertices = vertices;
    //mesh.uv = uv;
    //mesh.triangles = triangles;

    //GetComponent<MeshFilter>().mesh = mesh;
    for (int i = 0; i < spriteVertices.Length; ++i)
    {
      spriteVertices[i] = (spriteVertices[i] * sprite.pixelsPerUnit) + sprite.pivot;
    }
    sprite.OverrideGeometry(spriteVertices, spriteTriangles);
    GetComponent<SpriteMask>().sprite = sprite;
  }

  private Vector2 Vector3ToVector2(Vector3 vector3)
  {
    return new Vector2(vector3.x, vector3.y);
  }

  private Vector3 GetVectorFromAngle(float angle)
  {
    float angleRad = angle * Mathf.Deg2Rad;
    return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
  }

  private float GetAngleFromVector3 (Vector3 direction)
  {
    direction = direction.normalized;
    float result = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    if (result < 0) result += 360;
    return result;
  }

  public void SetOrigin(Vector3 origin)
  {
    //this.origin = origin;
    this.spriteOrigin = Vector3ToVector2(origin);
  }

  public void SetAimDirection (Vector3 aimDirection)
  {
    startAngle = GetAngleFromVector3(aimDirection) + fieldOfView / 2f;
  }
}
