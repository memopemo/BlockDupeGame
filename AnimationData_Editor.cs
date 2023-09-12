using UnityEditor;
using Animator2D;
using UnityEngine;

[CustomEditor(typeof(AnimationData))]
public class AnimationData_Editor: Editor
{
    string subFrameSetAll = "0";
    AnimationData animationData;
    private void OnEnable()
    {
        animationData = target as AnimationData;
    }
    public override void OnInspectorGUI()
    {
        
        subFrameSetAll = GUILayout.TextField(subFrameSetAll);
        
        if(GUILayout.Button("Set All Sub-Frame Lengths"))
        {
            if(int.TryParse(subFrameSetAll,out int i))
            {
                foreach (var frame in animationData.AnimationFrames)
                {
                    frame.HoldForTicks = i;
                }
            }
        }

        GUILayout.Space(10);
        
        base.OnInspectorGUI();

        GUILayout.Space(10);

        GUILayout.Label("Sprite Previews:");
        foreach (var frame in animationData.AnimationFrames)
        {
            GUILayout.BeginHorizontal();
            if (frame.SpriteShown == null) 
                continue;
            frame.SpriteShown = EditorGUILayout.ObjectField(frame.SpriteShown, typeof(Sprite), true, GUILayout.Height(64), GUILayout.Width(64)) as Sprite;
            GUILayout.Label("Sub-Frames Held For:");
            frame.HoldForTicks = EditorGUILayout.IntField(frame.HoldForTicks);
            GUILayout.EndHorizontal();
            

        }
        
    }
}