using System;
using System.Linq;
using Finastra.Hackathon.Accounting;
using Finastra.Hackathon.Finastra;

namespace Finastra.Hackathon
{
    public static class SimulationConfiguration
    {
        static SimulationConfiguration()
        {
            Reset();
        }

        public static string B2CClientId { get; set; }
        public static string B2CSecret { get; set; }
        public static string B2CBearerToken { get; set; }

        public static string B2EClientId { get; set; }
        public static string B2ESecret { get; set; }
        public static string B2EBearerToken { get; set; }


        public static Identity Lender { get; set; }
        public static RatioAnalysis RatioAnalysis { get; set; }
        public static string EmailAddress { get; set; }
        public static Guid SelectedNAIC { get; set; }

        public static bool SimulationStarted { get; set; }
        public static bool CustomerAlteredRationAnalysis { get; set; }
        public static bool LenderProposedAction { get; set; }
        public static bool AlertDismissed { get; set; }

        public static AmoritizationTable ProposedAmoritizationTable { get; set; }

        public static void Reset()
        {
            Lender = StaticData.Lenders.First();
            RatioAnalysis = new RatioAnalysis() { Date = new DateTime(2019, 9, 1), CostOfGoods = 75000, Inventory = 13400, Turnover = 5.60f, TurnDays = 64.32f };
            EmailAddress = "mpool@sycorr.com";
            SelectedNAIC = StaticData.NAIC.Single(x => x.Code == "424490" && x.Name == "Vegetables, canned, merchant wholesalers").Id;

            SimulationStarted = false;
            CustomerAlteredRationAnalysis = false;
            LenderProposedAction = false;
            AlertDismissed = false;

            ProposedAmoritizationTable = null;

            B2CClientId = "af55cb5d-f4c2-45fd-9b4a-c0aca137e152";
            B2CSecret = "764fdc15-faf5-4890-b9c2-ade2908fbff7";
            B2CBearerToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJhenhYOVlMYzQ1LThIOThBQXJhanZmeldhLWIycFEtcnI2c0w2dFlZZHpNIn0.eyJqdGkiOiIxYjI2Y2UzNi0xZjIzLTRmODMtYWRiNi0xZDQ2ZDNlMzU3Y2YiLCJleHAiOjE1NzU1NDQ0MzcsIm5iZiI6MCwiaWF0IjoxNTc1NTQwODM3LCJpc3MiOiJodHRwczovL2FwaS5mdXNpb25mYWJyaWMuY2xvdWQvbG9naW4vdjEiLCJhdWQiOlsiYjJjLXByb2ZpbGUtdjEtOTNhNmVmMjItMGFhNi00M2YxLTk2MjQtZjMzZWU4MDIyZTQ5IiwiY29ycG9yYXRlLXByb2ZpbGUtdjEtMWVjNjRkYTEtMDMyNC00OGJhLWJkZjYtN2Y0Njc4OTI2ZGI4IiwiY29ycG9yYXRlLWFjY291bnRlaW5mby1tZS12MS04MzFjYjA5ZC1jYzEwLTQ3NzItOGVkNS04YTZiNzJlYzhlMDEiXSwic3ViIjoiYTA2Zjc2NWUtNTRmNS00OTJhLWI0NDMtM2E3YmJjYjI5YTc2IiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiYWY1NWNiNWQtZjRjMi00NWZkLTliNGEtYzBhY2ExMzdlMTUyIiwiYXV0aF90aW1lIjoxNTc1NTQwODM3LCJzZXNzaW9uX3N0YXRlIjoiMmMwNjFlZjYtY2IyZC00ZTVjLThjNjQtNmM2MmEyOTU3ZTcxIiwiYWNyIjoiMSIsInNjb3BlIjoib3BlbmlkIGIyYy1wcm9maWxlLXYxLTkzYTZlZjIyLTBhYTYtNDNmMS05NjI0LWYzM2VlODAyMmU0OSBjb3Jwb3JhdGUtYWNjb3VudGVpbmZvLW1lLXYxLTgzMWNiMDlkLWNjMTAtNDc3Mi04ZWQ1LThhNmI3MmVjOGUwMSBjb3Jwb3JhdGUtcHJvZmlsZS12MS0xZWM2NGRhMS0wMzI0LTQ4YmEtYmRmNi03ZjQ2Nzg5MjZkYjgiLCJhcHAiOiI4ZmI1MTk3Yy1jZDk0LTQxOTMtYjUyOC0xMmE5NDNkMWJmNDAiLCJpcHdoaXRlbGlzdCI6IiIsInRlbmFudCI6InNhbmRib3giLCJ1c2VybmFtZSI6ImZmZGN1c2VyMSJ9.kFGjR6RFalUTm2fOCNM-iqoWQa4t0t1K61r0NpiDGvcnUPIVhv7D7bqJ6qeXL6mkP4qI3C1HesaGjoV8jgLKT3si5PYYkyX83Sij-ohv7GEdqWhMuW6nAvPqWtPw3Zpf8R2bM-uKf665ZHlQ87-_6Fp8NmTpzcXSLA7CexaBgV-fkDZxe2l65IlIhtgYu4c8V2xTZL4Q1tBqhF6lhOBbRO5oTj3ANsuR23h8kfVJyj7mwaqxwVW-rUyNUC3tedi609Lse_IhcoqlO7Fn1He1Y9U2HX3vP9J-dkhgNUT5I9UJ2j_8f-G4cUeeyLwyI8SRnrnPmiFEc2Rdrh52ZJ41Eg";

            B2EClientId = "a49ae472-c6c3-4580-9d94-6ab0a23f234a";
            B2ESecret = "85eef001-e46f-484e-acc0-bc97034983f1";
            B2EBearerToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJhenhYOVlMYzQ1LThIOThBQXJhanZmeldhLWIycFEtcnI2c0w2dFlZZHpNIn0.eyJqdGkiOiJkZmRlMWJhZC0xZjdjLTQyZWEtOTAzNC02MDQ0NzQyZTQwZTkiLCJleHAiOjE1NzU1NDQzNDAsIm5iZiI6MCwiaWF0IjoxNTc1NTQwNzM5LCJpc3MiOiJodHRwczovL2FwaS5mdXNpb25mYWJyaWMuY2xvdWQvbG9naW4vdjEiLCJhdWQiOlsiVXNQYXltZW50Q2FsY3VsYXRvci12MS03Y2I1ZDUxNS1kZWQ0LTRkMmItYWE0OS1lMWUzZGIyZDEwYTkiLCJpbnZlc3RtZW50LW1hbmFnZW1lbnQtYmVuY2htYXJrcy12MS03YmJmZDEzNC05ZmQ5LTQxNjktOWEyYi1kOWE4NWIzNjdhZGMiLCJpbnZlc3RtZW50LW1hbmFnZW1lbnQtYWNjb3VudGluZy12MS0wNTk2ZTMzZS05M2RjLTRhMTQtODRlNy0zOTY5ZTQxZmUxNWYiLCJpbnZlc3RtZW50LW1hbmFnZW1lbnQtZnVuZHMtdjEtZTgzZWJkMjUtNTQ5ZC00ODk3LWJkNDYtYWE3ZDQ5YmY1OGFmIl0sInN1YiI6ImEwNmY3NjVlLTU0ZjUtNDkyYS1iNDQzLTNhN2JiY2IyOWE3NiIsInR5cCI6IkJlYXJlciIsImF6cCI6ImE0OWFlNDcyLWM2YzMtNDU4MC05ZDk0LTZhYjBhMjNmMjM0YSIsImF1dGhfdGltZSI6MTU3NTU0MDczOSwic2Vzc2lvbl9zdGF0ZSI6IjJjMDYxZWY2LWNiMmQtNGU1Yy04YzY0LTZjNjJhMjk1N2U3MSIsImFjciI6IjEiLCJzY29wZSI6Im9wZW5pZCBpbnZlc3RtZW50LW1hbmFnZW1lbnQtYmVuY2htYXJrcy12MS03YmJmZDEzNC05ZmQ5LTQxNjktOWEyYi1kOWE4NWIzNjdhZGMgaW52ZXN0bWVudC1tYW5hZ2VtZW50LWFjY291bnRpbmctdjEtMDU5NmUzM2UtOTNkYy00YTE0LTg0ZTctMzk2OWU0MWZlMTVmIGludmVzdG1lbnQtbWFuYWdlbWVudC1mdW5kcy12MS1lODNlYmQyNS01NDlkLTQ4OTctYmQ0Ni1hYTdkNDliZjU4YWYgVXNQYXltZW50Q2FsY3VsYXRvci12MS03Y2I1ZDUxNS1kZWQ0LTRkMmItYWE0OS1lMWUzZGIyZDEwYTkiLCJhcHAiOiI4ZmI1MTk3Yy1jZDk0LTQxOTMtYjUyOC0xMmE5NDNkMWJmNDAiLCJpcHdoaXRlbGlzdCI6IiIsInRlbmFudCI6InNhbmRib3giLCJ1c2VybmFtZSI6ImZmZGN1c2VyMSJ9.K3M2fCWOlzFBa7xavlA0Z5yVJuV_vMjM-LlRSgysHnGJUHCGwO1zIFqaf1yCFwHTM9GCiKbkoeOMQbRnKsScMSl1-zxVnBBxvte80ZDCRP94qVq-fzaQPI2IKtl4Co4tI_izT_q0T9M1ozUxTilTXl-yHUTtAxOtHbFW-lXF1Oj_5pAHB8zxEhFYh2j5Hfkg9kMKbG834DWoOc3HzOixFP_E8Vlu5QwWrAulxW_0eJsc3dFomV7GjmeB7SGE9e447dC0nT_PLvK_I2F6EaPa1UtxVwisWZeI4V-JBt-kMN54MBD2gMivXRH4xw1E72peobqKtTecXWcUXLQi-QkJ7w";
        }
    }
}