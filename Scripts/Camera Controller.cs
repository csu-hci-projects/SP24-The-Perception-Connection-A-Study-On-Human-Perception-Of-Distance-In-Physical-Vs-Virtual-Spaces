using UnityEngine;

public class PortalCameraControl : MonoBehaviour
{
    public GameObject headSphere;  // Reference to the headSphere
    public Transform pa;  // Top left corner of your screen
    public Transform pb;  // Bottom left corner of your screen
    public Transform pc;  // Bottom right corner of your screen

    private Camera _camera;  // Camera attached to this GameObject

    void Start()
    {
        _camera = GetComponent<Camera>();  // Initialize camera
    }

    void Update()
    {
        UpdateFrustrum();  // Update the frustum on every frame
    }

    private void UpdateFrustrum()
    {
        float n = _camera.nearClipPlane;
        float f = _camera.farClipPlane;

        Vector3 pa = this.pa.position;  // Position of pa
        Vector3 pb = this.pb.position;  // Position of pb
        Vector3 pc = this.pc.position;  // Position of pc
        Vector3 pe = headSphere.transform.position;  // Position of headSphere (mapped to eye position)

        // Compute an orthonormal basis for the screen.
        Vector3 vr = (pb - pa).normalized;
        Vector3 vu = (pc - pa).normalized;
        Vector3 vn = Vector3.Cross(vu, vr).normalized;

        // Compute the screen corner vectors.
        Vector3 va = pa - pe;
        Vector3 vb = pb - pe;
        Vector3 vc = pc - pe;

        // Find the distance from the eye to screen plane.
        float d = -Vector3.Dot(va, vn);

        // Find the extent of the perpendicular projection.
        float nd = n / d;
        float l = Vector3.Dot(vr, va) * nd;
        float r = Vector3.Dot(vr, vb) * nd;
        float b = Vector3.Dot(vu, va) * nd;
        float t = Vector3.Dot(vu, vc) * nd;

        // Load the perpendicular projection.
        _camera.projectionMatrix = Matrix4x4.Frustum(l, r, b, t, n, f);
        _camera.transform.rotation = Quaternion.LookRotation(-vn, vu);
    }
}