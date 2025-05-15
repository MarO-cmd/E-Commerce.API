using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Store.Maro.Core.Entities.Orders
{
    public enum OrderStatus
    {
        // is used to specify how the enum values should be serialized or deserialized, usually when working with JSON or XML.
        [EnumMember(Value ="Pending")]
        Pending,

        [EnumMember(Value = "Payment Received")]
        PaymentReceived,

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}
