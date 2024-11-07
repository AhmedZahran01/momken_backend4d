using System.Runtime.Serialization;

namespace momken_backend.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        pending,

        [EnumMember(Value = "Payment Successed")]
        paymentSuccessed,

        [EnumMember(Value = "Payment Failed")]
        paymentFailed

    }
}
