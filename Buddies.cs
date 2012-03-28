using System;
using System.Collections.Generic;
using System.Text;

namespace Gk.LibIBuddy
{
    public class Buddies
    {
        enum VendorID { TenxTechnology = 0x1130 }
        enum TenxTechDeviceID { iBuddy1 = 0x0001, iBuddy2 = 0x0002 }

        Dictionary<string, iBuddy> _buddies = new Dictionary<string,iBuddy>();
        public int LastWin32Error = 0;

        private Color _color = Color.Off;
        
        public void RefreshList()
        {
            LastWin32Error = 0;

            UpdateColor();  // update IsAlive

            lock (_buddies)
            {
                var keys = new string[_buddies.Keys.Count];
                _buddies.Keys.CopyTo(keys, 0);
                foreach (var Key in keys)
                {
                    var buddy = _buddies[Key];
                    if (buddy.LastWin32Error != 0)
                    {
                        LastWin32Error = buddy.LastWin32Error;
                    }
                    if (!buddy.IsAlive)
                    {
                        buddy.Release();
                        _buddies.Remove(Key);
                    }
                }
            }

            List<string> devicePaths = new List<string>();
            devicePaths.AddRange(HIDDevices.Find((int)VendorID.TenxTechnology, (int)TenxTechDeviceID.iBuddy1));
            devicePaths.AddRange(HIDDevices.Find((int)VendorID.TenxTechnology, (int)TenxTechDeviceID.iBuddy2));
            foreach (var devicePath in devicePaths)
            {
                if (!_buddies.ContainsKey(devicePath))
                {
                    var buddy = new iBuddy();
                    if (buddy.Init(devicePath))
                    {
                        lock (_buddies)
                        {
                            _buddies.Add(devicePath, buddy);
                        }
                    }
                }
            }

        }

        public int Count { get { return _buddies.Count; } }

        public void Release()
        {
            lock (_buddies)
            {
                foreach (var buddy in _buddies.Values)
                {
                    buddy.Release();
                }
                _buddies.Clear();
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                UpdateColor();
            }
        }

        void UpdateColor()
        {
            lock (_buddies)
            {
                foreach (var buddy in _buddies.Values)
                {
                    buddy.SetColor(_color);
                }
            }
        }

        public void Dance(uint TimeMs = 5000)
        {
            lock (_buddies)
            {
                foreach (var buddy in _buddies.Values)
                {
                    buddy.Dance(TimeMs);
                }
            }
        }

        public void FlapWings(uint TimeMs = 5000)
        {
            lock (_buddies)
            {
                foreach (var buddy in _buddies.Values)
                {
                    buddy.FlapWings(TimeMs);
                }
            }
        }

        public void Heartbeat(uint TimeMs = 5000)
        {
            lock (_buddies)
            {
                foreach (var buddy in _buddies.Values)
                {
                    buddy.Heartbeat(TimeMs);
                }
            }
        }

        public void Rainbow(uint TimeMs = 3000)
        {
            lock (_buddies)
            {
                foreach (var buddy in _buddies.Values)
                {
                    buddy.Rainbow(TimeMs);
                }
            }
        }
    }
}
