using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defenc
{

    class SimpleRechargen : IRecharger
    {
        int ChargeRate;
        public int ChargeReady { get; set; }
        public int ChargeSpeed { get; set; }

        public void ReCharge ()
        {
            ChargeRate += ChargeSpeed;
        }

        public bool CheckCharge()
        {
            if(ChargeRate >= ChargeReady)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Reset()
        {
            ChargeRate = 0;
        }
    }
}
