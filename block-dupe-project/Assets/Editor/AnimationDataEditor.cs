using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Animator2D;
using Unity.VisualScripting;

[CustomEditor(typeof(AnimationData))]
[CanEditMultipleObjects]

public class AnimationDataEditor : Editor
{
    string subFrameSetAll = "0";
    AnimationData animData;
    public override bool HasPreviewGUI() => true;
    
    SerializedObject frames;
    SerializedProperty frameProperty;
    int spriteIndex = 0;

    private void OnEnable()
    {
        animData = target as AnimationData;
        frameProperty = serializedObject.FindProperty("AnimationFrames");
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        spriteIndex += 1;
        spriteIndex %= animData.AnimationFrames.Count*2;
        //Crop sprite into Texture.
        Sprite _spriteFrame = animData.AnimationFrames[spriteIndex/2].SpriteShown;
        Texture2D _croppedTexture = new Texture2D((int)_spriteFrame.rect.width, (int)_spriteFrame.rect.height);
        var _pixels = _spriteFrame.texture.GetPixels(
            (int)_spriteFrame.rect.x,
            (int)_spriteFrame.rect.y,
            (int)_spriteFrame.rect.width,
            (int)_spriteFrame.rect.height
        );
        _croppedTexture.SetPixels(_pixels);
        _croppedTexture.Apply();
        _croppedTexture.filterMode = FilterMode.Point;

        GUI.DrawTexture(r, _croppedTexture,ScaleMode.ScaleToFit);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        subFrameSetAll = GUILayout.TextField(subFrameSetAll);
        
        if(GUILayout.Button("Set All Tick Lenghts"))
        {
            
            if(int.TryParse(subFrameSetAll, out int numberToSet))
            {
                for (int index = 0; index < frameProperty.arraySize; index++)
                {
                    
                    frameProperty.GetArrayElementAtIndex(index).FindPropertyRelative("HoldForTicks").SetUnderlyingValue(numberToSet);
                }
            }
        }

        GUILayout.Space(10);
        base.OnInspectorGUI();
        GUILayout.Space(10);

        GUILayout.Label("Sprite Previews:");
        for (int index = 0; index < frameProperty.arraySize; index ++)
        {
            //Fuck unity.
            frameProperty.GetArrayElementAtIndex(index).FindPropertyRelative("SpriteShown").SetUnderlyingValue(EditorGUILayout.ObjectField(frameProperty.GetArrayElementAtIndex(index).FindPropertyRelative("SpriteShown").objectReferenceValue, typeof(Sprite), true, GUILayout.Height(64), GUILayout.Width(64)) as Sprite);
        }
        frameProperty.serializedObject.ApplyModifiedProperties();

    }

}
