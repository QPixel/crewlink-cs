using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crewlink.memoryreader
{
    public class GameOffsets
    {
        public int AmongUsClientOffset { get; set; }
        
        public int GameDataOffset { get; set; }
        
        public int MeetingHudOffset { get; set; }
        
        public int GameStartManagerOffset { get; set; }
        
        public int HudManagerOffset { get; set; }
        
        public int ServerManagerOffset { get; set; }
        
        public int TempDataOffset { get; set; }
        
        public int GameOptionsOffset { get; set; }
        
        public int[] MeetingHudPtr { get; set; }
        public int[] MeetingHudCachePtrOffsets { get; set; }
        public int[] GameStateOffsets { get; set; }
        public int[] AllPlayerPtrOffsets { get; set; }
        public int[] AllPlayersOffsets { get; set; }
        public int[] PlayerCountOffsets { get; set; }
        public int[] ExiledPlayerIdOffsets { get; set; }
        public int[] RawGameOverReasonOffsets { get; set; }
        public int[] WinningPlayersPtrOffsets { get; set; }
        public int[] WinningPlayersOffsets { get; set; }
        public int[] WinningPlayerCountOffsets { get; set; }
        public int[] GameCodeOffsets { get; set; }
        public int[] PlayRegionOffsets { get; set; }
        public int[] PlayMapOffsets { get; set; }
        public int[] StringOffsets { get; set; }
    }
}
