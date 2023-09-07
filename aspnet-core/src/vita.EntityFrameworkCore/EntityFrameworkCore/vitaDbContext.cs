using vita.CustomReportSP;
using vita.TenantDetails;
using vita.Vendor;
using vita.Customer;
using vita.Purchase;
using vita.Credit;
using vita.Debit;
using vita.ImportBatch;
using vita.Purchase;
using vita.Sales;
using vita.MasterData;
using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vita.Authorization.Delegation;
using vita.Authorization.Roles;
using vita.Authorization.Users;
using vita.Chat;
using vita.Editions;
using vita.Friendships;
using vita.MultiTenancy;
using vita.MultiTenancy.Accounting;
using vita.MultiTenancy.Payments;
using vita.Storage;

namespace vita.EntityFrameworkCore
{
    public class vitaDbContext : AbpZeroDbContext<Tenant, Role, User, vitaDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<Module> Module { get; set; }

        public virtual DbSet<Designation> Designation { get; set; }

        public virtual DbSet<BusinessOperationalModel> BusinessOperationalModel { get; set; }

        public virtual DbSet<BusinessTurnoverSlab> BusinessTurnoverSlab { get; set; }

        public virtual DbSet<ApportionmentBaseData> ApportionmentBaseData { get; set; }

        public virtual DbSet<CustomReport> CustomReport { get; set; }

        public virtual DbSet<TenantSectors> TenantSectors { get; set; }

        public virtual DbSet<TenantPurchaseVatCateory> TenantPurchaseVatCateory { get; set; }

        public virtual DbSet<TenantSupplyVATCategory> TenantSupplyVATCategory { get; set; }

        public virtual DbSet<TenantBusinessSupplies> TenantBusinessSupplies { get; set; }

        public virtual DbSet<TenantBusinessPurchase> TenantBusinessPurchase { get; set; }

        public virtual DbSet<TenantDocuments> TenantDocuments { get; set; }

        public virtual DbSet<TenantAddress> TenantAddress { get; set; }

        public virtual DbSet<TenantShareHolders> TenantShareHolders { get; set; }

        public virtual DbSet<TenantBasicDetails> TenantBasicDetails { get; set; }

        public virtual DbSet<Activecurrency> Activecurrency { get; set; }

        public virtual DbSet<BatchData> BatchData { get; set; }

        public virtual DbSet<VendorSectorDetail> VendorSectorDetails { get; set; }

        public virtual DbSet<VendorTaxDetails> VendorTaxDetailses { get; set; }

        public virtual DbSet<VendorForeignEntity> VendorForeignEntities { get; set; }

        public virtual DbSet<VendorOwnershipDetails> VendorOwnershipDetailses { get; set; }

        public virtual DbSet<VendorDocuments> VendorDocumentses { get; set; }

        public virtual DbSet<VendorContactPerson> VendorContactPersons { get; set; }

        public virtual DbSet<VendorAddress> VendorAddresses { get; set; }

        public virtual DbSet<Vendors> Vendorses { get; set; }

        public virtual DbSet<CustomerSectorDetail> CustomerSectorDetails { get; set; }

        public virtual DbSet<CustomerTaxDetails> CustomerTaxDetailses { get; set; }

        public virtual DbSet<CustomerForeignEntity> CustomerForeignEntities { get; set; }

        public virtual DbSet<CustomerOwnershipDetails> CustomerOwnershipDetailses { get; set; }

        public virtual DbSet<CustomerDocuments> CustomerDocumentses { get; set; }

        public virtual DbSet<CustomerContactPerson> CustomerContactPersons { get; set; }

        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

        public virtual DbSet<Customers> Customerses { get; set; }

        public virtual DbSet<ImportBatchData> ImportBatchDatas { get; set; }

        public virtual DbSet<PurchaseEntryParty> PurchaseEntryParties { get; set; }

        public virtual DbSet<PurchaseEntrySummary> PurchaseEntrySummaries { get; set; }

        public virtual DbSet<PurchaseEntryPaymentDetail> PurchaseEntryPaymentDetails { get; set; }

        public virtual DbSet<PurchaseEntryItem> PurchaseEntryItems { get; set; }

