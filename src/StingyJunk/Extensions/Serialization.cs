namespace StingyJunk.Extensions
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Newtonsoft.Json;

    public static class Serialization
    {
        /// <summary>
        ///     I hate the attributes and refactoring this binary formatter makes me pick
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T CloneByBinaryFormatter<T>(this T instance)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, instance);
                ms.Position = 0;

                return (T) formatter.Deserialize(ms);
            }
        }

        public static T CloneByJson<T>(this T instance) where T : class
        {
            var json = JsonConvert.SerializeObject(instance);
            var newInstance = JsonConvert.DeserializeObject(json, typeof(T));
            return newInstance as T;
        }
    }
}