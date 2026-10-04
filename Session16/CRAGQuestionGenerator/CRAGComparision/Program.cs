#pragma warning disable OPENAI001

using CRAGComparision;
using OpenAI;
using OpenAI.Embeddings;
using OpenAI.Responses;

string apiKey = "sk-proj-";


var embeddingClient = new EmbeddingClient(
    "text-embedding-3-small",
    apiKey);

var responseClient = new ResponsesClient(apiKey);


// ============================================================
// 1. SAMPLE HR POLICIES
// ============================================================

var policies = new List<Policy>
{
    new(
        1,
        "Annual Leave Policy",
        """
        Employees are entitled to 20 days of annual paid leave per
        calendar year. Leave must be requested through the HR portal
        and approved by the employee's manager before the leave begins.
        Unused annual leave may be carried forward according to company policy.
        """
    ),

    new(
        2,
        "Sick Leave Policy",
        """
        Employees can take up to 10 days of paid sick leave per year.
        Employees should inform their manager as soon as possible when
        they are unable to work because of illness. A medical certificate
        may be required for extended periods of absence.
        """
    ),

    new(
        3,
        "Work From Home Policy",
        """
        Eligible employees may work from home up to two days per week.
        Employees must obtain manager approval and remain available
        during normal working hours. Some roles may not be eligible
        for remote work because of business requirements.
        """
    ),

    new(
        4,
        "Parental Leave Policy",
        """
        Eligible employees may receive parental leave following the birth
        or adoption of a child. Employees should submit a parental leave
        request to HR in advance whenever possible. The duration and
        eligibility requirements depend on the employee's circumstances
        and applicable company policy.
        """
    ),

    new(
        5,
        "Salary Payment Policy",
        """
        Employee salaries are paid monthly. Salary is normally credited
        to the employee's registered bank account on the last working day
        of each month, subject to bank processing times and public holidays.
        """
    )
};


// ============================================================
// 2. CREATE EMBEDDINGS FOR POLICIES
// ============================================================

Console.WriteLine("Creating policy embeddings...\n");

foreach (var policy in policies)
{
    policy.Embedding = await Helper. CreateEmbeddingAsync(
        embeddingClient,
        policy.Content);
}


// ============================================================
// 3. EMPLOYEE QUESTIONS
// ============================================================

var questions = new List<string>
{
    "How many paid vacation days can I take?",
    "Can I work from home two days every week?",
    "When will my salary be credited?",
    "How many sick days do employees get?"
};


// ============================================================
// 4. PROCESS EACH QUESTION
// ============================================================

foreach (string question in questions)
{
    Console.WriteLine("==================================================");
    Console.WriteLine($"EMPLOYEE QUESTION");
    Console.WriteLine(question);
    Console.WriteLine("==================================================");

    // --------------------------------------------------------
    // A. NORMAL QUESTION RETRIEVAL
    // --------------------------------------------------------

    Console.WriteLine("\nNORMAL RETRIEVAL");
    Console.WriteLine("----------------");

    float[] questionEmbedding =
        await  Helper.CreateEmbeddingAsync(
            embeddingClient,
            question);

    var normalResults = policies
        .Select(policy => new
        {
            Policy = policy,
            Score = Helper.CosineSimilarity(
                questionEmbedding,
                policy.Embedding!)
        })
        .OrderByDescending(x => x.Score)
        .ToList();

    foreach (var result in normalResults)
    {
        Console.WriteLine(
            $"{result.Score:F4} - {result.Policy.Title}");
    }


    // --------------------------------------------------------
    // B. REWRITE QUESTION
    // --------------------------------------------------------

    Console.WriteLine("\nREWRITTEN QUESTION");
    Console.WriteLine("------------------");

    string rewrittenQuestion =
        await Helper.RewriteQuestionAsync(
            responseClient,
            question);

    Console.WriteLine(rewrittenQuestion);


    // --------------------------------------------------------
    // C. CRAG RETRIEVAL
    // --------------------------------------------------------

    float[] rewrittenEmbedding =
        await Helper.CreateEmbeddingAsync(
            embeddingClient,
            rewrittenQuestion);

    var cragResults = policies
        .Select(policy => new
        {
            Policy = policy,
            Score = Helper.CosineSimilarity(
                rewrittenEmbedding,
                policy.Embedding!)
        })
        .OrderByDescending(x => x.Score)
        .ToList();

    Console.WriteLine("\nCRAG RETRIEVAL");
    Console.WriteLine("--------------");

    foreach (var result in cragResults)
    {
        Console.WriteLine(
            $"{result.Score:F4} - {result.Policy.Title}");
    }


    // --------------------------------------------------------
    // D. TAKE TOP 2 DOCUMENTS
    // --------------------------------------------------------

    var topPolicies = cragResults
        .Take(2)
        .ToList();

    Console.WriteLine("\nRETRIEVED POLICIES");
    Console.WriteLine("------------------");

    foreach (var result in topPolicies)
    {
        Console.WriteLine(
            $"\n[{result.Score:F4}] {result.Policy.Title}");

        Console.WriteLine(result.Policy.Content);
    }


    // --------------------------------------------------------
    // E. GENERATE FINAL CRAG ANSWER
    // --------------------------------------------------------

    string context = string.Join(
        "\n\n",
        topPolicies.Select(x =>
            $"POLICY: {x.Policy.Title}\n{x.Policy.Content}"));

    string answer =
        await Helper.GenerateAnswerAsync(
            responseClient,
            question,
            context);

    Console.WriteLine("\nCRAG ANSWER");
    Console.WriteLine("-----------");
    Console.WriteLine(answer);

    Console.WriteLine();
}



