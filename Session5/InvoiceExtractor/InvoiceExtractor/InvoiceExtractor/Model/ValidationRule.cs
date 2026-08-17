using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceExtractor.Model
{
    public class ValidationRule
    {
     public   static InvoiceValidationResult ValidateInvoice(Invoice invoice)
        {
            var result = new InvoiceValidationResult();

            // Rule 1: Validate each line item
            foreach (var item in invoice.Items)
            {
                decimal expectedAmount =
                    item.Quantity * item.UnitCost;

                if (expectedAmount != item.Amount)
                {
                    result.Errors.Add(
                        $"Product '{item.ProductName}': " +
                        $"Expected amount {expectedAmount}, " +
                        $"but invoice shows {item.Amount}.");
                }
            }

            // Rule 2: Validate subtotal
            decimal calculatedSubTotal =
                invoice.Items.Sum(x => x.Amount);

            if (calculatedSubTotal != invoice.SubTotal)
            {
                result.Errors.Add(
                    $"Subtotal mismatch. " +
                    $"Calculated = {calculatedSubTotal}, " +
                    $"Invoice = {invoice.SubTotal}.");
            }

            // Rule 3: Validate GST
            decimal calculatedGst =
                Math.Round(
                    invoice.SubTotal * invoice.GstPercentage / 100,
                    2);

            if (calculatedGst != invoice.GstAmount)
            {
                result.Errors.Add(
                    $"GST mismatch. " +
                    $"Calculated = {calculatedGst}, " +
                    $"Invoice = {invoice.GstAmount}.");
            }

            // Rule 4: Validate final total
            decimal calculatedTotal =
                invoice.SubTotal + invoice.GstAmount;

            if (calculatedTotal != invoice.TotalAmount)
            {
                result.Errors.Add(
                    $"Total amount mismatch. " +
                    $"Calculated = {calculatedTotal}, " +
                    $"Invoice = {invoice.TotalAmount}.");
            }

            result.IsValid = result.Errors.Count == 0;

            return result;
        }
    }
}
