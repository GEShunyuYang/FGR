using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class IndirectGrassRenderer : MonoBehaviour
{
    [SerializeField] private GameObject GrassPrefab;
    private Transform Player;

    [SerializeField] private int InstanceCount = 50000;
    [SerializeField] private Vector2 AreaSize = new Vector2(80f, 80f);

    [SerializeField] private float MinScale = 0.8f;
    [SerializeField] private float MaxScale = 1.4f;

    [SerializeField] private float PlayerBendRadius = 1.5f;
    [SerializeField] private float BendStrength = 1.2f;

    [SerializeField] private Texture2D GroundTintTexture;
    [SerializeField, Range(0f, 1f)] private float TintStrength = 0.5f;

    private Mesh mesh;
    private Material material;

    private ComputeBuffer matrixBuffer;
    private ComputeBuffer argsBuffer;

    private Bounds drawBounds;

    private bool hasTriedFindPlayer;

    private static readonly int MatricesId = Shader.PropertyToID("_Matrices");
    private static readonly int PlayerPosRadiusId = Shader.PropertyToID("_PlayerPosRadius");
    private static readonly int BendStrengthId = Shader.PropertyToID("_BendStrength");
    private static readonly int GroundTintTexId = Shader.PropertyToID("_GroundTintTex");
    private static readonly int TintStrengthId = Shader.PropertyToID("_TintStrength");
    private static readonly int TintMapOriginSizeId = Shader.PropertyToID("_TintMapOriginSize");

    private void Start()
    {
        MeshFilter meshFilter = GrassPrefab.GetComponentInChildren<MeshFilter>();
        MeshRenderer meshRenderer = GrassPrefab.GetComponentInChildren<MeshRenderer>();

        mesh = meshFilter.sharedMesh;
        material = new Material(meshRenderer.sharedMaterial);

        Matrix4x4[] matrices = new Matrix4x4[InstanceCount];

        for (int i = 0; i < InstanceCount; i++)
        {
            float x = Random.Range(-AreaSize.x * 0.5f, AreaSize.x * 0.5f);
            float z = Random.Range(-AreaSize.y * 0.5f, AreaSize.y * 0.5f);

            Vector3 position = transform.position + new Vector3(x, 0f, z);
            Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            float scaleValue = Random.Range(MinScale, MaxScale);
            Vector3 scale = Vector3.one * scaleValue;

            matrices[i] = Matrix4x4.TRS(position, rotation, scale);
        }

        matrixBuffer = new ComputeBuffer(InstanceCount, sizeof(float) * 16);
        matrixBuffer.SetData(matrices);

        uint[] args = new uint[5];
        args[0] = mesh.GetIndexCount(0);
        args[1] = (uint)InstanceCount;
        args[2] = mesh.GetIndexStart(0);
        args[3] = mesh.GetBaseVertex(0);
        args[4] = 0;

        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);

        material.SetBuffer(MatricesId, matrixBuffer);

        drawBounds = new Bounds(
            transform.position,
            new Vector3(AreaSize.x, 30f, AreaSize.y)
        );

        // grass texture
        if (GroundTintTexture != null)
        {
            material.SetTexture(GroundTintTexId, GroundTintTexture);
        }

        material.SetFloat(TintStrengthId, TintStrength);

        Vector4 originSize = new Vector4(
            transform.position.x - AreaSize.x * 0.5f,
            transform.position.z - AreaSize.y * 0.5f,
            AreaSize.x,
            AreaSize.y
        );

        material.SetVector(TintMapOriginSizeId, originSize);
    }

    private void Update()
    {
        if (Player == null && !hasTriedFindPlayer)
        {
            hasTriedFindPlayer = true;

            Player player = FindFirstObjectByType<Player>();
            if (player != null)
            {
                Player = player.transform;
            }
        }

        if (Player != null)
        {
            Vector3 p = Player.position;
            material.SetVector(PlayerPosRadiusId, new Vector4(p.x, p.y, p.z, PlayerBendRadius));
            material.SetFloat(BendStrengthId, BendStrength);
        }

        Graphics.DrawMeshInstancedIndirect(
            mesh,
            0,
            material,
            drawBounds,
            argsBuffer,
            0,
            null,
            ShadowCastingMode.On,
            true
        );
    }

    private void OnDisable()
    {
        matrixBuffer?.Release();
        matrixBuffer = null;

        argsBuffer?.Release();
        argsBuffer = null;
    }
}