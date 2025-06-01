using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentNest.Core.Model.PayOS
{
    public record Response(
    int error,
    String message,
    object? data
);
}