#pragma warning disable OPENAI001

using OpenAI.Responses;

string apiKey = "sk-proj-";


var client = new ResponsesClient(apiKey);

var questions = new List<string>
{
    "What is my leave allowance??",
    "What happens if I don't use my leave?ng?",
    "How many unused annual leave days can be carried\r\nforward and when does carried-forward leave expire?",
    "I need a long vacation. What do I do?",
    "Is teeth treatment covered by work insurance?",
    "What benefits do I have for health?",
    "Does employee insurance include dental?",
    "Does the company pay for my doctor visits?",
    "Can my dependents use my insurance?"
};

foreach (string question in questions)
{
    string prompt = $"""
You are an HR policy question rewriting engine for a CRAG system.

The questions are related to employees, HR policies, benefits, leave,
attendance, payroll, compensation, performance, recruitment, onboarding,
offboarding, workplace rules, and other employee-related matters.

Your task is to rewrite the user's question to improve retrieval quality
from an HR policy knowledge base.

Rules:
1. Preserve the original intent of the employee's question.
2. Make the question clear, precise, and grammatically correct.
3. Use appropriate HR and employee-related terminology.
4. Make the question specific enough to retrieve the relevant HR policy.
5. Do not change the meaning of the question.
6. Do not invent company policies, rules, dates, amounts, or eligibility criteria.
7. Do not answer the question.
8. If the question is already clear, make only minimal improvements.
9. Return ONLY the improved question.
10. Keep the question concise.

Examples:

Original:
"how many leaves employee can take"

Improved:
"How many leave days is an employee entitled to under the company's leave policy?"

Original:
"can I work from home"

Improved:
"What is the company's policy on employee work-from-home eligibility and approval?"

Original:
"when salary credited"

Improved:
"What is the company's policy regarding the employee salary payment date?"

Original:
"maternity leave for employee"

Improved:
"What is the company's maternity leave policy and employee eligibility criteria?"

Original:
"can manager reject leave"

Improved:
"Under the company's leave policy, can a manager reject an employee's leave request?"

Now rewrite this employee HR question:

{question}
""";


    //string prompt = $"""
    //Improve this question for a CRAG system.

    //Requirements:
    //- Make the question clear and specific.
    //- Fix grammar.
    //- Preserve the original intent.
    //- Make it suitable for retrieval from a knowledge base.
    //- Return ONLY the improved question.

    //Question:
    //{question}
    //""";

    ResponseResult response = await client.CreateResponseAsync(
        "gpt-5.2",
        prompt
    );

    Console.WriteLine($"Original : {question}");
    Console.WriteLine($"Improved : {response.GetOutputText()}");
    Console.WriteLine();
}
