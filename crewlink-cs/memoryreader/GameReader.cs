using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace crewlink.memoryreader
{

    public enum GameState
    {
        LOBBY,
        TASKS,
        DISCUSSION,
        MENU,
        ENDED,
        UNKOWN
    }
    class GameReader
    {
        private ProcessMemory _processMemory;
        private IntPtr GameAssemblyPtr = IntPtr.Zero;
        private ProcessMemory.Module gameassemblyModule;
        private GameOffsets currentOffsets;
        public void test()
        {
            const string gameDataSig = "48 8B 05 ? ? ? ? 48 8B 88 ? ? ? ? 48 8B 01 48 85 C0 0F 84 ? ? ? ? BE ? ? ? ?";
            _processMemory = ProcessMemory.getInstance();
            _processMemory.HookProcess("Among Us");

            Debug.WriteLine("Attached to AmongUs Process");
            var foundModule = false;
            while (true)
            {
                foreach (var module in  _processMemory.modules)
                {
                    if (module.Name.Equals("GameAssembly.dll", StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.WriteLine("[GameReader] Found GameAssembly.Dll");
                        GameAssemblyPtr = module.BaseAddress;
                        gameassemblyModule = module;
                        foundModule = true;
                        break;
                    }
                }

                if (!foundModule)
                {
                    Debug.Write("[GameReader] Still looking for modules");
                    Thread.Sleep(500);
                    _processMemory.LoadModules();
                }
                else
                {
                    break; // we found all modules woooo
                }  
            }

            //ModuleHandle test = (ModuleHandle) AmongUsProcessHandle;
           // module = .Modules.Cast<ProcessModule>().SingleOrDefault(m => string.Equals(m.ModuleName, "GameAssembly.dll", StringComparison.OrdinalIgnoreCase));

            Debug.WriteLine($"[GameReader] GameAssembly BaseAddress: {gameassemblyModule.BaseAddress.ToInt32().ToString("X")}");
            // Debug.WriteLine($"GameAssembly BaseAddress: {gameassemblyModule.EntryPointAddress.ToInt32().ToString("X")}");
            Debug.WriteLine($"[GameReader] GameAssembly Size: {gameassemblyModule.MemorySize}, {gameassemblyModule.MemorySize.ToString("X")}");
            ulong instructionLocation = _processMemory.FindPattern(gameassemblyModule, gameDataSig);
            Debug.WriteLine($"AMONGUS gameDATA instructionLocation: {instructionLocation}");
            
        }
        /* private UIntPtr FindPattern(IntPtr handle, ProcessModule processModule, string pattern, short sigType, byte patternOffset, byte addressOffset)
         {
             IntPtr bytesRead;
             var moduleSize = module.ModuleMemorySize;
             var moduleBase = module.BaseAddress;
             byte[] ModuleBytes = new byte[module.ModuleMemorySize];
             Win32.ReadProcessMemory(handle, moduleBase, ModuleBytes, moduleSize, out bytesRead);
 
             var byteBase = ModuleBytes[0];
             ulong maxOffset = (ulong) moduleSize - 0x1000;
 
             for (var offset = (ulong)0; offset < maxOffset; offset++)
             {
                 if
             }
         }
         private bool CompareBytes(byte[] bytes, string pattern)
         {
             for (pattern; pattern != " " ? bytes++ : bytes, pattern++)
             {
 
             }
         }
        */
    }
}
