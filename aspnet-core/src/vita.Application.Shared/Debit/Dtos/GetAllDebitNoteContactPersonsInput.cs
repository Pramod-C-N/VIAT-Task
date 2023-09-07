using Abp.Application.Services.Dto;
using System;

namespace vita.Debit.Dtos
{
    public class GetAllDebitNoteContactPersonsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string IRNNoFilter { get; set; }

        public string NameFilter { get; set; }

        public string EmployeeCodeFilter { get; set; }

        public string ContactNumberFilter { get; set; }

        public string GovtIdFilter { get; set; }

        public string EmailFilter { get; set; }

        public string AddressFilter { get; set; }

        public string LocationFilter { get; set; }

        public string TypeFilter { get; set; }

        public string AdditionalData1Filter { get; set; }

        public string LanguageFilter { get; set; }

    }
}