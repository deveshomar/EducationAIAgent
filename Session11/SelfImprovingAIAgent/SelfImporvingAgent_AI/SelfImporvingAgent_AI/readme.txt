┌────────────────┐     ┌──────────────────┐     ┌─────────────────┐
 │  1. GENERATOR  │────>│   2. EVALUATOR   │────>│  Pass (Score   │────> End / Output
 │  Create Plan   │     │ Score & Feedback │     │     >= 90)?     │
 └────────────────┘     └──────────────────┘     └─────────────────┘
         ▲                                                │
         │                   No (Score < 90)              │
         └────────────────────────────────────────────────┘
                         3. Feed Critique Back


===============================================================================

   USER GOAL
    ↓
RESEARCH
    ↓
CREATE PLAN
    ↓
EVALUATE
    ↓
┌─────────────────┐
│   SCORE >= 90?  │   Create own validation score function
└───────┬─────────┘
        │
   ┌────┴────┐
  YES        NO
   ↓          ↓
FINAL      IMPROVE
PLAN         PLAN
   ↑          ↓
   │     EVALUATE AGAIN
   │          │
   └──────────┘
        ↓
       END