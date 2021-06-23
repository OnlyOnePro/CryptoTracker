﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace UWP.Helpers {
    public class LocalStorageHelper {
        public static async Task SaveObject<T>(string key, T obj) {
            try {
                var savedStuffFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                    key, CreationCollisionOption.ReplaceExisting);

                using (Stream writeStream = await savedStuffFile.OpenStreamForWriteAsync()) {
                    DataContractSerializer stuffSerializer = new DataContractSerializer(typeof(T));
                    stuffSerializer.WriteObject(writeStream, obj);
                    await writeStream.FlushAsync();
                    writeStream.Dispose();
                }
            }
            catch (Exception ex) {
                var _ = ex.Message;
            }
        }

        public static async Task<T> ReadObject<T>(string key) {
            try {
                var readStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(key);

                DataContractSerializer stuffSerializer = new DataContractSerializer(typeof(T));
                var setResult = (T)stuffSerializer.ReadObject(readStream);
                await readStream.FlushAsync();
                readStream.Dispose();

                return setResult;
            }
            catch (Exception ex) {
                var _ = ex.Message;
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

    }
}
