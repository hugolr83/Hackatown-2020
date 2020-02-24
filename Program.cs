using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace ConsoleApp3
{
    class Program
    {
        static bool post;
        static ISubscriptionClient subscriptionClient;
        static ISubscriptionClient subscriptionClient_notif;
        static List<string> VoituresRecherches;
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Listner started.");


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "ZXF1aXBlMTY6TE1KN1NZQ3BGS3R6YzN6Yw==");


            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {






            string ServiceBusConnectionString = "Endpoint=sb://licenseplatepublisher.servicebus.windows.net/;SharedAccessKeyName=ConsumeReads;SharedAccessKey=VNcJZVQAVMazTAfrssP6Irzlg/pKwbwfnOqMXqROtCQ=";
            string TopicName = "licenseplateread";
            string SubscriptionName = "fbS8qDfztJHYbf2G";

            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

            updateVoitures_Recherche();
            ServiceBusConnectionString = "Endpoint=sb://licenseplatepublisher.servicebus.windows.net/;SharedAccessKeyName=listeneronly;SharedAccessKey=w+ifeMSBq1AQkedLCpMa8ut5c6bJzJxqHuX9Jx2XGOk=";
            TopicName = "wantedplatelistupdate";
            SubscriptionName = "8wQLDabncqUFjdTk";

            subscriptionClient_notif = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);



            //liste voitures recherches


            // Console.WriteLine($"Received message: UserInfo:{resp}");


            // Register subscription message handler and receive messages in a loop.
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await subscriptionClient.CloseAsync();

            await subscriptionClient_notif.CloseAsync();

        }

        static async Task sendCombi(ServiceBusMessageSent message, int i,bool post)
        {
            
            if (post)
            {
                var serializeUser = JsonConvert.SerializeObject(message);

                var url = "https://licenseplatevalidator.azurewebsites.net/api/lpr/platelocation";
                //la data envoye
                var data = new StringContent(serializeUser, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);
                Console.WriteLine();
            }
            


            ////////
            ServiceBusMessageSent messageCopy = message;
            StringBuilder strB;
            if (i != message.LicensePlate.Length)
            {
                switch (message.LicensePlate[i])
                {

                    case 'B':

                        strB = new StringBuilder(messageCopy.LicensePlate);
                        strB[i] = '8';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, false);
                        await sendCombi(message, i++, true);
                        break;
                    case 'E':
                        strB = new StringBuilder(messageCopy.LicensePlate);
                        strB[i] = 'F';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        await sendCombi(message, i++, false);
                        break;
                    case 'K':
                        strB = new StringBuilder(messageCopy.LicensePlate);
                        strB[i] = 'X';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        await sendCombi(message, i++, false);
                        strB[i] = 'Y';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        break;
                    case 'I':
                        strB = new StringBuilder(messageCopy.LicensePlate);
                        strB[i] = '1';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        await sendCombi(message, i++, false);
                        strB[i] = 'T';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        strB[i] = 'J';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        break;

                    case 'S':
                        strB = new StringBuilder(messageCopy.LicensePlate);
                        strB[i] = '5';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        await sendCombi(message, i++, false);
                        break;
                    case 'O':
                        strB = new StringBuilder(messageCopy.LicensePlate);
                        strB[i] = 'D';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        await sendCombi(message, i++, false);
                        strB[i] = 'Q';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        strB[i] = '0';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        break;

                    case 'P':
                        strB = new StringBuilder(messageCopy.LicensePlate);
                        strB[i] = 'R';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        await sendCombi(message, i++, false);
                        break;


                    case 'Z':
                        strB = new StringBuilder(messageCopy.LicensePlate);
                        strB[i] = '2';
                        messageCopy.LicensePlate = strB.ToString();
                        await sendCombi(messageCopy, i++, true);
                        await sendCombi(message, i++, false);
                        break;
                }

            }
        }
            
                
                
                


                
         



           
        static async void sendVoitures_Recherches(string messageBody,string uri)
        {
            //convertir en objet serviceBus Message
            var serviceBusMessage = JsonConvert.DeserializeObject<ServiceBusMessageSent>(messageBody);

            serviceBusMessage.ContextImageReference = uri;
            await sendCombi(serviceBusMessage,0, true);

            //la faut chercher dans 

            //enlever limage
            

            // APi ou on envoit la voiture


            
           

        }
        static async void updateVoitures_Recherche(){
            var client_Voitures_Recherches = new HttpClient();

            var url_recherche = "https://licenseplatevalidator.azurewebsites.net/api/lpr/wantedplates";

            client_Voitures_Recherches.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "ZXF1aXBlMTY6TE1KN1NZQ3BGS3R6YzN6Yw ==");
            var response = await client_Voitures_Recherches.GetAsync(url_recherche);

            response.EnsureSuccessStatusCode();

            string temp= await response.Content.ReadAsStringAsync();

            VoituresRecherches = JsonConvert.DeserializeObject<List<string>>(temp);

         }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler);

            // Register the function that processes messages.


            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            subscriptionClient_notif.RegisterMessageHandler(ProcessMessagesAsync_notif, messageHandlerOptions);
        }
        static bool estRecherche(string messageBody)
        {
            var serviceBusMessage = JsonConvert.DeserializeObject<ServiceBusMessage>(messageBody);
            
                
                return VoituresRecherches.Contains(serviceBusMessage.LicensePlate);
                    
            
            
            
           

        }
        
        /*static string UploadImage(byte[] imageByteArr)
        {
            // Retrieve storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=objectif3;AccountKey=9R5vTbGk9DSWcSnabVFpYYnfieutj+CNG5l6Y92rZVL5mKR1Q1KJTZ4k51f+Af2xTml7HBiR86AZVdbscNmeQA==;EndpointSuffix=core.windows.net");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("objectif3");
            container.SetPermissionsAsync(
    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Create the container if it doesn't already exist.
            container.CreateIfNotExistsAsync().ConfigureAwait(false);



            var docId = Guid.NewGuid().ToString();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(docId);

            blockBlob.UploadFromByteArrayAsync(imageByteArr, 0, imageByteArr.Length);

            blockBlob.Properties.ContentType = "image/jpg";
            blockBlob.SetPropertiesAsync();

            return blockBlob.Uri.ToString();
        }*/
        static async Task<string> UploadPhotoAsync(byte[] photobytes, string photoName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=objectif3;AccountKey=9R5vTbGk9DSWcSnabVFpYYnfieutj+CNG5l6Y92rZVL5mKR1Q1KJTZ4k51f+Af2xTml7HBiR86AZVdbscNmeQA==;EndpointSuffix=core.windows.net");
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("objectif3");
            await container.CreateIfNotExistsAsync();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            await container.SetPermissionsAsync(containerPermissions);
            CloudBlockBlob photo = container.GetBlockBlobReference(photoName);
            await photo.UploadFromByteArrayAsync(photobytes, 0, photobytes.Length);
            return photo.Uri.ToString();
        }
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {


            var messageBody = Encoding.UTF8.GetString(message.Body);

            //enlever l'image du json
            if (estRecherche(messageBody)){

                var serviceBusMessage = JsonConvert.DeserializeObject<MessageEntier>(messageBody);
                byte[] decodedByteArray = Convert.FromBase64String(serviceBusMessage.ContextImageJpg);

                string url=await UploadPhotoAsync(decodedByteArray, serviceBusMessage.LicensePlate);
                sendVoitures_Recherches(messageBody,url);
                
                // This is your Base64-encoded bute[]

               


                // This work because all Base64-encoding is done with pure ASCII characters

            }


            Console.WriteLine($"Received message: UserInfo:{Encoding.UTF8.GetString(message.Body)}");
            //Console.WriteLine(dec)

            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static async Task ProcessMessagesAsync_notif(Message message, CancellationToken token)
        {


            
            //nbrVoituresRecherches = serviceBusMessage.TotalWantedCount;


            updateVoitures_Recherche();

            await subscriptionClient_notif.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var exception = exceptionReceivedEventArgs.Exception;

            return Task.CompletedTask;
        }

        public class ServiceBusMessage
        {
            /*
            public string Id { get; set; }
            public string Type { get; set; }
            public string Content { get; set; }
            */

            
            public string LicensePlateCaptureTime { get; set; }
            public string LicensePlate { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            
        }
        public class ServiceBusMessageSent
        {
            public string LicensePlateCaptureTime { get; set; }
            public string LicensePlate { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string ContextImageReference { get; set; }
        }

        public class MessageEntier
        {

            public string LicensePlateCaptureTime { get; set; }
            public string LicensePlate { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }

            public string ContextImageJpg { get; set; }
            public string LicensePlateImageJpg { get; set; }


        }
    }
}
