using System;
using RentNest.Core.Enums;

namespace RentNest.Core.UtilHelper
{
    public static class PaymentStatusHelper
    {
        public static string ToDbValue(this PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "P",
                PaymentStatus.Completed => "C",
                PaymentStatus.Refuned => "R",
                PaymentStatus.Inactive => "I",
                _ => throw new ArgumentOutOfRangeException(nameof(status), $"Unsupported payment status: {status}")
            };
        }

        public static PaymentStatus FromDbValue(string value)
        {
            return value switch
            {
                "P" => PaymentStatus.Pending,
                "C" => PaymentStatus.Completed,
                "R" => PaymentStatus.Refuned,
                "I" => PaymentStatus.Inactive,
                _ => throw new ArgumentOutOfRangeException(nameof(value), $"Unsupported payment status value: {value}")
            };
        }
    }
}
