using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace crewlink.memoryreader
{
    // Original Code from AmongUsCapture
    // https://github.com/
    public abstract class Memory
    {
        protected bool is64Bit;
        public Process process;
        public List<Module> modules;
        public bool IsHooked { get; set; }

        public bool HookProcess(string name)
        {
            if (!IsHooked)
            {
                Process[] processes = Process.GetProcessesByName(name);
                if (processes.Length > 0)
                {
                    process = processes[0];
                    if (process != null && !process.HasExited)
                    {
                        bool flag;
                        Win32.IsWow64Process(process.Handle, out flag);
                        is64Bit = Environment.Is64BitOperatingSystem && !flag;
                        LoadModules();
                    }
                }
            }

            IsHooked = process != null && !process.HasExited;
            return IsHooked;
        }
        
        private void LoadModules()
        {
            modules = new List<Module>();
            IntPtr[] buffer = new IntPtr[1024];
            uint cb = (uint) (IntPtr.Size * buffer.Length);
            if (Win32.EnumProcessModulesEx(process.Handle, buffer, cb, out uint totalModules, 3u))
            {
                uint moduleSize = totalModules / (uint) IntPtr.Size;
                StringBuilder stringBuilder = new StringBuilder(260);
                for (uint count = 0; count < moduleSize; count++)
                {
                    stringBuilder.Clear();
                    if (Win32.GetModuleFileNameEx(process.Handle, buffer[count], stringBuilder,
                        (uint) stringBuilder.Capacity) == 0u)
                        break;
                    string fileName = stringBuilder.ToString();
                    stringBuilder.Clear();
                    if (Win32.GetModuleBaseName(process.Handle, buffer[count], stringBuilder,
                        (uint) stringBuilder.Capacity) == 0u) 
                        break;
                    string moduleName = stringBuilder.ToString();
                    ModuleInfo moduleInfo = default;
                    if (!Win32.GetModuleInformation(process.Handle, buffer[count], out moduleInfo,
                        (uint) Marshal.SizeOf(moduleInfo))) 
                        break;
                    modules.Add(new Module
                    {
                        FileName = fileName,
                        BaseAddress = moduleInfo.BaseAddress,
                        EntryPointAddress = moduleInfo.EntryPoint,
                        MemorySize = moduleInfo.ModuleSize,
                        Name = moduleName
                    });
                }
            }
        }
        public class Module
        {
            public IntPtr BaseAddress { get; set; }
            public IntPtr EntryPointAddress { get; set; }
            public string FileName { get; set; }
            public uint MemorySize { get; set; }
            public string Name { get; set; }
            public FileVersionInfo FileVersionInfo => FileVersionInfo.GetVersionInfo(FileName);
            public override string ToString()
            {
                return Name ?? base.ToString();
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        protected struct ModuleInfo
        {
            public IntPtr BaseAddress;
            public uint ModuleSize;
            public IntPtr EntryPoint;
        }
        private static class Win32
        {
            public const int PROCESS_CREATE_THREAD = 2;
            public const int PROCESS_VM_OPERATION = 8;
            public const int PROCESS_VM_WRITE = 32;
            public const int PROCESS_VM_READ = 16;
            public const int PROCESS_QUERY_INFORMATION = 1024;
            public const uint PAGE_READWRITE = 4;
            public const uint MEM_COMMIT = 4096;
            public const uint MEM_RESERVE = 8192;

            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(
                int dwDesiredAccess,
                bool bInheritHandle,
                int dwProcessId);
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetModuleHandleA(string lpModuleName);

            [DllImport("kernel32.dll")]
            public static extern bool ReadProcessMemory(
                IntPtr hProcess,
                IntPtr lpBaseAddress,
                byte[] lpBuffer,
                int nSize,
                out IntPtr lpNumberOfBytesRead);

            [DllImport("kernel32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWow64Process(IntPtr hProcess,
                [MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumProcessModulesEx(IntPtr hProcess, [Out] IntPtr[] lphModule, uint cb,
                out uint lpcbNeeded, uint dwFilterFlag);

            [DllImport("psapi.dll", SetLastError = true)]
            public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName,
                uint nSize);

            [DllImport("psapi.dll")]
            public static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName,
                uint nSize);

            [DllImport("psapi.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out ModuleInfo lpmodinfo,
                uint cb);
        }
    }
}