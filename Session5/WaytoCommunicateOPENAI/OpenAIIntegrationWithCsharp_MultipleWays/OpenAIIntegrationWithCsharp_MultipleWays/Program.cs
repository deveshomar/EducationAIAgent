// See https://aka.ms/new-console-template for more information
using OpenAIIntegrationWithCsharp_MultipleWays;

Console.WriteLine("Calling Openai !");

//OpenAIClientData.CallAPI().Wait();  
DirectHTTPCall directHTTPCall = new DirectHTTPCall();   
directHTTPCall.CallOpenAI().Wait(); ;

//OpenAIReponseClientOpenAI.CallOpenAI().Wait();
