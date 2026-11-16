using System;
using System.Web.Services;
namespace TaskNotes.PartC {
    public class PriorityResult {
        public int Score { get; set; }
        public string Level { get; set; }
        public string Explanation { get; set; }
    }
    // Public, stateless SOAP service; no account or private task data is needed.
    [WebService(Namespace="urn:tasknotes:partc")]
    [WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1)]
    public class PriorityService : WebService {
        [WebMethod(Description="Score a task using importance (1-5), days until due (-365..3650), and estimated hours (1-100).")]
        public PriorityResult CalculatePriority(int importance, int daysUntilDue, int estimatedHours) {
            return PriorityRules.Calculate(importance, daysUntilDue, estimatedHours);
        }
    }
    public static class PriorityRules {
        public static PriorityResult Calculate(int importance, int daysUntilDue, int estimatedHours) {
            if (importance < 1 || importance > 5) throw new ArgumentOutOfRangeException("importance", "Use 1 through 5.");
            if (daysUntilDue < -365 || daysUntilDue > 3650) throw new ArgumentOutOfRangeException("daysUntilDue", "Use -365 through 3650.");
            if (estimatedHours < 1 || estimatedHours > 100) throw new ArgumentOutOfRangeException("estimatedHours", "Use 1 through 100.");
            int urgency = daysUntilDue < 0 ? 50 : daysUntilDue == 0 ? 45 : daysUntilDue == 1 ? 40 : daysUntilDue <= 3 ? 30 : daysUntilDue <= 7 ? 20 : daysUntilDue <= 14 ? 10 : 0;
            // Large tasks get a small planning boost. Score is deterministic and capped at 100.
            int effort = estimatedHours >= 8 ? 10 : estimatedHours >= 4 ? 5 : 0;
            int score = Math.Min(100, importance * 8 + urgency + effort);
            return new PriorityResult { Score=score, Level=score >= 75 ? "Critical" : score >= 55 ? "High" : score >= 30 ? "Medium" : "Low",
                Explanation=String.Format("Importance: {0}; urgency: {1}; effort: {2}. Sort descending by score; break ties by earlier due date, then task ID.", importance * 8, urgency, effort) };
        }
    }
}
