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
        private object _locker = new object();

        /// <summary>
        /// Creates a LineStateSaver which doesn't save its state to a file. (In memory only)
        /// </summary>
        public LineStateSaver() : this(null)
        {
        }

        /// <summary>
        /// Creates a line state saver which saves its state to a file
        /// </summary>
        /// <param name="fileName">Filename of the json file to save the state data to</param>
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
            if (!string.IsNullOrWhiteSpace(_fileName) && File.Exists(_fileName))
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
            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                var serializedData = JsonConvert.SerializeObject(_lineStateData, Formatting.Indented, new JsonSerializerSettings
                {
                    //TypeNameHandling = TypeNameHandling.All
                });

                lock (_locker)
                {
                    using (var writer = new StreamWriter(new FileStream(_fileName, FileMode.Create, FileAccess.Write, FileShare.Read)))
                    {
                        writer.Write(serializedData);
                    }
                }
            }
        }

        public async Task<T> Save<T>(Expression<Func<Task<T>>> function)
        {
            LineState callData = CreateCallData(function);

            var foundObject = _lineStateData.Data.FirstOrDefault(t => t.IsEqualTo(callData));

            if (foundObject != null)
            {
                return ObtainExistingResult<T>(callData, foundObject);
            }

            var func = function.Compile();
            var result = await func();
            return StoreNewlyCalledResult(callData, result);
        }

        public T Save<T>(Expression<Func<T>> function)
        {
            LineState callData = CreateCallData(function);

            var foundObject = _lineStateData.Data.FirstOrDefault(t => t.IsEqualTo(callData));

            if (foundObject != null)
            {
                return ObtainExistingResult<T>(callData, foundObject);
            }

            var func = function.Compile();
            var result = func();
            return StoreNewlyCalledResult(callData, result);
        }

        private static LineState CreateCallData<T>(Expression<Func<T>> function)
        {
            var methodCallBody = function.Body as MethodCallExpression;
            if (methodCallBody == null)
            {
                throw new InvalidOperationException($"Could not cast function.body to type MethodCallExpression. Actual type: {function?.Body?.GetType()}");
            }

            return new LineState(methodCallBody);
        }

        private static T ObtainExistingResult<T>(LineState callData, LineState foundObject)
        {
            var retval = JsonConvert.DeserializeObject<T>(foundObject.Output);
            Console.WriteLine($"Found method call to {callData} in storage. Returning result: {foundObject.Output}");
            return retval;
        }

        private T StoreNewlyCalledResult<T>(LineState callData, T result)
        {
            callData.Output = JsonConvert.SerializeObject(result);
            _lineStateData.Data.Add(callData);
            SaveData();

            Console.WriteLine($"Could not find method call to {callData} in storage. Saving result: {callData.Output}");

            return result;
        }
    }
}
