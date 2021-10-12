namespace Tower_Defenc
{
    class BurstRecharger : IRecharger
    {
        int ChargeRate;
        int burstMax;
        int burstCount;
        int mode;
        public int ChargeReady { get; set; }
        public int ChargeSpeed { get; set; }

        public void ReCharge()
        {
            if (mode == 0)
            {
                // долгая перезарядка
                ChargeRate += ChargeSpeed;
                if(ChargeRate >= ChargeReady)
                {
                    mode = 1;
                    burstCount = burstMax;
                }
            }
            else
            {
                ChargeRate += ChargeSpeed * 999;
            }
            // Хитрая перезарядка для стрельбы очередью
        }
        public BurstRecharger(int burst)
        {
            burstMax = burst;
            burstCount = burst;
        }

        public bool CheckCharge()
        {
            if (ChargeRate >= ChargeReady)
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
            if(mode == 1)
            {
                burstCount--;
                if(burstCount == 0)
                {
                    mode = 0;

                }
            }
        }
    }
}
