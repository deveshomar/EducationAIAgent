using OpenAI.Chat;

var apiKey = "";
//string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
  //  ?? throw new Exception("OPENAI_API_KEY not found");

var client = new ChatClient(
    model: "gpt-5-mini",
    apiKey: apiKey);

string imagePath = @"D:\Sessions\Education\Session5\ExtractSalesfromImages\Sales.png";

byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

var messages = new List<ChatMessage>
{
    new SystemChatMessage(
        """
        You are an invoice data extraction system.

        Analyze the provided invoice image and extract the invoice information into the exact JSON structure defined below.

        IMPORTANT RULES:

        1. Extract data only from the invoice image.
        2. Do NOT invent, guess, or infer missing information.
        3. If a field is not visible or cannot be read confidently, return null.
        4. Preserve the values exactly as shown on the invoice.
        5. Do NOT correct mathematical errors in the invoice.
        6. Do NOT recalculate subtotal, discount, tax, or grand total.
        7. The extracted value must represent the value printed on the invoice.
        8. For monetary values, return numbers without currency symbols or commas.
        9. For quantities, return numeric values.
        10. Preserve invoice numbers, PO numbers, GSTINs, HSN/SAC codes, IFSC codes, account numbers, and phone numbers as strings.
        11. Extract every line item visible in the invoice.
        12. Do not omit line items.
        13. Preserve the line-item serial number from the invoice.
        14. Preserve the product description including Make and Model when available.
        15. Return valid JSON only. Do not include markdown, explanations, comments, or additional properties.
        16. The JSON property names must exactly match the schema below.

        EXPECTED JSON STRUCTURE:

        {
        "seller": {
        "name": null,
        "address": null,
        "gstin": null,
        "phone": null,
        "email": null,
        "website": null
        },
        "bill_to": {
        "name": null,
        "address": null,
        "gstin": null,
        "phone": null,
        "email": null,
        "website": null
        },
        "ship_to": {
        "name": null,
        "address": null,
        "gstin": null,
        "phone": null,
        "email": null,
        "website": null
        },
        "invoice_number": null,
        "invoice_date": null,
        "due_date": null,
        "customer_po": null,
        "sales_representative": null,
        "line_items": [
        {
        "s_no": null,
        "description": null,
        "hsn_sac": null,
        "quantity": null,
        "unit": null,
        "unit_price": null,
        "discount_percent": null,
        "amount": null
        }
        ],
        "subtotal": null,
        "discount": null,
        "taxable_amount": null,
        "sgst": null,
        "cgst": null,
        "grand_total": null,
        "payment_terms": [],
        "bank_details": {
        "bank_name": null,
        "account_name": null,
        "account_number": null,
        "ifsc": null,
        "branch": null
        },
        "notes": []
        }

        FIELD EXTRACTION RULES:

        SELLER:
        Extract the complete seller/company information from the invoice header.

        BILL TO:
        Extract the customer/buyer information from the "BILL TO" section.

        SHIP TO:
        Extract the delivery/ship-to information from the "SHIP TO" section.

        INVOICE INFORMATION:
        Extract:

        * Invoice number
        * Invoice date
        * Due date
        * Customer PO
        * Sales representative

        LINE ITEMS:
        For every product row, extract:

        * Serial number
        * Complete product description
        * HSN/SAC
        * Quantity
        * Unit
        * Unit price
        * Discount percentage
        * Printed line-item amount

        Do not calculate the line-item amount yourself. Extract the amount printed in the invoice.

        TOTALS:
        Extract the values exactly as printed:

        * Subtotal
        * Discount
        * Taxable amount
        * SGST
        * CGST
        * Grand total

        Do not recalculate these values.

        PAYMENT TERMS:
        Extract every payment-term statement as a separate string in the payment_terms array.

        BANK DETAILS:
        Extract the bank information exactly as printed.

        NOTES:
        Extract every note/remark from the invoice as a separate string.

        FINAL REQUIREMENT:

        Return ONLY the JSON object matching the specified structure.

        After extraction, do not explain whether the invoice is mathematically correct or incorrect. Mathematical and business validation will be performed separately by a C# validation engine.
        
        """),

    new UserChatMessage(
        ChatMessageContentPart.CreateTextPart(
            """
            Extract:

            - Seller
            - Bill To
            - Ship To
            - Invoice number
            - Invoice date
            - Due date
            - Customer PO
            - Sales representative
            - Line items
            - Subtotal
            - Discount
            - Taxable amount
            - SGST
            - CGST
            - Grand total
            - Payment terms
            - Bank details
            - Notes
            """),

        ChatMessageContentPart.CreateImagePart(
            BinaryData.FromBytes(imageBytes),
            "image/png",
            ChatImageDetailLevel.High))
};

ChatCompletion completion =
    await client.CompleteChatAsync(messages);

string json = completion.Content[0].Text;

Console.WriteLine($"Input tokens  : {completion.Usage.InputTokenCount}");
Console.WriteLine($"Output tokens : {completion.Usage.OutputTokenCount}");
Console.WriteLine($"Total tokens  : {completion.Usage.TotalTokenCount}");

Console.WriteLine(json);