using System;
using Newtonsoft.Json;

namespace Apprenticeship.Function.CosmosDB
{

    [Serializable]
    public class VacancySummary
    {
        [JsonProperty(PropertyName = "vacancyid")]
        public int VacancyId { get; set; }

        [JsonProperty(PropertyName = "vacancyreferencenumber")]
        public int VacancyReferenceNumber { get; set; }

        [JsonProperty(PropertyName = "vacancyguid")]
        public string VacancyGuid { get; set; }

        [JsonProperty(PropertyName = "vacancyownerrelationshipid")]
        public int VacancyOwnerRelationshipId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "shortdescription")]
        public string ShortDescription { get; set; }

        [JsonProperty(PropertyName = "workingweek")]
        public string WorkingWeek { get; set; }

        [JsonProperty(PropertyName = "expectedDuration")]
        public string ExpectedDuration { get; set; }

        [JsonProperty(PropertyName = "durationtype")]
        public string DurationType { get; set; }

        [JsonProperty(PropertyName = "closingdate")]
        public string ClosingDate { get; set; }

        [JsonProperty(PropertyName = "possiblestartdate")]
        public string PossibleStartDate { get; set; }

        [JsonProperty(PropertyName = "offlinevacancy")]
        public bool OfflineVacancy { get; set; }

        [JsonProperty(PropertyName = "dateqaapproved")]
        public string DateQAApproved { get; set; }

        [JsonProperty(PropertyName = "trainingtype")]
        public string TrainingType { get; set; }

        [JsonProperty(PropertyName = "apprenticeshiplevel")]
        public string ApprenticeshipLevel { get; set; }

        [JsonProperty(PropertyName = "frameworkcodename")]
        public string FrameworkCodeName { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "numberofpositions")]
        public int NumberOfPositions { get; set; }

        [JsonProperty(PropertyName = "vacancytype")]
        public string VacancyType { get; set; }

        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }

        [JsonProperty(PropertyName = "correctownerid")]
        public int ContractOwnerId { get; set; }

        [JsonProperty(PropertyName = "vacancylocationtype")]
        public string VacancyLocationType { get; set; }

        [JsonProperty(PropertyName = "employerid")]
        public int EmployerId { get; set; }

        [JsonProperty(PropertyName = "employername")]
        public string EmployerName { get; set; }

        [JsonProperty(PropertyName = "providertradingname")]
        public string ProviderTradingName { get; set; }

        [JsonProperty(PropertyName = "wage")]
        public Wage Wage { get; set; }

        [JsonProperty(PropertyName = "isemployerpositiveaboutdisability")]
        public bool IsEmployerPositiveAboutDisability { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public double? Duration { get; set; }

        [JsonProperty(PropertyName = "employerannoymousname")]
        public string EmployerAnonymousName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}