        public virtual DbSet<PurchaseEntryVATDetail> PurchaseEntryVATDetails { get; set; }

        public virtual DbSet<PurchaseEntryDiscount> PurchaseEntryDiscounts { get; set; }

        public virtual DbSet<PurchaseEntryContactPerson> PurchaseEntryContactPersons { get; set; }

        public virtual DbSet<PurchaseEntryAddress> PurchaseEntryAddresses { get; set; }

        public virtual DbSet<PurchaseEntry> PurchaseEntries { get; set; }

        public virtual DbSet<IRNMaster> IRNMasters { get; set; }
        public virtual DbSet<CreditNoteParty> CreditNoteParty { get; set; }

        public virtual DbSet<CreditNoteSummary> CreditNoteSummary { get; set; }

        public virtual DbSet<CreditNotePaymentDetail> CreditNotePaymentDetail { get; set; }

        public virtual DbSet<CreditNoteItem> CreditNoteItem { get; set; }

        public virtual DbSet<CreditNoteVATDetail> CreditNoteVATDetail { get; set; }

        public virtual DbSet<CreditNoteDiscount> CreditNoteDiscount { get; set; }

        public virtual DbSet<CreditNoteContactPerson> CreditNoteContactPerson { get; set; }

        public virtual DbSet<CreditNoteAddress> CreditNoteAddress { get; set; }

        public virtual DbSet<CreditNote> CreditNote { get; set; }
        public virtual DbSet<DebitNoteParty> DebitNoteParties { get; set; }

        public virtual DbSet<DebitNoteSummary> DebitNoteSummaries { get; set; }

        public virtual DbSet<DebitNotePaymentDetail> DebitNotePaymentDetails { get; set; }

        public virtual DbSet<DebitNoteItem> DebitNoteItems { get; set; }

        public virtual DbSet<DebitNoteVATDetail> DebitNoteVATDetails { get; set; }

        public virtual DbSet<DebitNoteDiscount> DebitNoteDiscounts { get; set; }

        public virtual DbSet<DebitNoteContactPerson> DebitNoteContactPersons { get; set; }

        public virtual DbSet<DebitNoteAddress> DebitNoteAddresses { get; set; }

        public virtual DbSet<DebitNote> DebitNotes { get; set; }

        public virtual DbSet<InvoiceType> InvoiceType { get; set; }

        public virtual DbSet<FinancialYear> FinancialYear { get; set; }

        public virtual DbSet<ErrorType> ErrorType { get; set; }

        public virtual DbSet<HeadOfPayment> HeadOfPayment { get; set; }

        public virtual DbSet<Currency> Currency { get; set; }

        public virtual DbSet<TaxCategory> TaxCategory { get; set; }

        public virtual DbSet<InvoiceCategory> InvoiceCategory { get; set; }

        public virtual DbSet<ErrorGroup> ErrorGroup { get; set; }

        public virtual DbSet<Affiliation> Affiliation { get; set; }

        public virtual DbSet<PlaceOfPerformance> PlaceOfPerformance { get; set; }

        public virtual DbSet<OrganisationType> OrganisationType { get; set; }

        public virtual DbSet<PurchaseType> PurchaseType { get; set; }

        public virtual DbSet<AllowanceReason> AllowanceReason { get; set; }

        public virtual DbSet<UnitOfMeasurement> UnitOfMeasurement { get; set; }

        public virtual DbSet<NatureofServices> NatureofServices { get; set; }

        public virtual DbSet<ExemptionReason> ExemptionReason { get; set; }

        public virtual DbSet<ReasonCNDN> ReasonCNDN { get; set; }

        public virtual DbSet<DocumentMaster> DocumentMaster { get; set; }

        public virtual DbSet<PaymentMeans> PaymentMeans { get; set; }

        public virtual DbSet<BusinessProcess> BusinessProcess { get; set; }

        public virtual DbSet<TaxSubCategory> TaxSubCategory { get; set; }

        public virtual DbSet<Country> Country { get; set; }

        public virtual DbSet<SalesInvoicePaymentDetail> SalesInvoicePaymentDetails { get; set; }

        public virtual DbSet<SalesInvoiceVATDetail> SalesInvoiceVATDetails { get; set; }

