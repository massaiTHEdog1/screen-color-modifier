﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ScreenTemperature.Classes;
using ScreenTemperature.Services.Interfaces;

namespace ScreenTemperature.Services
{
    class MonitorService : IMonitorService
    {
        #region Variables

        private List<Monitor> _monitors;

        #endregion

        #region DLLs

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        #endregion

        #region Constructor

        //public MonitorService()
        //{

        //}

        #endregion

        #region Methods

        public List<Monitor> GetMonitors()
        {
            if (_monitors == null)
            {
                _monitors = new List<Monitor>();

                IntPtr hdc = CreateDC("DISPLAY", null, null, IntPtr.Zero);

                _monitors.Add(new Monitor()
                {
                    Name = "All screens",
                    Hdc = hdc
                });

                foreach (Screen screen in Screen.AllScreens)
                {
                    hdc = CreateDC(screen.DeviceName, null, null, IntPtr.Zero);

                    _monitors.Add(new Monitor()
                    {
                        Name = screen.DeviceName,
                        Hdc = hdc
                    });
                }
            }

            return _monitors;
        }

        public Monitor GetAllMonitorsInOne()
        {
            return GetMonitors()[0];
        }

        public List<Monitor> GetMonitorsExceptAllMonitorsInOne()
        {
            return GetMonitors().Skip(1).ToList();
        }

        public IntPtr GetHdcByMonitorName(string monitorName)
        {
            Monitor monitor = GetMonitors().FirstOrDefault(x => x.Name == monitorName);

            if (monitor == null)
            {
                return IntPtr.Zero;
            }
            else
            {
                return monitor.Hdc;
            }
        }

        #endregion
    }
}