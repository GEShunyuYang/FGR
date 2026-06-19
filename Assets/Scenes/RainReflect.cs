using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class RainReflect : MonoBehaviour
{
    [SerializeField] private Camera reflectionCamera;

    [SerializeField] private Transform reflectionPlane;

    [SerializeField] private RenderTexture reflectionTexture;

    [SerializeField] private LayerMask reflectionMask;

    [SerializeField] private float resolutionScale = 0.5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        reflectionCamera.enabled = false;
        CreateReflectionTexture();
    }

    private void CreateReflectionTexture()
    {
        int width = Mathf.RoundToInt(Screen.width * resolutionScale);
        int height = Mathf.RoundToInt(Screen.height * resolutionScale);

        reflectionTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        reflectionTexture.name = "PlanarReflectionRT";
        reflectionTexture.wrapMode = TextureWrapMode.Clamp;
        reflectionTexture.filterMode = FilterMode.Bilinear;
        reflectionTexture.Create();

        reflectionCamera.targetTexture = reflectionTexture;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = mainCamera.transform.position;
        pos.y = reflectionPlane.position.y - (pos.y - reflectionPlane.position.y);

        Vector3 forward = Vector3.Reflect(mainCamera.transform.forward, Vector3.up);
        Vector3 up = Vector3.Reflect(mainCamera.transform.up, Vector3.up);

        reflectionCamera.transform.SetPositionAndRotation(
            pos,
            Quaternion.LookRotation(forward, up)
        );

        reflectionCamera.targetTexture = reflectionTexture;
        reflectionCamera.cullingMask = reflectionMask;

        reflectionCamera.Render();

        Matrix4x4 vp =
            reflectionCamera.projectionMatrix *
            reflectionCamera.worldToCameraMatrix;

        Shader.SetGlobalTexture("_PlanarReflectionTex", reflectionTexture);
        Shader.SetGlobalMatrix("_PlanarReflectionVP", vp);
    }
}
