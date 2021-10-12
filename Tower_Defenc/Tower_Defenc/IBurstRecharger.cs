namespace Tower_Defenc
{
    public interface IRecharger
    {
        bool CheckCharge();
        void ReCharge();
        void Reset();
        int ChargeReady { get; set; }
        int ChargeSpeed { get; set; }
    }
}