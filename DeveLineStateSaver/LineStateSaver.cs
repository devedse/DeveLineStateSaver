using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeveLineStateSaver
{
    public class LineStateSaver
    {
        private LineStateData _lineStateData = new LineStateData();
        private string _fileName;

        public LineStateSaver(string fileName)
        {
            _fileName = fileName;

            LoadData();
        }

        public void Clear()
        {
            _lineStateData.Data.Clear();
            SaveData();
        }

        private void LoadData()
        {
            if (File.Exists(_fileName))
            {
                using (var reader = new StreamReader(new FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    var allText = reader.ReadToEnd();
                    _lineStateData = JsonConvert.DeserializeObject<LineStateData>(allText);
                }
            }
        }

        private void SaveData()
        {
            var serializedData = JsonConvert.SerializeObject(_lineStateData, Formatting.Indented, new JsonSerializerSettings
            {
                //TypeNameHandling = TypeNameHandling.All
            });

            using (var writer = new StreamWriter(new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.Read)))
            {
                writer.Write(serializedData);
            }
        }

        //public async Task<T> Save<T>(Func<T> function)
        //{

        //    return await function();
        //}

        public T Save<T>(Expression<Func<T>> function)
        {
            var methodCallBody = function.Body as MethodCallExpression;
            if (methodCallBody == null)
            {
                throw new InvalidOperationException($"Could not cast function.body to type MethodCallExpression. Actual type: {function?.Body?.GetType()}");
            }

            var callData = new LineState(methodCallBody);

            var foundObject = _lineStateData.Data.FirstOrDefault(t => t.IsEqualTo(callData));

            if (foundObject != null)
            {
                var retval = JsonConvert.DeserializeObject<T>(foundObject.Output);
                Console.WriteLine($"Found method call to {callData} in storage. Returning result: {foundObject.Output}");
                return retval;
            }


            var func = function.Compile();
            var result = func();
            callData.Output = JsonConvert.SerializeObject(result);
            _lineStateData.Data.Add(callData);
            SaveData();

            Console.WriteLine($"Could not find method call to {callData} in storage. Saving result: {callData.Output}");

            return result;
        }
    }
}
