                         ┌─────────────────────────┐
                         │       👤 EMPLOYEE        │
                         │                         │   can you please tell me my salarydetails and leave balance
                         │ "What is my salary      │
                         │  status?"               │
                         └────────────┬────────────┘
                                      │
                                      ▼
                    ┌─────────────────────────────────┐
                    │       🎯 SUPERVISOR AGENT       │
                    │                                 │
                    │        Supervisor Kernel        │  LLM 1
                    │                                 │
                    │  Understand Intent / Route      │
                    └────────────────┬────────────────┘
                                     │
                 ┌───────────────────┼PayRoll +Leave───────────────────┐
                 │                   │                   │
                 ▼                   ▼                   ▼
        ┌────────────────┐  ┌────────────────┐  ┌────────────────┐
        │  💰 PAYROLL    │  │  🏖️ LEAVE     │  │  🖥️ IT         │
        │     AGENT      │  │     AGENT      │  │     AGENT      │
        └───────┬────────┘  └───────┬────────┘  └───────┬────────┘
                │                   │                   │
                ▼                   ▼                   ▼
        ┌────────────────┐  ┌────────────────┐  ┌────────────────┐
        │ Payroll Kernel │  │  Leave Kernel  │  │    IT Kernel   │
        └───────┬────────┘  └───────┬────────┘  └───────┬────────┘
                │                   │                   │
                ▼                   ▼                   ▼
        ┌────────────────┐  ┌────────────────┐  ┌────────────────┐
        │ PayrollPlugin  │  │  LeavePlugin   │  │   ITPlugin     │
        └───────┬────────┘  └───────┬────────┘  └───────┬────────┘
                │                   │                   │
                ▼                   ▼                   ▼
        ┌────────────────┐  ┌────────────────┐  ┌────────────────┐
        │   Payroll API  │  │    Leave DB    │  │ ServiceNow /   │
        │                │  │                │  │      AD        │
        └────────────────┘  └────────────────┘  └────────────────┘


        ==============================================================


                                      👤 EMPLOYEE
                                  │
                                  │
                     "Apply 3 days leave"
                                  │
                                  ▼
                    ┌─────────────────────────┐
                    │    🎯 SUPERVISOR        │
                    │         AGENT           │
                    │                         │
                    │  Route + Create Plan    │
                    └────────────┬────────────┘
                                 │
                                 ▼
                       ┌──────────────────┐
                       │   🏖️ LEAVE AGENT │
                       └────────┬─────────┘
                                │
                                ▼
                       ┌──────────────────┐
                       │   Leave Kernel   │
                       └────────┬─────────┘
                                │
                   ┌────────────┴────────────┐
                   │                         │
                   ▼                         ▼
             ┌────────────┐            ┌────────────┐
             │ READ TOOLS │            │ WRITE TOOL │
             │            │            │            │
             │ GetBalance │            │ ApplyLeave │
             │ GetHistory │            │ CancelLeave│
             └─────┬──────┘            └──────┬─────┘
                   │                          │
                   ▼                          │
          FunctionChoiceBehavior.Auto()       │
                   │                          │
                   ▼                          │
             GetBalance                       │
                   │                          │
                   ▼                          │
              Leave DB                        │
                   │                          │
                   ▼                          │
            Balance = 8 days                  │
                   │                          │
                   └────────────┐             │
                                ▼             │
                     ┌──────────────────┐     │
                     │  HUMAN APPROVAL  │◄────┘
                     │                  │
                     │ "You have 8 days │
                     │  available.      │
                     │                  │
                     │ Apply 3 days?"   │
                     │                  │
                     │  [YES]   [NO]    │
                     └────────┬─────────┘
                              │
                            YES
                              │
                              ▼
                     ┌──────────────────┐
                     │  C# EXECUTES     │
                     │  EXACT APPROVED  │
                     │  WRITE OPERATION │
                     └────────┬─────────┘
                              │
                              ▼
                         ApplyLeave()
                              │
                              ▼
                         Leave DB/API