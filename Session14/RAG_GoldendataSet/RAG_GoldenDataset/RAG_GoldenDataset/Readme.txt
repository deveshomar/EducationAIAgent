
Hit Rate@3-
Did the Top 3 contain at least one relevant chunk?

Means when we get Result after cosine simily search,
we check the top 3 results. If at least one of those 
results is relevant to the query, we consider it a hit. 
This metric helps us evaluate the effectiveness of our retrieval
system in providing useful information to users.

Example
IT-03  - Expected 

Retrived 
IT-03
IT-02
SECURITY-01

IT-03 present

Means Hit@3 = 1	


===========================================

Precision@3

"Of the 3 chunks I retrieved, how many were actually relevant?"

Retrieved Top 3:

1. IT-03           ✅ Relevant
2. IT-02           ❌
3. SECURITY-01     ❌

Means Precision@3 = 1/3 = 0.33	
Out of 3 only one is of use

Precision@K =  
				Relevant retrieved chunks
				-------------------------
				Total retrieved chunks

Wht matters``
Your retriever is finding the answer, but it is also bringing unnecessary information.
==================================================

Recall@3

Now we look at the problem from the opposite direction.

Precision asks:Of what I retrieved, how much was relevant?


Recall asks:

Of everything relevant that I 
SHOULD have retrieved, how much did I actually retrieve?

Means->  Relevant chunks:

IT-03
IT-04

And we are getting only IT-03 in the top 3 results. So we are missing IT-04.
then It means Recall@3 = 1/2 = 0.5		

50% we are getting the relevant information, but we are missing 50% of it.	

Recall@K =
Relevant chunks retrieved
-------------------------
Total relevant chunks

===============================================================

Precision vs Recall

Golden Dataset have : [A, B, C]

Retrieved Top 3: [A, X, Y]

Relevant retrieved = 1
Total retrieved = 3
Total relevant = 3


Precision@3 = 1 / 3 = 33%   (out of 3 one is of use)

Recall@3 = 1 / 3 = 33%   (expected are 3 but get one one )

----------------
Example 2 
Golden Expected Chunk [A]
Retrieved: [A, X, Y]

Precision = 1 / 3 = 33%
Recall = 1 / 1 = 100%

====================================================================

MRR — Mean Reciprocal Rank
Position of retrival also matters  
MRR evaluates ranking quality.

Retriever A
1. IT-03 ✅
2. IT-02
3. HR-01

Here RR = 1 / 1
   = 1.0

Retriever B

1. IT-02
2. HR-01
3. IT-03 ✅

RR = 1 / 3
   = 0.333

Both have Hit rate =1

But they aren't equivalent.  Retriever A puts the relevant chunk at #1.
Retriever B puts it at #3.

MRR captures that difference: 


=============================================================================

Let's combine all four
Question : "What is the salary revision policy?"

Golden Dataset have : Relevant: SALARY-02

OutPut
Rank 1 → SALARY-01
Rank 2 → SALARY-02  ✅
Rank 3 → HR-01

Hit@3 
SALARY-02 exists.
Hit@3 = 1

Precision@3
1 relevant out of 3 is of use
Precision@3 = 1 / 3
            = 0.33

Recall@3

There is 1 relevant chunk and we found it:
Recall@3 = 1 / 1
         = 1.0

MMR
Relevant chunk is at position 2:
MRR contribution = 1 / 2

Metric          Score
----------------------
Hit@3           1.00
Precision@3     0.33
Recall@3        1.00
RR              0.50

==============================================================

Hit@3
   ↓
"Did I find anything useful?"

Precision@3
   ↓
"How much of what I retrieved is useful?"

Recall@3
   ↓
"Did I find all the useful information?"

MRR
   ↓
"How high did I rank the useful information?"


===============================================================	

We can test with Top 3, 5, 10 etc

             Top-1    Top-3    Top-5    Top-10
------------------------------------------------
Hit Rate
Precision
Recall
MRR


Top-1 → Recall 62%
Top-3 → Recall 86%
Top-5 → Recall 91%
Top-10 → Recall 96%


========================================================================================

| Metric          |     Result | What it tells us                                                     |
| --------------- | ---------: | -------------------------------------------------------------------- |
| **Hit Rate@3**  |    **95%** | For 95% of questions, at least one correct chunk was found in Top 3. |
| **Recall@3**    |    **95%** | The retriever is finding most of the relevant information.           |
| **MRR**         |    **95%** | The relevant chunk is usually ranked very high, often at Rank 1.     |
| **Precision@3** | **33.33%** | Only about 1 of the 3 retrieved chunks is relevant on average.       |




