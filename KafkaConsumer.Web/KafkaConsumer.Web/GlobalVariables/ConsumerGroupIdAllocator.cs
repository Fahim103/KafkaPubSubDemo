using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace KafkaConsumer.Web.GlobalVariables
{
    class KafkaGroupIdData
    {
        public string GroupId { get; set; }
        public bool IsAvailable { get; set; }
    }

    public static class ConsumerGroupIdAllocator
    { 
        private static string MutexName = "Fahim-Kafka-Mutex";
        private static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string FileName = "GroupIDList.json";
        private static string FilePath = Path.Combine(BaseDirectory, FileName);

        public static string GetGroupId()
        {
            var mutex = GetMutex();
            var isAcquired = true;

            try
            {
                var data = LoadJson();
                var avaialbleGroupItem = data.FindIndex(x => x.IsAvailable == true);

                if (avaialbleGroupItem != -1)
                {
                    // Update the Availabilty of the GroupID to be returned
                    data[avaialbleGroupItem].IsAvailable = false;
                }
                else
                {
                    string newGroupIdToAddToFile = Guid.NewGuid().ToString();
                    data.Add(new KafkaGroupIdData()
                    {
                        GroupId = newGroupIdToAddToFile,
                        IsAvailable = false
                    });

                    avaialbleGroupItem = data.Count - 1;
                }

                // Group ID to return to the new process
                var groupId = data[avaialbleGroupItem].GroupId;

                WriteJsonData(data);

                mutex.ReleaseMutex();
                isAcquired = false;
                return groupId;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if(isAcquired)
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        public static void ReleaseGroupId(string groupId)
        {
            Mutex mutex = GetMutex();

            try
            {
                var data = LoadJson();
                var index = data.FindIndex(x => x.GroupId.Equals(groupId));
                data[index].IsAvailable = true;

                WriteJsonData(data);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        static void WriteJsonData(List<KafkaGroupIdData> data)
        {
            using (StreamWriter writer = new StreamWriter(path: FilePath))
            {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                writer.Write(jsonData);
            }
        }

        static List<KafkaGroupIdData> LoadJson()
        {
            using (StreamReader reader = new StreamReader(path: FilePath))
            {
                string jsonData = reader.ReadToEnd();
                List<KafkaGroupIdData> kafkaGroupIdDatas = JsonConvert.DeserializeObject<List<KafkaGroupIdData>>(jsonData);
                return kafkaGroupIdDatas;
            }
        }

        private static Mutex GetMutex()
        {
            if (Mutex.TryOpenExisting(MutexName, out Mutex mutex))
            {
                mutex.WaitOne();
            }
            else
            {
                mutex = new Mutex(initiallyOwned: true, name: MutexName, createdNew: out bool mutexCreated);

                if (!mutexCreated)
                {
                    mutex.WaitOne();
                }
            }

            return mutex;
        }
    }
}