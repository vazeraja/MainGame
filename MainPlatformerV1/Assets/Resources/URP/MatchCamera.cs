using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MainGame {

    [RequireComponent(typeof(Camera)), ExecuteAlways]
    public class MatchCamera : MonoBehaviour {
        public Camera camera;

        private Camera thisCamera;
        private UniversalAdditionalCameraData camData;
        private UniversalAdditionalCameraData _thisCamData;

        private void OnEnable(){
            if (!camera) return;

            camera.TryGetComponent(out camData);
            TryGetComponent(out thisCamera);
            TryGetComponent(out _thisCamData);
        }

        private void LateUpdate(){
            if (!camera || !camData || !thisCamera || !_thisCamData) return;

            thisCamera.projectionMatrix = camera.projectionMatrix;
        }
    }
}
