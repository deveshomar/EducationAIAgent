using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPENAITOOLCALLING.TEST
{
    public class TestCases
    {
        public static List<(string Query, string ExpectedTool)> getTaxTestcases()
        {
            var testCases = new List<(string Query, string ExpectedTool)>
    {
        ("can how i pay tax", "get_tax_details"),

        ("Get tax details for employee 2344", "get_tax_details"),

        ("Please share tax details for employee 2344", "get_tax_details"),

        ("What is the tax information for employee 2344?", "get_tax_details"),

        ("Show me employee 2344's tax information", "get_tax_details"),

        ("How much tax has employee 2344 paid?", "get_tax_details"),

        ("How much tax was deducted for employee 2344?", "get_tax_details"),

        ("What is the taxable income of employee ?", "get_tax_details"),

        ("Show for emp 2344",
            "get_tax_details"),

        ("Tell me about employee 2344's taxes", "get_tax_details"),

        ("Can you check the tax information for employee 2344?",
            "get_tax_details"),

        ("I need employee 2344's tax details", "get_tax_details"),

        ("Please provide income tax details for employee 2344",
            "get_tax_details"),

        ("What tax was deducted from employee 2344?", "get_tax_details"),

        ("Can you tell me employee 2344's annual taxable income?","get_tax_details"),

        ("I want to know about employee 2344's taxes",
            "get_tax_details")
    };

            return testCases;
        }


        public static List<(string Query, string ExpectedTool)> getManagerDetailsTestcases()
        {
            var managerTestCases = new List<(string Query, string ExpectedTool)>
{
        ("Who is senior to employee ?", "get_manager_details"),

        ("Who does employee 2344 report under?",  "get_manager_details"),
        
        ("Who is senior to employee ?", "get_manager_details"),

        ("Who does employee 2344 report under?",   "get_manager_details"),
                
                // ==============================
    // DIRECT QUESTIONS
    // ==============================

    ("Get manager details for employee 2344", "get_manager_details"),

    ("Show manager details for employee 2344", "get_manager_details"),

    ("Please provide manager details for employee 2344", "get_manager_details"),

    ("Give me manager information for employee 2344", "get_manager_details"),

    ("Get the manager information of employee 2344", "get_manager_details"),

    ("Show me the manager of employee 2344", "get_manager_details"),

    ("Who is the manager of employee 2344?", "get_manager_details"),

    ("Who manages employee 2344?", "get_manager_details"),

    ("Who is managing employee 2344?", "get_manager_details"),

    ("Tell me who the manager of employee 2344 is", "get_manager_details"),


    // ==============================
    // MANAGER INFORMATION
    // ==============================

    ("What is the manager information for employee 2344?", "get_manager_details"),

    ("What are the manager details for employee 2344?", "get_manager_details"),

    ("Can you tell me employee 2344's manager?", "get_manager_details"),

    ("Can you tell me who employee 2344 reports to?", "get_manager_details"),

    ("Who does employee 2344 report to?", "get_manager_details"),

    ("Who does employee 2344 work under?", "get_manager_details"),

    ("Who is employee 2344's reporting manager?", "get_manager_details"),

    ("What is the reporting manager of employee 2344?", "get_manager_details"),

    ("Show reporting manager for employee 2344", "get_manager_details"),

    ("Get reporting manager information for employee 2344", "get_manager_details"),


    // ==============================
    // REPORTING / HIERARCHY
    // ==============================

    ("Who does employee 2344 report to?", "get_manager_details"),

    ("Who does employee 2344 report under?", "get_manager_details"),

    ("Who is above employee 2344 in the organization?", "get_manager_details"),

    ("Who is employee 2344's supervisor?", "get_manager_details"),

    ("Who supervises employee 2344?", "get_manager_details"),

    ("Who is the supervisor of employee 2344?", "get_manager_details"),

    ("Find the supervisor of employee 2344", "get_manager_details"),

    ("Find employee 2344's reporting person", "get_manager_details"),

    ("Tell me employee 2344's reporting person", "get_manager_details"),


    // ==============================
    // NATURAL / CONVERSATIONAL
    // ==============================

    ("I want to know who manages employee 2344", "get_manager_details"),

    ("I need to know employee 2344's manager", "get_manager_details"),

    ("I need employee 2344's manager details", "get_manager_details"),

    ("Can you check who manages employee 2344?", "get_manager_details"),

    ("Can you check employee 2344's manager?", "get_manager_details"),

    ("Please check who employee 2344 reports to", "get_manager_details"),

    ("I want information about employee 2344's manager", "get_manager_details"),

    ("I want the reporting manager details for employee 2344", "get_manager_details"),

    ("Please find out who employee 2344 reports to", "get_manager_details"),

    ("Could you tell me who manages employee 2344?", "get_manager_details"),


    // ==============================
    // MANAGER + DEPARTMENT
    // ==============================

    ("Who is employee 2344's manager and which department is he in?",
        "get_manager_details"),

    ("Show employee 2344's manager and department",
        "get_manager_details"),

    ("Give me the manager and department details for employee 2344",
        "get_manager_details"),

    ("What department does employee 2344's manager belong to?",
        "get_manager_details"),

    ("Tell me the name of employee 2344's manager",
        "get_manager_details"),


    // ==============================
    // SHORT QUERIES
    // ==============================

    ("Manager for employee 2344", "get_manager_details"),

    ("Employee 2344 manager", "get_manager_details"),

    ("Employee 2344's manager", "get_manager_details"),

    ("Manager details 2344", "get_manager_details"),

    ("Manager information 2344", "get_manager_details"),

    ("Reporting manager 2344", "get_manager_details"),

    ("Supervisor for 2344", "get_manager_details"),


    // ==============================
    // DIFFERENT WORDING
    // ==============================

    ("Who is responsible for managing employee 2344?",
        "get_manager_details"),

    ("Who is the person employee 2344 reports to?",
        "get_manager_details"),

    ("Which manager does employee 2344 report to?",
        "get_manager_details"),

    ("Which person manages employee 2344?",
        "get_manager_details"),

    ("Who is employee 2344's team manager?",
        "get_manager_details"),

    ("Who is the direct manager of employee 2344?",
        "get_manager_details"),

    ("Who is the direct supervisor of employee 2344?",
        "get_manager_details"),

    ("Find the direct manager for employee 2344",
        "get_manager_details"),

    ("Find employee 2344's manager",
        "get_manager_details"),

    ("Lookup manager for employee 2344",
        "get_manager_details")

};

            return managerTestCases;
        }
    }
}
