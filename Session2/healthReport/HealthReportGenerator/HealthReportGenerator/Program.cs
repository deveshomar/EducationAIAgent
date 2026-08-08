// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;

string healthJSON = @"{
  ""patientInfo"": {
    ""patientId"": ""P100245"",
    ""name"": ""Rahul Sharma"",
    ""age"": 42,
    ""gender"": ""Male""
  },
  ""bloodReport"": {
    ""cbc"": {
      ""hemoglobin"": 10.2,
      ""rbc"": 3.8,
      ""wbc"": 3900,
      ""platelets"": 135000,
      ""hematocrit"": 32
    },
    ""lipidProfile"": {
      ""totalCholesterol"": 800,
      ""ldl"": 182,
      ""hdl"": 34,
      ""triglycerides"": 800
    },
    ""sugar"": {
      ""fastingGlucose"": 118,
      ""hba1c"": 6.1
    }
  },
  ""analysis"": {
    ""anemia"": true,
    ""highCholesterol"": true,
    ""prediabetic"": true
  },
  ""dietPlan"": {
    ""breakfast"": [
      ""Oats with skim milk"",
      ""Boiled eggs"",
      ""Green tea""
    ],
    ""lunch"": [
      ""Brown rice"",
      ""Grilled chicken"",
      ""Salad""
    ],
    ""dinner"": [
      ""Vegetable soup"",
      ""Chapati"",
      ""Paneer curry""
    ],
    ""avoid"": [
      ""Fried food"",
      ""Sugary drinks"",
      ""Butter"",
      ""Processed snacks""
    ]
  }
}";

string healthreportData = @"{
  ""reportId"": ""LAB2026-000123"",
  ""patient"": {
    ""patientId"": ""P10045"",
    ""name"": ""Rahul Sharma"",
    ""age"": 34,
    ""gender"": ""Male"",
    ""dateOfBirth"": ""1992-03-15""
  },
  ""testDate"": ""2026-07-28T09:30:00+05:30"",
  ""labName"": ""ABC Diagnostics Pvt Ltd"",
  ""tests"": {
    ""cbc"": {
      ""hemoglobin"": {
        ""value"": 14.2,
        ""unit"": ""g/dL"",
        ""referenceRange"": ""13.0-17.0"",
        ""status"": ""Normal""
      },
      ""wbc"": {
        ""value"": 7200,
        ""unit"": ""cells/uL"",
        ""referenceRange"": ""4000-11000"",
        ""status"": ""Normal""
      },
      ""rbc"": {
        ""value"": 5.1,
        ""unit"": ""million/uL"",
        ""referenceRange"": ""4.5-5.9"",
        ""status"": ""Normal""
      },
      ""platelets"": {
        ""value"": 250000,
        ""unit"": ""cells/uL"",
        ""referenceRange"": ""150000-450000"",
        ""status"": ""Normal""
      },
      ""hematocrit"": {
        ""value"": 42.8,
        ""unit"": ""%"",
        ""referenceRange"": ""40-50"",
        ""status"": ""Normal""
      }
    },
    ""bloodSugar"": {
      ""fastingGlucose"": {
        ""value"": 96,
        ""unit"": ""mg/dL"",
        ""referenceRange"": ""70-99"",
        ""status"": ""Normal""
      },
      ""hba1c"": {
        ""value"": 5.4,
        ""unit"": ""%"",
        ""referenceRange"": ""<5.7"",
        ""status"": ""Normal""
      }
    },
    ""lipidProfile"": {
      ""totalCholesterol"": {
        ""value"": 198,
        ""unit"": ""mg/dL"",
        ""referenceRange"": ""<200"",
        ""status"": ""Normal""
      },
      ""ldl"": {
        ""value"": 122,
        ""unit"": ""mg/dL"",
        ""referenceRange"": ""<100"",
        ""status"": ""Borderline High""
      },
      ""hdl"": {
        ""value"": 48,
        ""unit"": ""mg/dL"",
        ""referenceRange"": "">40"",
        ""status"": ""Normal""
      },
      ""triglycerides"": {
        ""value"": 145,
        ""unit"": ""mg/dL"",
        ""referenceRange"": ""<150"",
        ""status"": ""Normal""
      }
    },
    ""liverFunction"": {
      ""alt"": {
        ""value"": 32,
        ""unit"": ""U/L"",
        ""referenceRange"": ""7-56"",
        ""status"": ""Normal""
      },
      ""ast"": {
        ""value"": 28,
        ""unit"": ""U/L"",
        ""referenceRange"": ""10-40"",
        ""status"": ""Normal""
      },
      ""bilirubinTotal"": {
        ""value"": 0.8,
        ""unit"": ""mg/dL"",
        ""referenceRange"": ""0.3-1.2"",
        ""status"": ""Normal""
      }
    },
    ""kidneyFunction"": {
      ""creatinine"": {
        ""value"": 0.9,
        ""unit"": ""mg/dL"",
        ""referenceRange"": ""0.7-1.3"",
        ""status"": ""Normal""
      },
      ""bloodUreaNitrogen"": {
        ""value"": 14,
        ""unit"": ""mg/dL"",
        ""referenceRange"": ""7-20"",
        ""status"": ""Normal""
      },
      ""egfr"": {
        ""value"": 108,
        ""unit"": ""mL/min/1.73m2"",
        ""referenceRange"": "">90"",
        ""status"": ""Normal""
      }
    },
    ""thyroid"": {
      ""tsh"": {
        ""value"": 2.1,
        ""unit"": ""uIU/mL"",
        ""referenceRange"": ""0.4-4.0"",
        ""status"": ""Normal""
      },
      ""freeT4"": {
        ""value"": 1.2,
        ""unit"": ""ng/dL"",
        ""referenceRange"": ""0.8-1.8"",
        ""status"": ""Normal""
      }
    },
    ""vitamins"": {
      ""vitaminD"": {
        ""value"": 24,
        ""unit"": ""ng/mL"",
        ""referenceRange"": ""30-100"",
        ""status"": ""Low""
      },
      ""vitaminB12"": {
        ""value"": 310,
        ""unit"": ""pg/mL"",
        ""referenceRange"": ""200-900"",
        ""status"": ""Normal""
      }
    }
  },
  ""summary"": {
    ""overallStatus"": ""Mostly Normal"",
    ""abnormalParameters"": [
      ""LDL Cholesterol"",
      ""Vitamin D""
    ],
    ""doctorRemarks"": ""Mildly elevated LDL cholesterol and Vitamin D deficiency. Recommend dietary modifications, regular exercise, and Vitamin D supplementation as advised by physician.""
  }
}";

var apiKey = "--";
var url = "https://api.openai.com/v1/responses";

using var client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

var requestBody = new
{
    model = "gpt-4.1-mini",
    input = "Consider you are health expert and able to read all blood reports,Please read this JSON and suggest about Diet plan , Sugget about overall heath and if need some precautions and report data is -" + healthreportData
};
var json = JsonSerializer.Serialize(requestBody);
var content = new StringContent(json, Encoding.UTF8, "application/json");

var response = await client.PostAsync(url, content);
var responseString = await response.Content.ReadAsStringAsync();

Console.WriteLine(responseString);



//using JsonDocument doc = JsonDocument.Parse(responseString);
//var output = doc.RootElement
//    .GetProperty("output")[0]
//    .GetProperty("content")[0]
//    .GetProperty("text")
//    .GetString();

//Console.WriteLine(output);

