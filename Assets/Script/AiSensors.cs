using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class AiSensors : MonoBehaviour
{
    #region Exposed

    public float m_distance = 10f;
    public float m_angle = 30f;
    public float m_height = 1.0f;
    public Color m_meshColor = Color.red;
    public int m_scanFrequency = 30;

    public LayerMask m_layers;
    public LayerMask m_occlusionLayer;

    public List<GameObject> Objects = new List<GameObject>();

    public bool m_isGroundUnit;



    #endregion


    #region Unity API

    void Start()
    {
        _scanInterval = 1.0f / m_scanFrequency;
    }


    void Update()
    {
        _scanTimer -= Time.deltaTime;

        if (_scanTimer < 0)
        {
            _scanTimer += _scanInterval;
            Scan();
        }
    }

    #endregion 


    #region Main Method

    private void Scan()
    {
        _count = Physics.OverlapSphereNonAlloc(transform.position, m_distance, _colliders, m_layers, QueryTriggerInteraction.Collide);

        Objects.Clear();

        for (int i = 0; i < _count; i++)
        {
            GameObject obj = _colliders[i].gameObject;

            if (m_isInsight(obj))
            {
                Objects.Add(obj);
            }
        }
    }

    public bool m_isInsight(GameObject obj)
    {
        Vector3 _origin = transform.position;
        Vector3 _dest = obj.transform.position;
        Vector3 _direction = _dest - _origin;

        if (Vector3.Distance(_origin, _dest) > m_distance)
        {
            return false;
        }

        if (m_isGroundUnit)
        {
            if (_direction.y < 0 || _direction.y > m_height)
            {
                return false;
            }
        }


        _direction.y = 0;
        float _deltaAngle = Vector3.Angle(_direction, transform.forward);

        if (_deltaAngle > m_angle)
        {
            return false;
        }



        _origin.y += m_height / 2;
        _dest.y = _origin.y;

        if (Physics.Linecast(_origin, _dest, m_occlusionLayer))
        {
            return false;
        }

        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int _segments = 10;
        int _numTriangles = (_segments * 4) + 2 + 2;
        int _numVertices = _numTriangles * 3;

        Vector3[] _vertices = new Vector3[_numVertices];
        int[] _triangles = new int[_numVertices];

        Vector3 _bottomCenter = Vector3.zero;
        Vector3 _bottomLeft = Quaternion.Euler(0, -m_angle, 0) * Vector3.forward * m_distance;
        Vector3 _bottomRight = Quaternion.Euler(0, m_angle, 0) * Vector3.forward * m_distance;

        Vector3 _topCenter = _bottomCenter + Vector3.up * m_height;
        Vector3 _topLeft = _bottomLeft + Vector3.up * m_height;
        Vector3 _topRight = _bottomRight + Vector3.up * m_height;

        int _vert = 0;

        //Left side
        _vertices[_vert++] = _bottomCenter;
        _vertices[_vert++] = _bottomLeft;
        _vertices[_vert++] = _bottomRight;

        _vertices[_vert++] = _topLeft;
        _vertices[_vert++] = _topCenter;
        _vertices[_vert++] = _bottomCenter;

        //right - side
        _vertices[_vert++] = _bottomCenter;
        _vertices[_vert++] = _topCenter;
        _vertices[_vert++] = _topRight;

        _vertices[_vert++] = _topRight;
        _vertices[_vert++] = _bottomRight;
        _vertices[_vert++] = _bottomCenter;

        float _currentAngle = -m_angle;
        float _deltaAngle = (m_angle * 2) / _segments;

        for (int i = 0; i < _segments; i++)
        {

            _bottomLeft = Quaternion.Euler(0, _currentAngle, 0) * Vector3.forward * m_distance;
            _bottomRight = Quaternion.Euler(0, _currentAngle + _deltaAngle, 0) * Vector3.forward * m_distance;


            _topLeft = _bottomLeft + Vector3.up * m_height;
            _topRight = _bottomRight + Vector3.up * m_height;

            //Far Side
            _vertices[_vert++] = _bottomLeft;
            _vertices[_vert++] = _bottomRight;
            _vertices[_vert++] = _topRight;

            _vertices[_vert++] = _topRight;
            _vertices[_vert++] = _topLeft;
            _vertices[_vert++] = _bottomLeft;

            //Top
            _vertices[_vert++] = _topCenter;
            _vertices[_vert++] = _topLeft;
            _vertices[_vert++] = _topLeft;

            //Bottom
            _vertices[_vert++] = _bottomCenter;
            _vertices[_vert++] = _topRight;
            _vertices[_vert++] = _topLeft;

            _currentAngle += _deltaAngle;
        }




        for (int i = 0; i < _numVertices; i++)
        {
            _triangles[i] = i;
        }

        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = m_meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, m_distance);

        for (int i = 0; i < _count; i++)
        {
            Gizmos.DrawSphere(_colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }

    }


    #endregion


    #region Privates

    private Collider[] _colliders = new Collider[50];
    private Mesh mesh;
    private int _count;
    private float _scanInterval;
    private float _scanTimer;

    #endregion
}
