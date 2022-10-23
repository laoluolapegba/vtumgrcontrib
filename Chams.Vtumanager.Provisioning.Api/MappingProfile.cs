using AutoMapper;
using Chams.Vtumanager.Provisioning.Api.ViewModels;
using Chams.Vtumanager.Provisioning.Entities.BillPayments;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.BulkSMS;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Carpaddy;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Cornerstone;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.JosElectricity;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Kaduna;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Kedco;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Mutual;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.PortharcourtElectric;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Showmax;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Smile;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Spectranet;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Startimes;
using Chams.Vtumanager.Provisioning.Entities.BillPayments.Waec;
using Chams.Vtumanager.Provisioning.Entities.IbadanDisco;
using Chams.Vtumanager.Provisioning.Services.BillPayments.Jamb;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco.AbujaPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.AbujaDisco.AbujaPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.BillpaymentRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.BulkSMS.BulkSMSRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Cornerstone.CornerstoneRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric.EkoElectricPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.EkoElectric.EkoElectricPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric.IkejaElectricPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.IkejaElectric.IkejaElectricTokenPurchaseRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.JosElectricity.JosElectricPostPaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.JosElectricity.JosElectricPrePaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Kaduna.KadunaElectricPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Kaduna.KadunaElectricPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Kedco.KedcoElectricPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Kedco.KedcoElectricPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice.DstvBoxOfficeRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice.DstvRenewRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice.DstvRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice.GotvRenew;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Multichoice.GotvRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Mutual.MutualMortorInsuranceRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.PortharcourtElectric.PortHarcourtElectricPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.PortharcourtElectric.PortHarcourtElectricPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Showmax.ShowmaxVoucherRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Smile.SmileCommBundleRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Smile.SmileCommRechargeRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Spectranet.SpectranetPaymentPlanRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Spectranet.SpectranetPINRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Spectranet.SpectranetRefillRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Startimes.StartimesRequest;
using static Chams.Vtumanager.Provisioning.Entities.BillPayments.Waec.WaecPINRequest;
using static Chams.Vtumanager.Provisioning.Entities.IbadanDisco.IbadanDiscoPostpaidRequest;
using static Chams.Vtumanager.Provisioning.Entities.IbadanDisco.IbadanDiscoPrepaidRequest;
using static Chams.Vtumanager.Provisioning.Services.BillPayments.Jamb.JambPINRequest;

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

            CreateMap<BillpaymentRequestDetails, JambCandidatePINDetails>();
            CreateMap<BillpaymentRequest, JambPINRequest>();

            CreateMap<BillpaymentRequestDetails, JosElectricityPrePaidDetails>();
            CreateMap<BillpaymentRequest, JosElectricPrePaidRequest>();


            CreateMap<BillpaymentRequestDetails, JosElectricityPostPaidDetails>();
            CreateMap<BillpaymentRequest, JosElectricPostPaidRequest>();


            CreateMap<BillpaymentRequestDetails, KadunaElectricPrepaidDetails>();
            CreateMap<BillpaymentRequest, KadunaElectricPrepaidRequest>();

            CreateMap<BillpaymentRequestDetails, KadunaElectricPostpaidDetails>();
            CreateMap<BillpaymentRequest, KadunaElectricPostpaidRequest>();
            //
            CreateMap<BillpaymentRequestDetails, KedcoElectricityPrepaidDetails>();
            CreateMap<BillpaymentRequest, KedcoElectricPrepaidRequest>();


            CreateMap<BillpaymentRequestDetails, KedcoElectricityPostpaidDetails>();
            CreateMap<BillpaymentRequest, KedcoElectricPostpaidRequest>();



            CreateMap<BillpaymentRequestDetails, BoxOfficerequestDetails>()
                .ForMember(a => a.amount, opt => opt.MapFrom(src => (int)src.amount));
            CreateMap<BillpaymentRequestDetails, DstvRequestDetails>()
                .ForMember(a => a.amount, opt => opt.MapFrom(src => (int)src.amount)); 
            CreateMap<BillpaymentRequestDetails, DstvRenewRequestDetails>()
                .ForMember(a => a.amount, opt => opt.MapFrom(src => (int)src.amount));

            CreateMap<BillpaymentRequest, DstvRequest>();
            CreateMap<BillpaymentRequest, DstvRenewRequest>();
            CreateMap<BillpaymentRequest, DstvBoxOfficeRequest>();
            
            CreateMap<BillpaymentRequestDetails, GotvRequestDetails>(); 
            CreateMap<BillpaymentRequest, GotvRequest>();

            CreateMap<BillpaymentRequestDetails, GotvRenewDetails>();
            CreateMap<BillpaymentRequest, GotvRenew>();


            CreateMap<BillpaymentRequestDetails, MutualMortorInsurancDetails>();
            CreateMap<BillpaymentRequest, MutualMortorInsuranceRequest>();

            CreateMap<BillpaymentRequestDetails, PortHarcourtElectricPrepaidDetails>();
            CreateMap<BillpaymentRequest, PortHarcourtElectricPrepaidRequest>();

            CreateMap<BillpaymentRequestDetails, PortHarcourtElectricPostpaidDetails>();
            CreateMap<BillpaymentRequest, PortHarcourtElectricPostpaidRequest>();


            CreateMap<BillpaymentRequestDetails, ShowmaxVoucherDetails>();
            CreateMap<BillpaymentRequest, ShowmaxVoucherRequest>();

            CreateMap<BillpaymentRequestDetails, SmileCommBundleDetails>();
            CreateMap<BillpaymentRequest, SmileCommBundleRequest>();


            //SpectranetPaymentPlanRequest
            CreateMap<BillpaymentRequestDetails, SmileRechargeDetails>();
            CreateMap<BillpaymentRequest, SmileCommRechargeRequest>();

            CreateMap<BillpaymentRequestDetails, SpectranetPaymentPlanDetails>(); 
            CreateMap<BillpaymentRequest, SpectranetPaymentPlanRequest>();

            CreateMap<BillpaymentRequestDetails, SpectranetPINDetails>(); 
            CreateMap<BillpaymentRequest, SpectranetPINRequest>();

            CreateMap<BillpaymentRequestDetails, SpectranetRefillDetails>();
            CreateMap<BillpaymentRequest, SpectranetRefillRequest>(); 

            CreateMap<BillpaymentRequestDetails, StartTimesDetails>();
            CreateMap<BillpaymentRequest, StartimesRequest>();

            CreateMap<BillpaymentRequestDetails, WaecPINDetails>();
            CreateMap<BillpaymentRequest, WaecPINRequest>();

            //.ForMember(a=>a.details.amount, opt => opt.MapFrom(src => (int)src.details.amount));



        }
    }
}
