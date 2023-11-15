using UnityEditor;
using UnityEngine;

namespace Horror
{
    [CustomEditor(typeof(ClickToPlaceHelper))]
    public class ClickToPlaceHelperEditor : Editor
    {
        private ClickToPlaceHelper _clickHelper => target as ClickToPlaceHelper;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("클릭한 위치로 Object 배치") && !_clickHelper.IsTargeting)
            {
                _clickHelper.BeginTargeting();
                SceneView.duringSceneGui += DuringSceneGui;
            }
        }

        private void DuringSceneGui(SceneView sceneView)
        {
            Event currentGUIEvent = Event.current;

            Vector3 mousePos = currentGUIEvent.mousePosition;
            float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = sceneView.camera.pixelHeight - mousePos.y * pixelsPerPoint;
            mousePos.x *= pixelsPerPoint;

            Ray ray = sceneView.camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _clickHelper.UpdateTargeting(hit.point);
            }

            switch (currentGUIEvent.type)
            {
                case EventType.MouseMove:
                    HandleUtility.Repaint();
                    break;
                case EventType.MouseDown:
                    if (currentGUIEvent.button == 0) // 좌클릭
                    {
                        _clickHelper.EndTargeting();
                        SceneView.duringSceneGui -= DuringSceneGui;
                        currentGUIEvent.Use();
                    }
                    break;
            }
        }
    }
}
