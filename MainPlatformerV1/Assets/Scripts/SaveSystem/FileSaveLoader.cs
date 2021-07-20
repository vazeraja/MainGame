using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace TN.SaveSystem {

    public interface ISaveLoader {
        
    }
    public class FileSaveLoader : ISaveLoader {
        #region Json Serialization

        public static bool WriteToFile(string fileName, string fileContents) {
            if (!Directory.Exists(Application.persistentDataPath + "/saves/")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
            }

            string savePath = Application.persistentDataPath + "/saves/" + fileName;

            try {
                File.WriteAllText(savePath, fileContents);
                return true;
            }
            catch (Exception e) {
                Debug.LogError($"Failed to write to {savePath} with exception {e}");
                return false;
            }
        }

        public static bool LoadFromFile(string fileName, out string result) {
            if (!Directory.Exists(Application.persistentDataPath + "/saves/")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
            }

            string savePath = Application.persistentDataPath + "/saves/" + fileName;

            try {
                result = File.ReadAllText(savePath);
                return true;
            }
            catch (Exception e) {
                Debug.LogError($"Failed to read from {savePath} with exception {e}");
                result = "";
                return false;
            }
        }

        #endregion

        #region Xml Serialization

        public static bool Save<T>(string saveName, T saveData) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            if (!Directory.Exists(Application.persistentDataPath + "/saves")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            }

            string path = Application.persistentDataPath + "/saves/" + saveName + ".save";
            TextWriter writer = new StreamWriter(path);

            serializer.Serialize(writer, saveData);
            writer.Close();

            return true;
        }

        public static T Load<T>(string path) {
            if (!File.Exists(path)) {
                throw new Exception();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.UnknownNode += serializer_UnknownNode;
            serializer.UnknownAttribute += serializer_UnknownAttribute;

            FileStream file = File.Open(path, FileMode.Open);

            try {
                T save = (T) serializer.Deserialize(file);
                file.Close();
                return save;
            }
            catch {
                Debug.LogErrorFormat("Failed to load file {0}", path);
                file.Close();
                throw new Exception();
            }
        }

        private static void serializer_UnknownNode(object sender, XmlNodeEventArgs e) {
            Debug.Log("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e) {
            System.Xml.XmlAttribute attr = e.Attr;
            Debug.Log("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }

        #endregion
    }
}