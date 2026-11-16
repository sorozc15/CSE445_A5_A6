using System;
using TaskNotes.PartC;
class PriorityRulesTests {
    static void Expect(int i,int d,int h,int score,string level) {
        var r=PriorityRules.Calculate(i,d,h);
        if(r.Score!=score || r.Level!=level) throw new Exception("Unexpected result");
    }
    static void Bad(int i,int d,int h) {
        try { PriorityRules.Calculate(i,d,h); } catch(ArgumentOutOfRangeException) { return; }
        throw new Exception("Invalid input accepted");
    }
    static void Main() {
        Expect(5,0,8,95,"Critical"); Expect(3,7,2,44,"Medium"); Expect(1,30,1,8,"Low");
        Expect(5,-1,100,100,"Critical"); Expect(1,3650,1,8,"Low"); Expect(1,-365,1,58,"High");
        Bad(0,0,1); Bad(6,0,1); Bad(1,-366,1); Bad(1,3651,1); Bad(1,0,0); Bad(1,0,101);
        for(int i=1;i<=5;i++) for(int d=-2;d<40;d++) for(int h=1;h<=100;h++) {
            var r=PriorityRules.Calculate(i,d,h);
            if(r.Score<0 || r.Score>100) throw new Exception("Score out of bounds");
            if(d<39 && r.Score<PriorityRules.Calculate(i,d+1,h).Score) throw new Exception("Urgency monotonicity failed");
            if(i<5 && r.Score>PriorityRules.Calculate(i+1,d,h).Score) throw new Exception("Importance monotonicity failed");
        }
        Console.WriteLine("Priority tests passed, including 21,000 bounded/monotonic input combinations.");
    }
}
