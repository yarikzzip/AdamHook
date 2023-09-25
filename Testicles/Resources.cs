using Memory.Utils;
using Memory.Win64;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testicles
{
    public class Resources
    {
        static MemoryHelper64 helper;
        static int stateAddr;
        public static void ResourceChange()
        {
            string[] resources = { "Oil", "Aluminum", "Rubber", "Tungsten", "Steel", "Chromium" };
            int[] resourcecount = { 1000, 1000, 1000, 1000, 1000, 1000 };
            int state;

            state = 68;
            int resource = getResourceCount(state);
            if (resource > 0)
            {
                for(int i = 0; i < 6; i++)
                {
                    stateAddr = resourcecount[i];
                }
            }
            else
            {
                Console.WriteLine("[AdamHook DEBUG] Cannot add resources to state with none");
            }

                    

        }

        public static int getResourceCount(int state) 
        {
            ulong baseStateaddr = helper.GetBaseAddress(0x25E9D00);
            int[] offsetTS = { state };
            ulong targetStateAddr = MemoryUtils.OffsetCalculator(helper, baseStateaddr, offsetTS);
            int stateResource = 0;
            
            if (helper.ReadMemory<Int32>(targetStateAddr) > 0)
            {
                stateResource = 1;
                return stateResource;
            }
            return stateResource;

            
        }
    }
}
