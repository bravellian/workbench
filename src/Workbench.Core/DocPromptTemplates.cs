namespace Workbench.Core;

internal static class DocPromptTemplates
{
    public static string BuildTemplate(string docType)
    {
        return docType.Trim().ToLowerInvariant() switch
        {
            "adr" => """
                # <title>
                
                ## Status
                
                ## Context
                
                ## Decision
                
                ## Consequences
                """,
            "spec" => """
                # <title>
                
                ## Summary
                
                ## Goals
                
                ## Non-goals
                
                ## Requirements
                """,
            "runbook" => """
                # <title>
                
                ## Purpose
                
                ## Steps
                
                ## Rollback
                """,
            _ => """
                # <title>
                
                ## Overview
                
                ## Notes
                """
        };
    }
}
