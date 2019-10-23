using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace BPTrackers.Models
{
    public static class NP
    {
        public static RootObject GetDataParcel(string track)
        {
            RootObject deserializedProduct = new RootObject();
            WebRequest request = WebRequest.Create("https://api.novaposhta.ua/v2.0/json/");
            request.Method = "POST"; // для отправки используется метод Post
                                     // данные для отправки
            string data = $"{{\"apiKey\": \"222ec18b9e04b89eb78b285db6f14e3c\", \"modelName\": \"TrackingDocument\", \"calledMethod\": \"getStatusDocuments\", \"methodProperties\": {{\"Documents\": [{{\"DocumentNumber\": \"{track}\", \"Phone\": \"\"}}]}}}}";
            // преобразуем данные в массив байтов
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);
            // устанавливаем тип содержимого - параметр ContentType
            request.ContentType = "application/x-www-form-urlencoded";
            // Устанавливаем заголовок Content-Length запроса - свойство ContentLength
            request.ContentLength = byteArray.Length;

            //записываем данные в поток запроса
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string str = reader.ReadToEnd();
                    deserializedProduct = JsonConvert.DeserializeObject<RootObject>(str);
                }
            }
            response.Close();
            return deserializedProduct;
        }
    }

    public class Datum
    {
        public string Number { get; set; }
        public int Redelivery { get; set; }
        public string RedeliverySum { get; set; }
        public string RedeliveryNum { get; set; }
        public string RedeliveryPayer { get; set; }
        public string OwnerDocumentType { get; set; }
        public string LastCreatedOnTheBasisDocumentType { get; set; }
        public string LastCreatedOnTheBasisPayerType { get; set; }
        public string LastCreatedOnTheBasisDateTime { get; set; }
        public string LastTransactionStatusGM { get; set; }
        public string LastTransactionDateTimeGM { get; set; }
        public string DateCreated { get; set; }
        public int CheckWeight { get; set; }
        public int SumBeforeCheckWeight { get; set; }
        public string PayerType { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientDateTime { get; set; }
        public string ScheduledDeliveryDate { get; set; }
        public string PaymentMethod { get; set; }
        public string CargoDescriptionString { get; set; }
        public string CargoType { get; set; }
        public string CitySender { get; set; }
        public string CityRecipient { get; set; }
        public string WarehouseRecipient { get; set; }
        public string CounterpartyType { get; set; }
        public string AfterpaymentOnGoodsCost { get; set; }
        public string ServiceType { get; set; }
        public string UndeliveryReasonsSubtypeDescription { get; set; }
        public int WarehouseRecipientNumber { get; set; }
        public string LastCreatedOnTheBasisNumber { get; set; }
        public string WarehouseRecipientInternetAddressRef { get; set; }
        public string MarketplacePartnerToken { get; set; }
        public string ClientBarcode { get; set; }
        public string SenderAddress { get; set; }
        public string RecipientAddress { get; set; }
        public string CounterpartySenderDescription { get; set; }
        public string CounterpartyRecipientDescription { get; set; }
        public string CounterpartySenderType { get; set; }
        public string DateScan { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentStatusDate { get; set; }
        public string AmountToPay { get; set; }
        public string AmountPaid { get; set; }
        public string LastAmountTransferGM { get; set; }
        public string LastAmountReceivedCommissionGM { get; set; }
        public double DocumentCost { get; set; }
        public double DocumentWeight { get; set; }
        public string AnnouncedPrice { get; set; }
        public string UndeliveryReasonsDate { get; set; }
        public string RecipientWarehouseTypeRef { get; set; }
        public string RedeliveryPaymentCardDescription { get; set; }
        public string OwnerDocumentNumber { get; set; }
        public string InternationalDeliveryType { get; set; }
        public string WarehouseSender { get; set; }
        public string WarehouseRecipientRef { get; set; }
        public string LoyaltyCardSender { get; set; }
        public string LoyaltyCardRecipient { get; set; }
        public string CreatedOnTheBasis { get; set; }
        public string DeliveryTimeframe { get; set; }
        public string VolumeWeight { get; set; }
        public string SeatsAmount { get; set; }
        public string ActualDeliveryDate { get; set; }
        public string RefCitySender { get; set; }
        public string RefCityRecipient { get; set; }
        public string CardMaskedNumber { get; set; }
        public string BarcodeRedBox { get; set; }
        public List<object> Packaging { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string RefEW { get; set; }
        public string RedeliveryPaymentCardRef { get; set; }
        public string DatePayedKeeping { get; set; }
        public string OnlineCreditStatusCode { get; set; }
        public string OnlineCreditStatus { get; set; }
    }

    public class Warning
    {
        public string ID_59000441053628 { get; set; }
    }

    public class RootObject
    {
        public bool success { get; set; }
        public List<Datum> data { get; set; }
        public List<object> errors { get; set; }
        public List<Warning> warnings { get; set; }
        public List<object> info { get; set; }
        public List<object> messageCodes { get; set; }
        public List<object> errorCodes { get; set; }
        public List<object> warningCodes { get; set; }
        public List<object> infoCodes { get; set; }
    }
}