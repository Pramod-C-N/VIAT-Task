using Abp.Application.Services.Dto;
using System;

namespace vita.Customer.Dtos
{
    public class GetAllCustomerDocumentsesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomerIDFilter { get; set; }

        public Guid? CustomerUniqueIdentifierFilter { get; set; }

        public string DocumentTypeCodeFilter { get; set; }

        public string DocumentNameFilter { get; set; }

        public string DocumentNumberFilter { get; set; }

        public DateTime? MaxDoumentDateFilter { get; set; }
        public DateTime? MinDoumentDateFilter { get; set; }

        public string StatusFilter { get; set; }

    }
}