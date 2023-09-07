using vita.Editions.Dto;

namespace vita.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < vitaConsts.MinimumUpgradePaymentAmount;
        }
    }
}
