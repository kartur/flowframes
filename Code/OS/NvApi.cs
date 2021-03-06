﻿using NvAPIWrapper;
using NvAPIWrapper.GPU;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flowframes.OS
{
    class NvApi
    {
        static PhysicalGPU gpu;
        static float vramGb;
        static float vramFreeGb;

        public static async void Init()
        {
            try
            {
                NVIDIA.Initialize();
                PhysicalGPU[] gpus = PhysicalGPU.GetPhysicalGPUs();
                if (gpus.Length == 0)
                    return;
                gpu = gpus[0];

                Logger.Log("Init NvApi");
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to initialize NvApi: {e.Message}\nIgnore this if you don't have an Nvidia GPU.");
            }
        }

        public static void RefreshVram()
        {
            if (Form.ActiveForm != Program.mainForm || gpu == null)    // Don't refresh if not in focus or no GPU detected
                return;
            vramGb = (gpu.MemoryInformation.AvailableDedicatedVideoMemoryInkB / 1000f / 1024f);
            vramFreeGb = (gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB / 1000f / 1024f);
            Color col = Color.White;
            if (vramFreeGb < 2f)
                col = Color.Orange;
            if (vramFreeGb < 1f)
                col = Color.OrangeRed;
            //Program.mainForm.SetVramLabel($"{gpu.FullName}: {vramGb.ToString("0.00")} GB VRAM - {vramFreeGb.ToString("0.00")} GB Free", col);
        }

        public static float GetVramGb ()
        {
            try
            {
                return (gpu.MemoryInformation.AvailableDedicatedVideoMemoryInkB / 1000f / 1024f);
            }
            catch (Exception e)
            {
                return 0f;
            }
        }

        public static float GetFreeVramGb()
        {
            try
            {
                return (gpu.MemoryInformation.CurrentAvailableDedicatedVideoMemoryInkB / 1000f / 1024f);
            }
            catch (Exception e)
            {
                return 0f;
            }
        }

        public static string GetGpuName()
        {
            try
            {
                NVIDIA.Initialize();
                PhysicalGPU[] gpus = PhysicalGPU.GetPhysicalGPUs();
                if (gpus.Length == 0)
                    return "";

                return gpus[0].FullName;
            }
            catch
            {
                return "";
            }
        }
    }
}
