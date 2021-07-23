using System.Collections.Generic;
using Aarthificial.Reanimation;
using TheKiwiCoder;
using TN.Extensions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;


public class ReanimatorGraphEditor : EditorWindow
{
    [MenuItem("Reanimator/Resolution Graph")]
    public static void ShowWindow()
    {
        ReanimatorGraphEditor wnd = GetWindow<ReanimatorGraphEditor>();
        wnd.titleContent = new GUIContent("ReanimatorGraph");
        wnd.minSize = new Vector2(1200, 500);
    }

    private ReanimatorGraphView graphView;
    private InspectorView inspectorView;


    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uxml");
        visualTree.CloneTree(root);
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Reanimator/Editor/ResolutionGraph/ReanimatorGraphEditor.uss");
        root.styleSheets.Add(styleSheet);
        
        graphView = root.Q<ReanimatorGraphView>();
        inspectorView = root.Q<InspectorView>();


        root.RegisterCallback<MouseDownEvent>(evt => { });

        root.RegisterCallback<DragExitedEvent>(evt => {
            List<ScriptableObject> objs = new List<ScriptableObject>();
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

            if (DragAndDrop.objectReferences == null) return;

            var refs = DragAndDrop.objectReferences;
            refs.ForEach(obj => {
                if (obj is ScriptableObject scriptableObject)
                    objs.Add(scriptableObject);
            });

            objs.ForEach(x => Debug.Log(x.GetType().Name));
        });
        
        OnSelectionChange();
    }
    private void OnSelectionChange()
    {
        ResolutionGraph graph = Selection.activeObject as ResolutionGraph;
        if (graph) {
            graphView.PopulateView(graph);
        }
    }
}