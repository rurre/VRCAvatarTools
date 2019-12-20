﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pumkin.HelperFunctions;
using Pumkin.AvatarTools;
using Pumkin.DataStructures;
using System;

namespace Pumkin.Presets
{
    [CustomEditor(typeof(PumkinsPosePreset))]
    public class PumkinsPosePresetEditor : Editor
    {
        static string[] defaultMusclesNames;

        bool muscles_expand = false;
        bool transforms_expand = false;

        SerializedObject serializedPosePreset;

        SerializedProperty pName,
            pMuscles,
            pPresetMode,
            pTransformPaths,
            pTransformRotations;              

        PumkinsPosePreset Preset
        {
            get { return (PumkinsPosePreset)target; }
        }

        private void OnEnable()
        {            
            if(defaultMusclesNames == null || defaultMusclesNames.Length == 0)
                defaultMusclesNames = HumanTrait.MuscleName;

            serializedPosePreset = new SerializedObject(Preset);            
            
            pName = serializedPosePreset.FindProperty("name");
            pMuscles = serializedPosePreset.FindProperty("muscles");
            pPresetMode = serializedPosePreset.FindProperty("presetMode");
            pTransformPaths = serializedPosePreset.FindProperty("transformPaths");
            pTransformRotations = serializedPosePreset.FindProperty("transformRotations");            
        }

        void DrawPropertyGUI()
        {
            serializedPosePreset.UpdateIfRequiredOrScript();
               
            EditorGUILayout.PropertyField(pName, new GUIContent(Strings.Preset.name));

            Helpers.DrawGUILine();

            EditorGUILayout.PropertyField(pPresetMode, new GUIContent(Strings.Preset.mode));

            Helpers.DrawGUILine();

            if((PumkinsPosePreset.PosePresetMode)pPresetMode.enumValueIndex == PumkinsPosePreset.PosePresetMode.HumanPose)
            {
                Helpers.DrawPropertyArrayWithNames(pMuscles, Strings.PoseEditor.muscles, defaultMusclesNames, ref muscles_expand, false, 185);
            }
            else
            {
                Helpers.DrawPropertyArraysHorizontalWithDeleteAndAdd(new SerializedProperty[] { pTransformPaths, pTransformRotations }, Strings.PoseEditor.transformRotations, ref transforms_expand);
            }            

            Helpers.DrawGUILine();

            if(GUILayout.Button(Strings.Buttons.selectInToolsWindow))
            {
                PumkinsPresetManager.SelectPresetInToolWindow(Preset);
            }

            serializedPosePreset.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            DrawPropertyGUI();
        }
    }
}