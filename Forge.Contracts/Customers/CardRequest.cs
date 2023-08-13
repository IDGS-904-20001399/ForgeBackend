using System;
using System.ComponentModel.DataAnnotations;

namespace Forge.Contracts.Customers
{
    public record CardRequest(
        // [property: Required][property: CreditCard] 
        string CardNumber, 
        //  [property: Required][property: DataType(DataType.Date)][property: DisplayFormat(DataFormatString = "{0:MM/yy}", ApplyFormatInEditMode = true)] 
        string ExpirationDate, 
        // [property: Required][property: RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CVV")] 
        string CVV
        );
}