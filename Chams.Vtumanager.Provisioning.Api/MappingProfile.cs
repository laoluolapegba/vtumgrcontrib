using AutoMapper;
using Chams.Vtumanager.Provisioning.Api.ViewModels;
using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.BulkSMS;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Carpaddy;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Cornerstone;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice;
using Chams.Vtumanager.Provisioning.Entities.IbadanDisco;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco.AbujaPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco.AbujaPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.BillpaymentRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.BulkSMS.BulkSMSRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Cornerstone.CornerstoneRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric.EkoElectricPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric.EkoElectricPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric.IkejaElectricPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric.IkejaElectricPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric.IkejaElectricRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric.IkejaElectricTokenPurchaseRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice.DstvRenewRequest;
using static Chams.Vtumanager.Provisioning.Entities.IbadanDisco.IbadanDiscoPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.IbadanDisco.IbadanDiscoPrepaidRequest;


namespace Chams.Vtumanager.Provisioning.Api
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BillpaymentRequestDetails, AbujaPostpaidRequestDetails>();
            CreateMap<BillpaymentRequest, AbujaPostpaidRequest>();

            CreateMap<BillpaymentRequestDetails, AbujaPrepaidDetails>();
            CreateMap<BillpaymentRequest, AbujaPrepaidRequest>();

            CreateMap<BillpaymentRequestDetails, BulkSMSDetails>();
            CreateMap<BillpaymentRequest, BulkSMSRequest>();

            CreateMap<BillpaymentRequest, CarpaddyRequest>();

            CreateMap<BillpaymentRequestDetails, CornerstoneRequestDetails>();
            CreateMap<BillpaymentRequest, CornerstoneRequest>();

            CreateMap<BillpaymentRequestDetails, EkoElectricPostpaidDetails>();
            CreateMap<BillpaymentRequest, EkoElectricPostpaidRequest>();

            CreateMap<BillpaymentRequestDetails, EkoElectricPrepaidRequestDetails>();
            CreateMap<BillpaymentRequest, EkoElectricPrepaidRequest>();

            CreateMap<BillpaymentRequestDetails, IbadanDiscoPostpaidRequestDetails>();
            CreateMap<BillpaymentRequest, IbadanDiscoPostpaidRequest>();

            CreateMap<BillpaymentRequestDetails, IbadanDiscoPrepaidRequestDetails>();
            CreateMap<BillpaymentRequest, IbadanDiscoPrepaidRequest>();
            //
            
            CreateMap<BillpaymentRequestDetails, IkejaElectricPostpaidDetails>();
            CreateMap<BillpaymentRequest, IkejaElectricPostpaidRequest>();

            CreateMap<BillpaymentRequestDetails, IkejaElectricTokenPurchaseDetails>();
            CreateMap<BillpaymentRequest, IkejaElectricTokenPurchaseRequest>();

            CreateMap<BillpaymentRequestDetails, DstvRequestDetails>();
            CreateMap<BillpaymentRequest, DstvRenewRequest>();
                //.ForMember(a=>a.details.amount, opt => opt.MapFrom(src => (int)src.details.amount));



        }
    }
}
