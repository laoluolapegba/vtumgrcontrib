namespace Chams.Vtumanager.Fulfillment.NineMobile.Services
{
    public interface IMailHelper
    {
        bool SendMail(string subject, string mailBody, string recipientAddress);
    }
}