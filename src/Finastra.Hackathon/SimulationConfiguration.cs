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
            B2CBearerToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJhenhYOVlMYzQ1LThIOThBQXJhanZmeldhLWIycFEtcnI2c0w2dFlZZHpNIn0.eyJqdGkiOiJmZTFjYjgzNy1mNDRkLTRlMmMtYjg0MS1kNWY2Njg1Y2Q3MjYiLCJleHAiOjE1NzU1NjkyOTAsIm5iZiI6MCwiaWF0IjoxNTc1NTY1NjkwLCJpc3MiOiJodHRwczovL2FwaS5mdXNpb25mYWJyaWMuY2xvdWQvbG9naW4vdjEiLCJhdWQiOlsiYjJjLXByb2ZpbGUtdjEtOTNhNmVmMjItMGFhNi00M2YxLTk2MjQtZjMzZWU4MDIyZTQ5IiwiY29ycG9yYXRlLXByb2ZpbGUtdjEtMWVjNjRkYTEtMDMyNC00OGJhLWJkZjYtN2Y0Njc4OTI2ZGI4IiwiY29ycG9yYXRlLWFjY291bnRlaW5mby1tZS12MS04MzFjYjA5ZC1jYzEwLTQ3NzItOGVkNS04YTZiNzJlYzhlMDEiXSwic3ViIjoiYTA2Zjc2NWUtNTRmNS00OTJhLWI0NDMtM2E3YmJjYjI5YTc2IiwidHlwIjoiQmVhcmVyIiwiYXpwIjoiYWY1NWNiNWQtZjRjMi00NWZkLTliNGEtYzBhY2ExMzdlMTUyIiwiYXV0aF90aW1lIjoxNTc1NTY1NjkwLCJzZXNzaW9uX3N0YXRlIjoiNzQxNTcxNWItMWRjNC00MmM3LThiNGQtZWRhYmRmMmU2NjYzIiwiYWNyIjoiMSIsInNjb3BlIjoib3BlbmlkIGNvcnBvcmF0ZS1wcm9maWxlLXYxLTFlYzY0ZGExLTAzMjQtNDhiYS1iZGY2LTdmNDY3ODkyNmRiOCBiMmMtcHJvZmlsZS12MS05M2E2ZWYyMi0wYWE2LTQzZjEtOTYyNC1mMzNlZTgwMjJlNDkgY29ycG9yYXRlLWFjY291bnRlaW5mby1tZS12MS04MzFjYjA5ZC1jYzEwLTQ3NzItOGVkNS04YTZiNzJlYzhlMDEiLCJhcHAiOiI4ZmI1MTk3Yy1jZDk0LTQxOTMtYjUyOC0xMmE5NDNkMWJmNDAiLCJpcHdoaXRlbGlzdCI6IiIsInRlbmFudCI6InNhbmRib3giLCJ1c2VybmFtZSI6ImZmZGN1c2VyMSJ9.aEj8FshMI4K7AKVxCVDzvzggCmdLi9mb3z5okkMiJ0orkOkKpwbOwsu2KapduQJi-CG35jf6RZkvfjPQAp2NgSocJtzGW24hK6H35CF4RNbe1nZl2oXdDucxyYNUUOXxDRvWPmvhJBV1azLOSIA_FxROAT33N4ZoP74R2dlwbCFubcEqjVbMNAoydi8Qza5g7DKcL9laek5ndJ9TH4Ditv6DenBKA8_9_d5cIOoK3MGpxDORuv68R2czH688l6us-r3dKh-bu064P9aIBvsnl2yBAgkYMi5pXk7xhYP8heRn6HkUAg650JSxZRVFtjqFI6Fq2CIUs-Z18MW4r8_Knw";

            B2EClientId = "a49ae472-c6c3-4580-9d94-6ab0a23f234a";
            B2ESecret = "85eef001-e46f-484e-acc0-bc97034983f1";
            B2EBearerToken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJhenhYOVlMYzQ1LThIOThBQXJhanZmeldhLWIycFEtcnI2c0w2dFlZZHpNIn0.eyJqdGkiOiI4YzlkNzcwZi00M2NhLTQ1ZjctOTgxMy0wYTkxMjczN2UzM2IiLCJleHAiOjE1NzU1Njg3NjQsIm5iZiI6MCwiaWF0IjoxNTc1NTY1MTY0LCJpc3MiOiJodHRwczovL2FwaS5mdXNpb25mYWJyaWMuY2xvdWQvbG9naW4vdjEiLCJhdWQiOlsiVXNQYXltZW50Q2FsY3VsYXRvci12MS03Y2I1ZDUxNS1kZWQ0LTRkMmItYWE0OS1lMWUzZGIyZDEwYTkiLCJpbnZlc3RtZW50LW1hbmFnZW1lbnQtYmVuY2htYXJrcy12MS03YmJmZDEzNC05ZmQ5LTQxNjktOWEyYi1kOWE4NWIzNjdhZGMiLCJpbnZlc3RtZW50LW1hbmFnZW1lbnQtYWNjb3VudGluZy12MS0wNTk2ZTMzZS05M2RjLTRhMTQtODRlNy0zOTY5ZTQxZmUxNWYiLCJpbnZlc3RtZW50LW1hbmFnZW1lbnQtZnVuZHMtdjEtZTgzZWJkMjUtNTQ5ZC00ODk3LWJkNDYtYWE3ZDQ5YmY1OGFmIl0sInN1YiI6ImEwNmY3NjVlLTU0ZjUtNDkyYS1iNDQzLTNhN2JiY2IyOWE3NiIsInR5cCI6IkJlYXJlciIsImF6cCI6ImE0OWFlNDcyLWM2YzMtNDU4MC05ZDk0LTZhYjBhMjNmMjM0YSIsImF1dGhfdGltZSI6MTU3NTU2NTE2Miwic2Vzc2lvbl9zdGF0ZSI6Ijc0MTU3MTViLTFkYzQtNDJjNy04YjRkLWVkYWJkZjJlNjY2MyIsImFjciI6IjEiLCJzY29wZSI6Im9wZW5pZCBVc1BheW1lbnRDYWxjdWxhdG9yLXYxLTdjYjVkNTE1LWRlZDQtNGQyYi1hYTQ5LWUxZTNkYjJkMTBhOSBpbnZlc3RtZW50LW1hbmFnZW1lbnQtYmVuY2htYXJrcy12MS03YmJmZDEzNC05ZmQ5LTQxNjktOWEyYi1kOWE4NWIzNjdhZGMgaW52ZXN0bWVudC1tYW5hZ2VtZW50LWFjY291bnRpbmctdjEtMDU5NmUzM2UtOTNkYy00YTE0LTg0ZTctMzk2OWU0MWZlMTVmIGludmVzdG1lbnQtbWFuYWdlbWVudC1mdW5kcy12MS1lODNlYmQyNS01NDlkLTQ4OTctYmQ0Ni1hYTdkNDliZjU4YWYiLCJhcHAiOiI4ZmI1MTk3Yy1jZDk0LTQxOTMtYjUyOC0xMmE5NDNkMWJmNDAiLCJpcHdoaXRlbGlzdCI6IiIsInRlbmFudCI6InNhbmRib3giLCJ1c2VybmFtZSI6ImZmZGN1c2VyMSJ9.KzeC4F5L1mare96glASjsWjnXO8yeZ4b8QIR0-C9rcWBTUYHV85iY9wwdHG0UdOmZvCEdFC0dpwQ8GMRGfkAYx9qFIkOd4dMKsi3JAU1QZ2WkhNbzbU9EBDbItHzEh69F0WH2M-GszqxfbaQqWFZHHVOsYk-FOuGr5TP9UuB7FK6PtNAk0-X-hubhs7ywui5fRIoORsjXD1RnbfmWj2DVDFulTXl9uLlnwRw-I7dnD8IDpcJ3ZnwqE57TMqEGx7Vm2YvgiABiqdn-HTxQF7lxDz8kMb7ERV_hqBYJCTitvhLp8zLsJ48Pnmfuzfpe-wnEENz1R8w8_ds9SKx7lm4eQ";
        }
    }
}