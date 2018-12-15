﻿//-----------------------------------------------------------------------
// <copyright file="EnvironmentalLight.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
namespace HuaweiARUnitySDK
{

    using UnityEngine;
    using HuaweiARInternal;

    [ExecuteInEditMode]
    public class AREnvironmentalLight : MonoBehaviour
    {
        void Update()
        {
#if UNITY_EDITOR
            Shader.SetGlobalFloat("_GlobalLightEstimation", 1.0f);
#else
            ARLightEstimate light = ARFrame.GetLightEstimate();
			if(!light.Valid){
				return;
			}
			const float middleGray = 0.466f;

			float normalizedIntensity = light.PixelIntensity / middleGray;
			Shader.SetGlobalFloat("_GlobalLightEstimation", normalizedIntensity);
#endif
        }
    }
}