        public virtual DbSet<SalesInvoiceDiscount> SalesInvoiceDiscounts { get; set; }

        public virtual DbSet<SalesInvoiceContactPerson> SalesInvoiceContactPersons { get; set; }

        public virtual DbSet<SalesInvoiceAddress> SalesInvoiceAddresses { get; set; }

        public virtual DbSet<SalesInvoiceParty> SalesInvoiceParties { get; set; }

        public virtual DbSet<SalesInvoiceSummary> SalesInvoiceSummaries { get; set; }
        public virtual DbSet<TransactionCategory> TransactionCategory { get; set; }

        public virtual DbSet<Constitution> Constitution { get; set; }

        public virtual DbSet<TenantType> TenantType { get; set; }

        public virtual DbSet<Sector> Sector { get; set; }

        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }

        public virtual DbSet<SalesInvoice> SalesInvoices { get; set; }

        public virtual DbSet<Title> Title { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        public vitaDbContext(DbContextOptions<vitaDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Designation>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<BusinessOperationalModel>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BusinessTurnoverSlab>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Country>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ImportBatchData>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ApportionmentBaseData>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BatchData>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CustomReport>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantBasicDetails>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantSectors>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantPurchaseVatCateory>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantSupplyVATCategory>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantBusinessSupplies>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantBusinessPurchase>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantDocuments>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantAddress>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantShareHolders>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantBasicDetails>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DocumentMaster>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Activecurrency>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ImportBatchData>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BatchData>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<VendorSectorDetail>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<VendorTaxDetails>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<VendorForeignEntity>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<VendorOwnershipDetails>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<VendorDocuments>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<VendorContactPerson>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<VendorAddress>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Vendors>(v =>
                       {
                           v.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CustomerSectorDetail>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CustomerTaxDetails>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CustomerForeignEntity>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CustomerOwnershipDetails>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CustomerDocuments>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CustomerContactPerson>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CustomerAddress>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Customers>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ImportBatchData>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntryParty>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntrySummary>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntryPaymentDetail>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntryItem>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntryVATDetail>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntryDiscount>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntryContactPerson>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntryAddress>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseEntry>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<IRNMaster>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNoteParty>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<CreditNoteSummary>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNotePaymentDetail>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNoteItem>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNoteVATDetail>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNoteDiscount>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNoteContactPerson>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNoteAddress>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<CreditNote>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteParty>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<DebitNoteSummary>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNotePaymentDetail>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteItem>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteVATDetail>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteDiscount>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteContactPerson>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteAddress>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNote>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteDiscount>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteContactPerson>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNoteAddress>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DebitNote>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoice>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<InvoiceType>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<FinancialYear>(f =>
                       {
                           f.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ErrorType>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<HeadOfPayment>(h =>
                       {
                           h.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Currency>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TaxCategory>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<InvoiceCategory>(i =>
                       {
                           i.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ErrorGroup>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Affiliation>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PlaceOfPerformance>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<OrganisationType>(o =>
                       {
                           o.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PurchaseType>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<AllowanceReason>(a =>
                       {
                           a.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<UnitOfMeasurement>(u =>
                       {
                           u.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<NatureofServices>(n =>
                       {
                           n.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ExemptionReason>(x =>
                       {
                           x.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<ReasonCNDN>(r =>
                       {
                           r.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<DocumentMaster>(d =>
                       {
                           d.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<PaymentMeans>(p =>
                       {
                           p.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BusinessProcess>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TaxSubCategory>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Country>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoicePaymentDetail>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoiceVATDetail>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoiceDiscount>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoiceContactPerson>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoiceAddress>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoiceParty>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoiceSummary>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<TransactionCategory>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Constitution>(c =>
                       {
                           c.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<TenantType>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Sector>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Gender>(g =>
                       {
                           g.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<SalesInvoiceItem>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<SalesInvoice>(s =>
                       {
                           s.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<Title>(t =>
                       {
                           t.HasIndex(e => new { e.TenantId });
                       });
            modelBuilder.Entity<BinaryObject>(b =>
                       {
                           b.HasIndex(e => new { e.TenantId });
                       });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}