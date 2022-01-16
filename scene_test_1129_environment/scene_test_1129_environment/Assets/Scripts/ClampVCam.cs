using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that clamps target disatance
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class ClampVcamDistance : CinemachineExtension
{
    [Tooltip("Clamp the vcam's distance from the target to at least this amount")]
    public float m_MinDistance = 10;

    private void OnValidate()
    {
        m_MinDistance = Mathf.Max(0, m_MinDistance);
    }

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body && VirtualCamera.Follow != null)
        {
            var dir = state.RawPosition - VirtualCamera.Follow.position;
            float d = dir.magnitude;
            if (d < m_MinDistance)
                state.RawPosition = VirtualCamera.Follow.position + dir.normalized * m_MinDistance;
        }
    }
}