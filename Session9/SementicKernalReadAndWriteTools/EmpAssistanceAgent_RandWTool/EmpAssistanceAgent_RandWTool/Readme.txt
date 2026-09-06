                   USER   (can you please apply leave for me frmo x to y )
                    │
                    ↓
          EmployeeIntentClassifier   LLM 1 (API CALL)
                    │
             NO Auto() ❌ apply_leave
                    │ ValidateWriteOperation (apply_leave))
          ┌─────────┼─────────┐
          ↓         ↓         ↓
        READ      WRITE      CHAT
          │         │
          ↓         ↓
 LLM 2   Read SK
        Agent   C# Flow
       Auto()       │
          │         ├─ Validation
          ↓         ├─ Authorization
     Read Tools     ├─ Approval
                    └─ Write Tool


Steps

a.Define Write and Read Operations (Implement Read and Write Tools)
b.Define Employee Intent Classifier
c.Check the Employee Intent Classifier is ReadVs Write 
d.If Read, then call LLM and attach the Read Tool to the LLM   and make Auto call
e.If write then call your code and validate the request and then call the Write Tool to perform the write operation    
